﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public Color trailColor;

    float speed = 10;
    float damege = 1;
    float lifeTime = 3;
    float adjustRayOffset = .1f;

    void Start()
    {
        Destroy (gameObject, lifeTime);

        Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0) {
            OnHitObject (initialCollisions [0], transform.position);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }
	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	
	void Update () 
	{
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions (moveDistance);
        transform.Translate (Vector3.forward * moveDistance);
	}

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit, moveDistance + adjustRayOffset, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject (hit.collider, hit.point);
        }
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable> ();
        if (damageableObject != null) {
            damageableObject.TakeHit (damege, hitPoint, transform.forward);
        }
        GameObject.Destroy (gameObject);
    }
}
