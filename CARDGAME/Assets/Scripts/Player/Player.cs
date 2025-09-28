using UnityEngine;

public class Player : Entity
{
    public GameManager gameManager;
    public PlayerSO playerData;

    [Header("Player Debug")]
    public bool attack = false;
    void Awake()
    {
        eMaxHealth = playerData.MAX_HEALTH;
        if (gameManager == null)
        {
            gameManager = GameObject.FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in scene");
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerData == null)
        {
            Debug.LogError(gameObject.name + " Player Data not assigned in inspector");
        }
        currentHealth = playerData.STARTING_HEALTH;
        if (currentHealth <= 0)
        {
            Debug.LogError("Player Starting Health is less than or equal to 0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(attack && readyToAttack && !inAnimation && alive){
            Attacking(1f, 1f, 10f, null); //example values
            attack = false;
        }
    }

    //Set this to take in all the information about the attack
    /* Attack attack
     * {
     *      float damage;
     *      float reachargeTime;
     *      float attackRange;
     *      AnimationClip attackAnimation;
     * }
     */
   
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    public void Heal(float amount)
    {
        if (debugging) Debug.Log("Player Healed by " + amount);
        currentHealth += amount;
        if (currentHealth > playerData.MAX_HEALTH) currentHealth = playerData.MAX_HEALTH;
        healthBar.value = currentHealth / playerData.MAX_HEALTH;
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0 && alive)
        {
            Dying();
            return;
        }
    }
    public override void Dying()
    {
        if (debugging) Debug.Log("Player Died");
        alive = false;
        inAnimation = true;
        // ! Death Animation
        //play death animation
        //after animation ends, call EndGame()
        gameManager.EndGame();
    }
}
