using UnityEngine;

public class Enemy : Entity
{

    public EnemySO enemyData;
    public float currentHealth;
    public float attackDistance;
    public GameObject player;
    public bool readyToAttack = true;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
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
            Attacking();
            return;
        }
        Walking();
        
    }
    
    void Walking(){
        // ! Walking Animation
        //enemy is walking to position has not collided with someting yet
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), enemyData.SPEED * Time.deltaTime);
    }

    void Attacking()
    {
        // ! Attacking Animation
        //enemy is attacking a tower or a unit
        //play attack animation
        readyToAttack = false;
        inAnimation = true;
        if (debugging) Debug.Log("Attacking");
        //int attackIndex = Random.Range(0, enemyData.attacks.Count - 1);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange);
        if(debugging) Debug.Log("Enemy Attacking Players in Range: " + hitPlayer.Length);
        foreach (Collider2D player in hitPlayer)
        {
            if(player.GetComponent<Player>() == null) continue;
            player.GetComponent<Player>().TakeDamage(enemyData.damage);
        }
        Invoke("resetAttack", enemyData.reachargeTime);//THE ANIMATION WILL HANDLE THIS
        Invoke("finishAnimation", enemyData.reachargeTime); //THE ANIMATION WILL HANDLE THIS
    }
    void resetAttack(){
        readyToAttack = true;
    }
    void finishAnimation(){
        inAnimation = false;
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.transform.position);
    }

    void Dying(){
        // ! Dying Animation
        //enemy has been killed
        if(debugging) Debug.Log("Dying");
        alive = false;
        //play death animation
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage){
        if(debugging) Debug.Log("Taking Damage");
        currentHealth -= damage;
    }   
}
