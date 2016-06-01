using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public enum AudioChannel {Master, Sfx, Music};

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }

    AudioSource sfx2DSound;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    Transform audioListener;
    Transform playerT;
    SoundLibrary library;

    public static AudioManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < musicSources.Length; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            GameObject newSfx2DSound = new GameObject("2D sfx source");
            sfx2DSound = newSfx2DSound.AddComponent<AudioSource>();
            sfx2DSound.transform.parent = transform;

            audioListener = FindObjectOfType<AudioListener>().transform;
            if (FindObjectOfType<Player>() != null)
            {
                playerT = FindObjectOfType<Player>().transform;
            }

            masterVolumePercent =  PlayerPrefs.GetFloat("master vol", 1f);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", 1f);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1f);
        }
    }

    void OnLevelWasLoaded(int index) {
        if (playerT == null) {
            if (FindObjectOfType<Player> () != null) {
                playerT = FindObjectOfType<Player> ().transform;
            }
        }
    }

    void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        print("vol " + volumePercent + " " + channel);
        switch (channel)
        {
            case AudioChannel.Master:
                {
                    masterVolumePercent = volumePercent;
                }
                break;
            case AudioChannel.Music:
                {
                    musicVolumePercent = volumePercent;
                }
                break;
            case AudioChannel.Sfx:
                {
                    sfxVolumePercent = volumePercent;
                }
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, sfxVolumePercent * masterVolumePercent);
        }
    }

    public void PlaySound(string soundName, Vector3 position)
    {
        PlaySound(library.GetClipFromName(soundName), position);
    }

    public void PlaySound2D(string soundName, Vector3 position)
    {
        sfx2DSound.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    }

    IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
        
            yield return null;
        }
    }
}
