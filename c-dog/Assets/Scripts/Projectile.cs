using UnityEngine;
using System.Collections;

public class Projectile : PoolObject {

	public LayerMask collisionMask;
	public Color trailColour;
	float speed = 10;
	float damage = 1;

	float lifetime = 3;
	float skinWidth = .1f;

    float spawnTime;

    void Awake()
    {
        GetComponent<TrailRenderer> ().material.SetColor ("_TintColor", trailColour);
    }

	void Start()
    {
        spawnTime = Time.time;

		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);
		if (initialCollisions.Length > 0)
        {
			OnHitObject(initialCollisions[0], transform.position);
		}
	}

    public override void OnObjectReuse()
    {
        Start();
    }

	public void SetSpeed(float newSpeed)
    {
		speed = newSpeed;
	}
	
	void Update ()
    {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);

        if (spawnTime + lifetime < Time.time)
        {
            Destroy();
        }
	}

	void CheckCollisions(float moveDistance)
    {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint)
    {
		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null)
        {
			damageableObject.TakeHit(damage, hitPoint, transform.forward);
		}

        Destroy();
	}
}
