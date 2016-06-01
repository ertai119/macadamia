﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]

public class Player : LivingEntity {

	public float moveSpeed = 5;
    public Crosshairs crosshairs;

	Camera viewCamera;
	PlayerController playerController;
	GunController gunController;

	// Use this for initialization
    protected override void Start () 
	{
        base.Start ();
	}
	
    void Awake()
    {
        playerController = GetComponent<PlayerController> ();
        gunController = GetComponent<GunController> ();

        viewCamera = Camera.main;
        FindObjectOfType<Spawner>().OnNewWave += OnNextWave;
    }

    void OnNextWave(int waveNumber)
    {
        health = startingHealth;
        gunController.EquipGun();
    }

	// Update is called once per frame
	void Update () 
	{
		// Movement Input
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));	
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		playerController.Move (moveVelocity);

		// Lookat Input
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
        Plane groundPlane = new Plane (Vector3.up, Vector3.up * gunController.GunHeight);

		float rayDistance;
		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin, point, Color.red);
			playerController.LookAt(point);
            crosshairs.transform.position = point;
            crosshairs.DetectTargets(ray);

            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
            {
                gunController.Aim(point);    
            }
		}

		// Weapon Input
		if (Input.GetMouseButton(0))
		{
            gunController.OnTriggerHold ();
		}

        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease ();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gunController.Reload();
        }
	}

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }

}