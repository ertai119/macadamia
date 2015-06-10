using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour 
{

	float tumble = 5;

	void Start()
	{
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble;
	}
}
