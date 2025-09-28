using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Entity
{
    public GameManager gameManager;
    public PlayerSO playerData;
    Animator pAnimator;
    public List<String> attacks = new List<String>();

    [Header("Player Debug")]
    public bool attack = false;
    public bool healing = false;
    public bool buffing = false;

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

        pAnimator = GetComponent<Animator>();
        if (pAnimator == null)
        {
            Debug.LogError(gameObject.name + " Animator component not found");
        }
        eAnimator = pAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack && readyToAttack && !inAnimation && alive)
        {
            Attacking(1f, 1f, 10f, null); //example values
            attack = false;
        }
        if (healing && alive)
        {
            Heal(10f); //example value
            healing = false;
        }
        if (buffing && alive)
        {
            buffDamage(2f, 5f); //example value
            buffing = false;
        }
       
    }
    public override void Attacking(float damage, float reachargeTime, float attackRange, AnimationClip attackAnimation)
    {
        // ! Attacking Animation
        if (attacks.Count > 0)
        {
            int attackIndex = UnityEngine.Random.Range(0, attacks.Count);
            pAnimator.SetTrigger(attacks[attackIndex]);
        }
        else
        {
            Debug.LogWarning("No attack animations assigned to player.");
        }
        inAnimation = true;
        Invoke("finishAnimation", pAnimator.GetCurrentAnimatorStateInfo(0).length);
        base.Attacking(damage, reachargeTime, attackRange, attackAnimation);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    public void Heal(float amount)
    {
        if (debugging) Debug.Log("Player Healed by " + amount);
        currentHealth += amount;
        if (currentHealth > playerData.MAX_HEALTH) currentHealth = playerData.MAX_HEALTH;
        healthBar.value = currentHealth / playerData.MAX_HEALTH;
        pAnimator.SetTrigger("Heal");
        inAnimation = true;
        Invoke("finishAnimation", pAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public override void Dying()
    {
        if (debugging) Debug.Log("Player Died");
        alive = false;
        inAnimation = true;

        //play death animation
        //after animation ends, call EndGame()
        gameManager.EndGame();
    }
    public override void buffDamage(float multiplier, float duration)
    {
        base.buffDamage(multiplier, duration);
        pAnimator.SetTrigger("Buff");
        inAnimation = true;
        Invoke("finishAnimation", pAnimator.GetCurrentAnimatorStateInfo(0).length);
    }
    public void setWalking(bool walking)
    {
        pAnimator.SetBool("Walking", walking);
    }  

}
