using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;

	public Slider[] volumeSliders;
    public Toggle easyMode;

    public static Menu instance;

    public bool easyModeFlag { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy (gameObject);
        } 

        instance = this;
        //DontDestroyOnLoad (gameObject);

        easyModeFlag = PlayerPrefs.GetInt("easy mode", 1) > 0;
    }

	void Start()
    {
        if (volumeSliders[0] != null)
        {
            volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        }

        if (volumeSliders[1] != null)
        {
            volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        }

        if (volumeSliders[2] != null)
        {
            volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;
        }
		
        if (easyMode != null)
        {
            easyMode.isOn = easyModeFlag;
        }
	}

	public void Play()
    {
		SceneManager.LoadScene ("Game");
	}

	public void Quit()
    {
		Application.Quit ();
	}

	public void OptionsMenu()
    {
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void MainMenu()
    {
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}
	
    public void SetEasyMode()
    {
        easyModeFlag = easyMode.isOn;
        PlayerPrefs.SetInt("easy mode", easyModeFlag ? 1 : 0);
        PlayerPrefs.Save();
    }

	public void SetMasterVolume(float value)
    {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value)
    {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSfxVolume(float value)
    {
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);
	}

}
