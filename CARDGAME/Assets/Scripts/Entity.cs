using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected bool damagable = true;
    protected bool alive = true;
    protected bool inAnimation = false;
    public float currentHealth;
    public bool readyToAttack = true;
    public float damageMultiplier = 1f; //for buffs and debuffs
    public float damageReduction = 1f; //for armor and shields
    
    public enum entietyType
    {
        player, enemy
    }
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
            if(obj.GetComponent<Entity>() == null) continue;
            Entity entity = obj.GetComponent<Entity>();
            if(entity.alive == false) continue;
            if(entity.damagable == false) continue;
            if(entity is Player && this is Enemy) obj.GetComponent<Player>().TakeDamage(damage * damageMultiplier);
            if(entity is Enemy && this is Player) obj.GetComponent<Enemy>().TakeDamage(damage * damageMultiplier);
            
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
        if (debugging) Debug.Log(gameObject.name + " Taking Damage");
        currentHealth -= damage * damageReduction;
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
