using UnityEngine;
using System.Collections;

public class PlayerContoller : MonoBehaviour
{
	float speed = 5;
	float tilt = 5;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		float dirX = Input.GetAxis("Horizontal");
		float dirY = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 (dirX, 0, dirY);

		GetComponent<Rigidbody> ().velocity = movement * speed;
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody> ().velocity.x * -tilt);
	}
}
