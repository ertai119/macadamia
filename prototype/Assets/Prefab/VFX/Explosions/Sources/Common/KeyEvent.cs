using UnityEngine;
using System.Collections;

public class KeyEvent : MonoBehaviour {

	public int keyValue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PressKey(int key)
	{	
		Debug.Log (" key : " + key);

		switch (key)
		{
		case 0: //exit
			Application.Quit();
			break;
		case 1: //start
			Application.LoadLevel(1);
			break;
		case 2: //restart
			Application.LoadLevel(1);
			break;
		}
	}
}
