using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]

public class Player : MonoBehaviour {

	public float moveSpeed = 5;

	Camera viewCamera;
	PlayerController playerController;
	GunController gunController;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();

		viewCamera = Camera.main;
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
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);

		float rayDistance;
		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin, point, Color.red);
			playerController.LookAt(point);
		}

		// Weapon Input
		if (Input.GetMouseButton(0))
		{
			gunController.Shoot ();	
		}
	}
}
