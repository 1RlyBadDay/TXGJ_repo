using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("debugging ")]
    public bool debugging;

    [Header("Background Settings")]
    public bool scrolling = false;
    BackGroundManager backGroundManager;
    EntityManager entityManager;
    public bool StopScrolling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGroundManager = gameObject.GetComponent<BackGroundManager>();
        if (backGroundManager == null)
        {
            Debug.LogError("BackGroundManager not found in scene");
        }
        entityManager = gameObject.GetComponent<EntityManager>();
        if (entityManager == null)
        {
            Debug.LogError("EntityManager not found in scene");
        }

        entityManager.spawning = true; //start spawiong

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Debug.Log("Space pressed in GameManager");

        if (scrolling)
        {
            //scroll background
            backGroundManager.StartScrolling();
            scrolling = false;
        }
        if (StopScrolling)
        {
            backGroundManager.StopScrolling();
            StopScrolling = false;
        }
    }
    
    public void EndGame(){
        //end the game
        if (debugging) Debug.Log("Game Over");
        //stop background scrolling
        backGroundManager.StopScrolling();

        // * SEND TO UPGRADES PAGE:
        // Colored (maybe red for now, I'm thinking... can be changed here) fade over 2 seconds to the limbo scene
        Initiate.Fade("LimboScene", Color.red, 0.8f);
    }
}
