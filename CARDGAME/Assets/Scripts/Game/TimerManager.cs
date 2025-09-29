using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Global countdown timer with a vertical fill bar and numeric text.
/// Call TimerManager.Instance.AddTime(float) to add time (clamped to max).
/// </summary>
public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    [Header("Timer Settings")]
    public float maxTime = 60f;           // seconds when bar is full
    [SerializeField] private float currentTime;
    public float timeDrainRate = 1f;      // seconds drained per real second

    [Header("UI References")]
    public Image timerFillImage;          // assign TimerFill (Image Type=Filled, Vertical, Origin=Top)
    public TextMeshProUGUI timerText;     // assign TimerText

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {

        // ✅ Apply global time multiplier (fourth Limbo button sets index 3)
        if (GlobalGameState.Instance != null)
        {
            float mult = GlobalGameState.Instance.GetMultiplier(3); // 1f normally, 1.5f if upgrade bought
            if (mult > 1f)
            {
                maxTime *= mult;
                currentTime *= mult; // keep it proportional to start full
                Debug.Log($"[TimerManager] Max time boosted by {mult}× -> {maxTime:F1}s");
            }
        }


        if (currentTime <= 0f) currentTime = maxTime;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (isGameOver) return;

        // drain time
        currentTime -= timeDrainRate * Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);
        UpdateTimerUI();

        if (currentTime <= 0f && !isGameOver)
        {
            isGameOver = true;
            Debug.Log("[TimerManager] GAME OVER – timer hit 0.0");

            //ToDo ing: trigger real GameOver logic here
            
            // * Black fade to the "Main Menu" or later "Limbo" scene:
            Initiate.Fade("LimboScene", Color.black, 5f);

        }
    }

    private void UpdateTimerUI()
    {
        if (timerFillImage != null && maxTime > 0f)
            timerFillImage.fillAmount = Mathf.Clamp01(currentTime / maxTime);

        if (timerText != null)
            timerText.text = Mathf.Max(0f, currentTime).ToString("F1");
    }

    /// <summary>
    /// Add time (clamped to max). Call from anywhere: TimerManager.Instance.AddTime(5f);
    /// </summary>
    public void AddTime(float seconds)
    {
        if (isGameOver) return;
        float before = currentTime;
        currentTime = Mathf.Clamp(currentTime + seconds, 0f, maxTime);
        UpdateTimerUI();
        Debug.Log($"[TimerManager] AddTime({seconds}) – before {before:F1}s, now {currentTime:F1}s (max {maxTime})");
    }
}
