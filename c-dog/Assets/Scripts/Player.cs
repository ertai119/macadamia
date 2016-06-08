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
	
    public GameObject moveJoystick;
    public GameObject cameraJoystick;
    public GameObject shootButton;

	protected override void Start ()
    {
		base.Start ();

        Analytics.Transaction("12345abcde", 0.99m, "USD", null, null);

        Gender gender = Gender.Female;
        Analytics.SetUserGender(gender);

        int birthYear = 2014;
        Analytics.SetUserBirthYear(birthYear);
	}

	void Awake()
    {
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		
		FindObjectOfType<Spawner> ().OnNewWave += OnNewWave;

        if (Menu.instance.easyModeFlag)
        {
            cameraJoystick.SetActive(false);
            shootButton.SetActive(true);
        }
        else
        {
            cameraJoystick.SetActive(true);
            shootButton.SetActive(false);
        }
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

        if (Menu.instance.easyModeFlag == false)
        {
            // Look input
            Vector3 cameraInput = new Vector3(CnInputManager.GetAxis("CameraHorizontal"), 0f, CnInputManager.GetAxis("CameraVertical"));
            Vector3 camDir = cameraInput.normalized;
            if (camDir != Vector3.zero)
            {
                buttonDown = true;

                Vector3 lookPoint = cameraInput.normalized + controller.transform.position;
                controller.LookAt(lookPoint);
                //gunController.Aim(lookPoint);

                //crosshairs.transform.position = lookPoint;
            }
            else
            {
                buttonDown = false;
            }    
        }
        else
        {
            buttonDown = CnInputManager.GetButton("Shoot");
            if (moveInput.normalized != Vector3.zero)
            {
                Vector3 lookPoint = moveInput.normalized + controller.transform.position;
                controller.LookAt(lookPoint);
                //gunController.Aim(lookPoint);

                //crosshairs.transform.position = lookPoint;
            }
        }       

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
