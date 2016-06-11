using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	Vector3 velocity;
	Rigidbody myRigidbody;
    public Camera cam;

	void Start ()
    {
		myRigidbody = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 _velocity)
    {
		velocity = _velocity;
	}

	public void LookAt(Vector3 lookPoint)
    {
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

	void FixedUpdate()
    {
		myRigidbody.MovePosition (myRigidbody.position + velocity * Time.fixedDeltaTime);
        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z - 3f);
	}
}
