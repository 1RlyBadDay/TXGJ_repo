using UnityEngine;

public class Enemy : Entity
{

    public EnemySO enemyData;
    public float attackDistance;
    public GameObject player;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy Data not assigned in inspector");
        }
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in scene");
        }
        currentHealth = enemyData.HEALTH;
        if (attackDistance == 0) {
            attackDistance = enemyData.attackRange - 0.25f; //slightly less than attack range so it can reach the player
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inAnimation || !alive) return;
        if (currentHealth <= 0 && alive)
        {
            Dying();
            return;
        }
        if(debugging) Debug.Log("Distance to player: " + Vector2.Distance(transform.position, player.transform.position));
        if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance && readyToAttack)
        {
            Attacking(enemyData.damage, enemyData.reachargeTime, enemyData.attackRange, enemyData.attackAnimation);
            return;
        }
        Walking();
        
    }
    
    void Walking(){
        // ! Walking Animation
        //enemy is walking to position has not collided with someting yet
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), enemyData.SPEED * Time.deltaTime);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.transform.position);
    }

    public override void Dying(){
        // ! Dying Animation
        //enemy has been killed
        if(debugging) Debug.Log("Dying");
        alive = false;
        //play death animation
        Destroy(gameObject);
    }

 
}
