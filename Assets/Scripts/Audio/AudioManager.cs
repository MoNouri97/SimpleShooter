using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public float masterVolume { get; private set; }
	public float sfxVolume { get; private set; }
	public float musicVolume { get; private set; }

	AudioSource[] musicSources;
	AudioSource sfxSource;
	int activeSource = 0;
	Transform playerT;
	Transform listener;
	SoundLibrary library;

	#region Volume Setters

	public void SetMasterVolume(float value)
	{
		PlayerPrefs.SetFloat("masterVolume", value);
		masterVolume = value;
	}

	public void SetSfxVolume(float value)
	{
		PlayerPrefs.SetFloat("sfxVolume", value);
		sfxVolume = value;
	}

	public void SetMusicVolume(float value)
	{
		PlayerPrefs.SetFloat("musicVolume", value);
		musicVolume = value;
	}
	#endregion

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}


		instance = this;
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnNewScene;

		library = GetComponent<SoundLibrary>();
		Player player = FindObjectOfType<Player>();
		if (player != null)
		{
			playerT = player.transform;
		}
		listener = FindObjectOfType<AudioListener>().transform;
		musicSources = new AudioSource[2];
		for (int i = 0; i < 2; i++)
		{
			musicSources[i] = new GameObject("musicSrc " + i).AddComponent<AudioSource>();
			musicSources[i].gameObject.transform.parent = transform;
		}

		//sfx
		sfxSource = new GameObject("SFX Source").AddComponent<AudioSource>();
		sfxSource.gameObject.transform.parent = transform;

		//get old prefs
		masterVolume = PlayerPrefs.GetFloat("masterVolume", 1);
		musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
		sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);

	}


	void OnNewScene(Scene scene, LoadSceneMode mode)
	{
		if (playerT != null || FindObjectOfType<Player>() == null)
		{
			return;
		}

		playerT = FindObjectOfType<Player>().transform;
	}

	void Update()
	{
		if (playerT != null)
			listener.position = playerT.position;
	}

	public void PlaySound(AudioClip clip, Vector3 pos)
	{
		if (clip == null)
			return;
		AudioSource.PlayClipAtPoint(clip, pos, masterVolume * sfxVolume);
	}

	public void PlaySound(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
	}

	public void PlaySound(string clip)
	{
		PlaySound(library.GetClipFromName(clip));
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1)
	{
		activeSource = 1 - activeSource;
		musicSources[activeSource].clip = clip;
		musicSources[activeSource].Play();

		StartCoroutine(MusicFade(fadeDuration));

	}

	IEnumerator MusicFade(float time)
	{
		float percent = 0;
		float speed = 1 / time;
		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			musicSources[activeSource].volume = Mathf.Lerp(0, musicVolume * masterVolume, percent);
			musicSources[1 - activeSource].volume = Mathf.Lerp(musicVolume * masterVolume, 0, percent);
			yield return null;
		}

	}
}
