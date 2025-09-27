using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool scrolling = false;
    BackGroundManager backGroundManager;
    [Header("Background Settings")]
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
        if(StopScrolling)
        {
            backGroundManager.StopScrolling();
            StopScrolling = false;
        }
    }
}
