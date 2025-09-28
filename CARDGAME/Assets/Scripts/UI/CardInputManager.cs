using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

//! Runs the arrow/keypress sequence UI modal and validates input.
//? Singleton for convenience: CardInputManager.Instance.StartSequence(...)
public class CardInputManager : MonoBehaviour
{
    //! UI references (assign in inspector)
    public GameObject modalRoot;      // root panel to show/hide
    public TextMeshProUGUI  sequenceText;         // shows the sequence and progress
    public Image timerBar;            // filled image showing sequence time remaining
    public TextMeshProUGUI  progressText;         // optional "2 / 4" style
    public float defaultTimeout = 2f; // fallback

    private Action onSuccess;
    private Action onFailure;

    private InputKey[] currentSequence;
    private int index = 0;
    private float timer = 0f;
    private float totalTime;  // store starting limit // * NEW    
    private bool running = false;

    public static CardInputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (modalRoot) modalRoot.SetActive(false);
    }

    private void Update()
    {
        // Debug all key presses
        if (Input.anyKeyDown)
            Debug.Log($"Any key pressed: {Input.inputString}");
            
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("SPACE pressed");
        

        timer -= Time.deltaTime;
        // //Debug.Log(timer); // ! DEBUGGING 

        if (timerBar)
            timerBar.fillAmount = Mathf.Clamp01(timer / Mathf.Max(0.0001f, timer + 0f + 0f)); // guard

        // check for key presses we support
        InputKey? pressed = DetectPressedKey();
        if (pressed.HasValue)
        {
            HandleKey(pressed.Value);
        }

        // time out
        if (timer <= 0f)
        {
            FailSequence();
        }

        // * NEW:
        if (timerBar)
            timerBar.fillAmount = Mathf.Clamp01(timer / totalTime);

    }

    //! Start the modal sequence. The caller should have already paused card lifetime.
    public void StartSequence(CardData data, Action onSuccessCallback, Action onFailureCallback)
    {
        if (running) return; // already busy
        currentSequence = data.inputSequence ?? new InputKey[0];
        onSuccess = onSuccessCallback;
        onFailure = onFailureCallback;
        index = 0;

        /*
        timer = Mathf.Max(0.01f, data.timeLimit);
        */

        // * NEW:
        totalTime = Mathf.Max(0.01f, data.timeLimit);
        timer = totalTime;


        running = true;
        if (modalRoot) modalRoot.SetActive(true);
        RefreshUI();
    }

    //! Cancel / fail from outside
    public void CancelSequence()
    {
        FailSequence();
    }

    private void RefreshUI()
    {
        if (sequenceText)
            sequenceText.text = SequenceProgressString(currentSequence, index);
        if (progressText)
            progressText.text = $"{index} / {currentSequence.Length}";
        if (timerBar)
            timerBar.fillAmount = 1f;
    }

    private void HandleKey(InputKey key)
    {
        if (!running) return;

        // correct?
        if (index < currentSequence.Length && key == currentSequence[index])
        {
            index++;
            RefreshUI();

            if (index >= currentSequence.Length)
            {
                // success
                SucceedSequence();
            }
        }
        else
        {
            // wrong key => failure
            FailSequence();
        }
    }

    private void SucceedSequence()
    {
        running = false;
        if (modalRoot) modalRoot.SetActive(false);
        onSuccess?.Invoke();
        ClearCallbacks();
    }

    private void FailSequence()
    {
        running = false;
        if (modalRoot) modalRoot.SetActive(false);
        onFailure?.Invoke();
        ClearCallbacks();
    }

    private void ClearCallbacks()
    {
        onSuccess = null;
        onFailure = null;
        currentSequence = null;
        index = 0;
    }

    //! Convert sequence + progress to a readable string where completed keys are wrapped with [✓]
    private string SequenceProgressString(InputKey[] seq, int progress)
    {
        if (seq == null || seq.Length == 0) return "(empty)";
        var sb = new StringBuilder();
        for (int i = 0; i < seq.Length; i++)
        {
            string s = KeyToSymbol(seq[i]);
            if (i < progress) sb.Append($"[{s}] "); // completed
            else if (i == progress) sb.Append($"({s}) "); // next to press
            else sb.Append($"{s} ");
        }
        return sb.ToString().Trim();
    }

    private string KeyToSymbol(InputKey k) =>
        k switch
        {
            InputKey.Left => "<",
            InputKey.Right => ">",
            InputKey.Up => "^",
            InputKey.Down => "v",
            _ => k.ToString()
        };

    //! Detect which supported key was pressed this frame
    private InputKey? DetectPressedKey()
    {
        // Debug.Log("Detecting key press...");
        // Debug.Log("Input.anyKeyDown: " + Input.anyKeyDown);
        // arrow keys
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return InputKey.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return InputKey.Right;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return InputKey.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return InputKey.Down;

        // letter keys
        if (Input.GetKeyDown(KeyCode.A)) return InputKey.A;
        if (Input.GetKeyDown(KeyCode.S)) return InputKey.S;
        if (Input.GetKeyDown(KeyCode.D)) return InputKey.D;
        if (Input.GetKeyDown(KeyCode.W)) return InputKey.W;

        // cancel
        if (Input.GetKeyDown(KeyCode.Escape)) return null; // Escape will be treated by caller as cancel via CancelSequence() if desired

        return null;
    }
}
