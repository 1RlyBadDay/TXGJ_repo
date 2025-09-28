using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("debugging ")]
    public bool debugging;

    [Header("Background Settings")]
    public bool scrolling = false;
    BackGroundManager backGroundManager;
    public bool StopScrolling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGroundManager = gameObject.GetComponent<BackGroundManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
