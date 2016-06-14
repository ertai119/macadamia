using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CnControls;
using UnityEngine.Analytics;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (WeaponController))]
public class Player : LivingEntity {

    public float moveSpeed = 5;

    public Crosshairs crosshairs;

    bool buttonDown = false;
    PlayerController controller;
    WeaponController weaponController;
    Enemy myTarget;

    public GameObject moveJoystick;
    public GameObject cameraJoystick;
    public GameObject shootButton;


    protected override void Start ()
    {
        base.Start ();
    }

    void Awake()
    {
        controller = GetComponent<PlayerController> ();
        weaponController = GetComponent<WeaponController> ();

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
        weaponController.EquipGun (waveNumber - 1);

        if (Menu.instance.easyModeFlag)
        {
            StartCoroutine(FindNearTarget());
        }
    }

    IEnumerator FindNearTarget()
    {
        float refreshRate = .03f;

        while (!dead)
        {
            Vector3 curPos = transform.position;
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            float minDist = 255f;
            int nextTargetIdx = -1;

            for (int i = 0; i < allEnemies.Length; i++)
            {
                Enemy spawnedEnemy = allEnemies[i];
                float dist = Vector3.Distance(curPos, spawnedEnemy.transform.position);
                if (dist < minDist && dist < 10f)
                {
                    minDist = dist;
                    nextTargetIdx = i;
                }
            }

            if (nextTargetIdx != -1)
            {
                SetTarget(allEnemies[nextTargetIdx]);
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }

    void SetTarget(Enemy target)
    {
        if (target == null)
            return;

        myTarget = target;
        UpdateTargetMarker();
    }

    void UpdateTargetMarker()
    {
        if (myTarget == null)
            return;

        controller.LookAt(myTarget.transform.position);
        weaponController.Aim(myTarget.transform.position);
        crosshairs.transform.position = myTarget.transform.position;
    }

    void Update ()
    {
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
            }
            else
            {
                buttonDown = false;
            }
        }
        else
        {
            UpdateTargetMarker();
            buttonDown = CnInputManager.GetButton("Shoot");
        }

        // Weapon input
        if (buttonDown == true)
        {
            weaponController.OnTriggerHold();
        }

        if (buttonDown == false)
        {
            weaponController.OnTriggerRelease();
        }

        if (Input.GetKeyDown (KeyCode.R))
        {
            weaponController.Reload();
        }

        if (transform.position.y < -10)
        {
            TakeDamage (health);
        }
    }

    public override void Die ()
    {
        AudioManager.instance.PlaySound ("Player Death", transform.position);
        base.Die ();
    }
}
