using System.Linq;
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
    
    
    public void Attacking(float damage, float reachargeTime, float attackRange, AnimationClip attackAnimation)
    {
        // ! Attacking Animation
        //player is attacking a tower or a unit
        //play attack animation
        readyToAttack = false;
        inAnimation = true;
        if (debugging) Debug.Log(gameObject.name + "Attacking");
        //int attackIndex = Random.Range(0, enemyData.attacks.Count - 1);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if (debugging) Debug.Log(gameObject.name + " Attacking entites in Range: " + hits.Length);
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
        Invoke("finishAnimation", reachargeTime); //THE ANIMATION WILL HANDLE THIS
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
    } 
    public void buffDamage(float multiplier, float duration){
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
