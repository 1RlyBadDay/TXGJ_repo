using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    protected bool damagable = true;
    protected bool alive = true;
    protected bool inAnimation = false;
    public float currentHealth;
    public bool readyToAttack = true;
    public float damageMultiplier = 1f; //for buffs and debuffs
    public float damageReduction = 1f; //for armor and shields
    public float eMaxHealth;
    public Slider healthBar;
    public Animator eAnimator;

    
    public virtual void Attacking(float damage, float reachargeTime, float attackRange, AnimationClip attackAnimation)
    {
        Debug.Log("entity entered attack");
        // ! Attacking Animation
        // if(damage <= 0) {
        //     Debug.LogError("Damage must be greater than 0");
        //     return;
        // }
        // if(attackRange <= 0) {
        //     Debug.LogError("Attack range must be greater than 0");
        //     return;
        // }
        //player is attacking a tower or a unit
        //play attack animation
        readyToAttack = false;
       // Debug.Log(gameObject.name + "Attacking with range: " + attackRange);
        //int attackIndex = Random.Range(0, enemyData.attacks.Count - 1);
      
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        Debug.Log(gameObject.name + " Attacking entites in Range: " + hits.Length);
        foreach (Collider2D obj in hits)
        {
            Entity entity = obj.GetComponent<Entity>();
            if (entity == null || !entity.alive || !entity.damagable) continue;

            // Use pattern matching to safely check and cast at the same time
            if (this is Enemy && entity is Player playerHit)
            {
                playerHit.TakeDamage(damage * damageMultiplier);
                if (debugging) Debug.Log($"{this.name} hit Player for {damage * damageMultiplier} damage");
            }
            else if (this is Player && entity is Enemy enemyHit)
            {
                enemyHit.TakeDamage(damage * damageMultiplier);
                if (debugging) Debug.Log($"{this.name} hit Enemy for {damage * damageMultiplier} damage");
            }
            else
            {
                if (debugging) Debug.Log($"{this.name} hit {obj.name} but no valid target type found");
            }

        }
        Invoke("resetAttack", reachargeTime);//THE ANIMATION WILL HANDLE THIS
    }
    void resetAttack(){
        readyToAttack = true;
    }
    void finishAnimation(){
        inAnimation = false;
    }
    public virtual void TakeDamage(float damage)
    {

        if (debugging && this is Enemy) Debug.Log("Enemy Health: " + currentHealth);
        if (debugging && this is Player) Debug.Log("Player Health: " + currentHealth);
        if (debugging) Debug.Log(gameObject.name + " Taking Damage");
        currentHealth -= damage * damageReduction;
        healthBar.value = currentHealth / eMaxHealth;

        if (currentHealth <= 0 && alive)
        {
            
            eAnimator.SetBool("Dead", true);
            eAnimator.SetTrigger("Hurt");
            Dying();
            return;
        }
        eAnimator.SetTrigger("Hurt");
        inAnimation = true;
        Invoke("finishAnimation", eAnimator.GetCurrentAnimatorStateInfo(0).length);
    } 
    public virtual void buffDamage(float multiplier, float duration){
    
        damageMultiplier *= multiplier;
        Invoke("resetDamageBuff", duration);
    }
    void resetDamageBuff(){
        damageMultiplier = 1f;
    }
    public abstract void Dying();
    

    [Header("Debugging")]
    public bool debugging;
}
