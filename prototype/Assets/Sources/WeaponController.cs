using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform firePosition;

	void Start ()
	{
		InvokeRepeating ("Fire", 1, 2);
	}

	void Fire()
	{
		Instantiate (shot, firePosition.position, firePosition.rotation);
	}
}
