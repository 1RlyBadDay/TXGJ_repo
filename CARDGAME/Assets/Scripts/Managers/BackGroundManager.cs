using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class BackGroundManager : MonoBehaviour
{
    bool scrolling = false;
    public float scrollSpeed = 0.5f;
    public Transform background1StartPos;
    public Transform background1EndPos;
    public List<GameObject> backgrounds;
    public Player player;
    public void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in scene");
        }
    }
    public void StartScrolling()
    {
        scrolling = true;

    }

    public void StopScrolling()
    {
        scrolling = false;
        player.setWalking(false);
    }
    
    void scrollBackground()
    {
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.position = Vector2.MoveTowards(bg.transform.position, background1EndPos.position, scrollSpeed * Time.deltaTime);
            if (bg.transform.position == background1EndPos.position)
            {
                bg.transform.position = background1StartPos.position;
            }
        }
    }
    public void Update()
    {
        if (scrolling)
        {
            scrollBackground();
            player.setWalking(true);
       } 

    }
}
