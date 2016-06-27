using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

    protected enum State {Idle, Chasing, Attacking};
    State currentState;

    WeaponController weaponController;

    public ParticleSystem deathEffect;
    public static event System.Action OnDeathStatic;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColour;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    AIController ai;

    bool hasTarget;

    void Awake()
    {
        weaponController = GetComponent<WeaponController>();
        pathfinder = GetComponent<NavMeshAgent>();
        ai = GetComponent<AIController>();
    }

    protected override void Start ()
    {
        base.Start ();

        ai.Activate();
    }

    public void SetTarget(GameObject targetObj)
    {
        if (target == null)
        {
            hasTarget = false;
        }
        else
        {
            hasTarget = true;

            target = targetObj.transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;
            weaponController.EquipGun(0);
            //StartCoroutine (UpdatePath ());
        }
    }

    public void Move()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
        if (!dead)
        {
            pathfinder.SetDestination (targetPosition);
        }
    }

    public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour)
    {
        pathfinder.speed = moveSpeed;

        if (hasTarget) 
        {
            damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
        }
        startingHealth = enemyHealth;

        deathEffect.startColor = new Color (skinColour.r, skinColour.g, skinColour.b, 1);
        skinMaterial = GetComponent<Renderer> ().material;
        skinMaterial.color = skinColour;
        originalColour = skinMaterial.color;
    }

    public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        AudioManager.instance.PlaySound ("Impact", transform.position);
        if (damage >= health && !dead)
        {
            if (OnDeathStatic != null)
            {
                OnDeathStatic ();
            }

            AudioManager.instance.PlaySound ("Enemy Death", transform.position);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
        base.TakeHit (damage, hitPoint, hitDirection);
    }

    void OnTargetDeath() 
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update ()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    AudioManager.instance.PlaySound ("Enemy Attack", transform.position);
                    //StartCoroutine (AttackMelee());
                }

                AttackRange();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("stop ai");
            ai.Deactivate();
        }
    }

    void AttackRange()
    {
        weaponController.OnTriggerHold();
    }

    IEnumerator AttackMelee()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathfinder.SetDestination (targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}