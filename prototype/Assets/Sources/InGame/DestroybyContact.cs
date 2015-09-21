using UnityEngine;
using System.Collections;

public class DestroybyContact : MonoBehaviour {

	public GameObject explosion;	
	public GameObject playerExplosion;
	private Vector2 touchpos;

	GameObject gameController;
	
	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.Find ("GameController");
	}

	void Update()
	{
		touchClick ();
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
						if (hit.collider.tag == "Enemy")
						{		
							Debug.Log("hit info : " + hit.collider);	

							Instantiate (explosion, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);

							gameController.GetComponent<GameController>().AddScore (10);

							Destroy (hit.collider.gameObject);
						}
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			return;
		}

		if (other.tag == "Player") 
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
		}

		if (explosion != null)
		{
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.GetComponent<GameController>().GameOver();
		}

		//gameController.GetComponent<GameController>().AddScore (10);

		Destroy (gameObject);
		//Destroy (other.gameObject);
	}
}
