using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected bool damagable = true;
    protected bool alive = true;
    protected bool inAnimation = false;
    public abstract void TakeDamage(float damage);

    [Header("Debugging")]
    public bool debugging;
}
