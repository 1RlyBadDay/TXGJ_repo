using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public EnemySO enemyData;
    public float attackDistance;
    public GameObject player;
    private GameManager gameManager;
    private EntityManager entityManager;
    public bool walking = true;
    // private GameObject timeManager;
    private TimerManager timeManagerScript;
    public List<string> attacks = new List<string>();

    void Awake()
    {
        eMaxHealth = enemyData.MAX_HEALTH;
        currentHealth = enemyData.MAX_HEALTH;
        if (currentHealth <= 0)
        {
            Debug.LogError("Enemy Starting Health is less than or equal to 0");
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        entityManager = GameObject.Find("GameManager").GetComponent<EntityManager>();
        if (entityManager == null)
        {
            Debug.LogError("EntityManager not found in scene");
        }

        timeManagerScript = GameObject.Find("GlobalTimerUI").GetComponent<TimerManager>();

        if (timeManagerScript == null)
        {
            Debug.LogError("TimeManager not found in scene");
        }

        eAnimator = GetComponent<Animator>();
        if (eAnimator == null)
        {
            Debug.LogError(gameObject.name + " Animator component not found");
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
        if (attackDistance == 0)
        {
            attackDistance = enemyData.attackRange - 0.25f; //slightly less than attack range so it can reach the player
        }
        walking = true;
        gameManager.scrolling = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (inAnimation || !alive) return;
        if (currentHealth <= 0 && alive)
        {
            Dying();
            return;
        }
//        if (debugging) Debug.Log("Distance to player: " + Vector2.Distance(transform.position, player.transform.position));
        if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance && readyToAttack)
        {
            if (walking)
            {
                walking = false;
                gameManager.StopScrolling = true;
                eAnimator.SetBool("Walk", false);
            }
            Attacking(enemyData.damage, enemyData.reachargeTime, enemyData.attackRange, enemyData.attackAnimation);
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            return; 
        }
        walking = true;
        Walking();

    }
    public override void Attacking(float damage, float reachargeTime, float attackRange, AnimationClip attackAnimation)
    {
        // ! Attacking Animation
        
        if (attacks.Count > 0)
        {
            int attackIndex = UnityEngine.Random.Range(0, attacks.Count);
            eAnimator.SetTrigger(attacks[attackIndex]);
        }
        else
        {
            Debug.LogWarning("No attack animations assigned to player.");
        }
        base.Attacking(damage, reachargeTime, attackRange, attackAnimation);
        Debug.Log(gameObject.name + " is Attacking");
        
        inAnimation = true;
        Invoke("finishAnimation", eAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    void Walking()
    {
        // ! Walking Animation
        eAnimator.SetBool("Walk", true);
        //enemy is walking to position has not collided with someting yet
        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(player.transform.position.x, currentPos.y, currentPos.z);
        Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, enemyData.SPEED * Time.deltaTime);
        transform.position = newPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
    }


    public override void Dying()
    {
        // ! Dying Animation" HANDLED IN HURT WHEN DYING
        //enemy has been killed
        Debug.Log("Enemy Died");
        alive = false;
        entityManager.RemoveEnemy(gameObject);

        //play death animation
        Debug.Log("invoked kill enemy)");  
        Invoke("KillEnemy", 0.25f);
        //? Increment GOLD, and later $ As well I think...
        /*
        GlobalGameState.Instance.Gold += 10;
        */
        // Enemy.cs  (inside Dying)
        float goldMult = GlobalGameState.Instance.GetMultiplier(4); // 1 = no upgrade
        int reward = Mathf.RoundToInt(10 * goldMult);
        GlobalGameState.Instance.Gold += reward;


        //? INCREMENT TIME ADD:
        if (timeManagerScript != null)
        {
            timeManagerScript.AddTime(10f);
        }
         
        
    }
    
    public void KillEnemy(){
        Destroy(gameObject);
    }

 
}
