using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;

    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;

    Spawner spawner;

    void Start ()
    {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;
    }

    void OnNewWave(int waveNumber)
    {
        newWaveTitle.text = "- Wave : #" + waveNumber + " -";

        string strEnemyCount = spawner.waves[waveNumber - 1].infinite == true
            ? "Infinite" : spawner.waves[waveNumber - 1].enemyCount + "";
        newWaveEnemyCount.text = " Enemies : " + strEnemyCount;

        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }

    void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
	}

    IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 1f;
        float speed = 3f;
        float animatePercent = 0;
        int dir = 1;

        float endDelayTime = Time.time + 1 / speed + delayTime;
            
        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-100, 250, animatePercent);
            yield return null;
        }
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        Cursor.visible = true;

        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }

    }

    // UI input
    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");
    }
}
