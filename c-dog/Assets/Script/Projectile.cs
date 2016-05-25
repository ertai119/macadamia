﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 10;

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
}
