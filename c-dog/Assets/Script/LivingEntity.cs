using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead = false;

    public System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir)
    {
        TakeDamage (damage);
    }

    public virtual void TakeDamage(float damage)
    {
        print ("hit " + damage);
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die ();
        }
    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;

        if (OnDeath != null) 
        {
            OnDeath ();
        }

        GameObject.Destroy (gameObject);
    }
}
