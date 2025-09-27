using UnityEngine;

public class Player : Entity
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float damage){
        if(debugging) Debug.Log("Player Taking Damage");
    }
}
