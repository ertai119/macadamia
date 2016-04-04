using UnityEngine;
using System.Collections;

public class PlayerContoller : MonoBehaviour
{
	float speed = 5;
	float tilt = 5;
	public GameObject shot;
	public Transform firePosition;

	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("start");
	}

	IEnumerator OnMouseDown()
	{
		Vector3 scrSpace = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

		Debug.Log ("hit IEnumerator");

		while (Input.GetMouseButton(0))
		{
			Vector3 curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
			transform.position = curPosition;

			yield return null;
		}
	}

	IEnumerator HitMouseDown()
	{
		Vector3 scrSpace = Camera.main.WorldToScreenPoint (GetComponent<Rigidbody>().position);
		Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

		Vector3 dir = Input.mousePosition - scrSpace;
		dir.Normalize ();

		Transform localTm = GetComponent<Rigidbody> ().transform;
		localTm.Rotate (dir);

		Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 convertedPos;
		convertedPos.x = clickedPos.y;
		convertedPos.y = 0;
		convertedPos.z = clickedPos.x;


		GetComponent<Rigidbody> ().transform.LookAt(convertedPos);

		Debug.Log ("local pos : " + scrSpace + " offset : " + offset + " mouse pos : " + Input.mousePosition);

		return null;
	}

	public void ShotMissle()
	{		
		Instantiate(shot, firePosition.position, firePosition.rotation);
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{

			Debug.Log ("clicked");

			//Instantiate(shot, firePosition.position, firePosition.rotation);
			HitMouseDown();

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				if(hit.transform.gameObject == gameObject)
				{

				}
			}
		}
	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		//HitMouseDown();
		/*
		float dirX = Input.GetAxis("Horizontal");
		float dirY = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 (dirX, 0, dirY);

		GetComponent<Rigidbody> ().velocity = movement * speed;
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody> ().velocity.x * -tilt);
		*/
	}

	void touchClick()
	{
		if (Input.GetMouseButtonDown(0))
		{ 
			// 오브젝트 정보를 담을 변수 생성
			RaycastHit hit;

			// 터치 좌표를 담는 변수
			Ray touchray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// 터치한 곳에 ray를 보냄
			Physics.Raycast (touchray, out hit);

			// ray가 오브젝트에 부딪힐 경우
			if (hit.collider != null) 
			{
				if (Input.touchCount >= 1 || Input.GetKey(KeyCode.Mouse0))
				{
					if (hit.collider.tag != null)
					{
					}
				}
			}
		}
	}
}

