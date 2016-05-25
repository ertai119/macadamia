using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]

public class PlayerController : MonoBehaviour {

	Vector3 velocity;
	Rigidbody myRigidBody;

	// Use this for initialization
	void Start () 
	{
		myRigidBody = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 vel)
	{
		velocity = vel;
	}

	public void LookAt(Vector3 lookPoint)
	{
		Vector3 heightCorrentedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrentedPoint);
	}

	public void FixedUpdate()
	{
		myRigidBody.MovePosition (myRigidBody.position + velocity * Time.fixedDeltaTime);
	}
}
