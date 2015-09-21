using UnityEngine;
using System.Collections;

public class PlayerContoller : MonoBehaviour
{
	float speed = 5;
	float tilt = 5;
	public GameObject shot;
	public Transform firePosition;

	// Use this for initialization
	void Start ()
	{
	
	}

	IEnumerator OnMouseDown()
	{
		Vector3 scrSpace = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));
		
		while (Input.GetMouseButton(0))
		{
			Vector3 curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
				Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
			transform.position = curPosition;
			yield return null;
		}
	}

	void Update()
	{
		if (Input.touchCount > 0 || Input.GetKey(KeyCode.Mouse0))
		{
			//Instantiate(shot, firePosition.position, firePosition.rotation);
		}
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
