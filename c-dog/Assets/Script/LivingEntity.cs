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

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage (damage);
    }

    public void TakeDamage(float damage)
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
