using UnityEngine;

using UnityEngine;

public class Unit : MonoBehaviour {
    
    public float speed = 1f;

    // Start
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }


}
