using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CnControls;
using UnityEngine.Analytics;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;

	public Crosshairs crosshairs;

    bool buttonDown = false;
	PlayerController controller;
	GunController gunController;
	
	protected override void Start ()
    {
		base.Start ();

        /*Analytics.Transaction("12345abcde", 0.99m, "USD", null, null);

        Gender gender = Gender.Female;
        Analytics.SetUserGender(gender);

        int birthYear = 2014;
        Analytics.SetUserBirthYear(birthYear);*/
	}

	void Awake()
    {
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		//viewCamera = Camera.main;
		FindObjectOfType<Spawner> ().OnNewWave += OnNewWave;
	}

	void OnNewWave(int waveNumber)
    {
		health = startingHealth;
		gunController.EquipGun (waveNumber - 1);

        Analytics.CustomEvent("on new wave", new Dictionary<string, object>
            {
                { "health", health },
                { "wave number", waveNumber }
            });        
	}

	void Update () {
		// Movement input
        Vector3 moveInput = new Vector3 (CnInputManager.GetAxisRaw ("Horizontal"), 0f, CnInputManager.GetAxisRaw ("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		// Look input
        Vector3 cameraInput = new Vector3 (CnInputManager.GetAxis ("CameraHorizontal"), 0f, CnInputManager.GetAxis("CameraVertical"));
        Vector3 camDir = cameraInput.normalized;
        if (camDir != Vector3.zero)
        {
            buttonDown = true;

            Vector3 lookPoint = cameraInput.normalized + controller.transform.position;
            controller.LookAt(lookPoint);
            //gunController.Aim(lookPoint);

            crosshairs.transform.position = lookPoint;
        }
        else
        {
            buttonDown = false;
        }

        print(buttonDown);

		// Weapon input
        if (buttonDown == true)
        {
			gunController.OnTriggerHold();
		}

        if (buttonDown == false)
        {
			gunController.OnTriggerRelease();
		}

		if (Input.GetKeyDown (KeyCode.R))
        {
			gunController.Reload();
		}

		if (transform.position.y < -10) {
			TakeDamage (health);
		}
	}

	public override void Die ()
	{
		AudioManager.instance.PlaySound ("Player Death", transform.position);
		base.Die ();
	}
		
}
