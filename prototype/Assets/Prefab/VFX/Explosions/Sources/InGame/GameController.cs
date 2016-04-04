using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public float startWait = 1;
	public float spawnWait = 0.75f;
	public float waveWait = 2;

	public GUIText scoreText;
	public GUIText gameoverText;

	GameObject ui;
	int score;
	bool gameover;

	void Start ()
	{
		Screen.SetResolution (480, 800, true);

		scoreText.text = "";
		gameoverText.text = "";

		score = 0;
		gameover = false;
		//googleAnalytics.LogEvent("Barren Fields", "Rescue", "Dragon", 1);

		ui = GameObject.Find("restart_btn"); 
		ui.SetActive (false);

		GameStart ();
	}

	public void GameStart()
	{		
		StartCoroutine (SpawnWaves ());
	}

	public void AddScore(int newScore)
	{
		score += newScore;
		UpdateScore ();
	}

	void UpdateScore()
	{
		scoreText.text = "Score : " + score;
	}

	public void GameOver()
	{
		gameoverText.text = "Game Over!!!";
		gameover = true;
	}

	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);

		while (true) 
		{
			for(int i = 0 ; i < 10 ; i++)
			{
				GameObject hazard = hazards [Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range(-6, 6), 5, 9);
				Quaternion spawnRotation = Quaternion.Euler (new Vector3 (0, 180, 0));
				Instantiate (hazard, spawnPosition, spawnRotation);
				
				yield return new WaitForSeconds(spawnWait);
			}

			yield return new WaitForSeconds(waveWait);

			if (gameover == true)
			{
				ui.SetActive(true);

				break;
			}
		}
	}

	void Update ()
	{
	}
	
}