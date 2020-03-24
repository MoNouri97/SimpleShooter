using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	private float masterVolume = 1;
	private float sfxVolume = 1;
	private float musicVolume = 0.1f;

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
		masterVolume = value;
	}

	public void SetMusicVolume(float value)
	{
		PlayerPrefs.SetFloat("musicVolume", value);
		masterVolume = value;
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
		playerT = FindObjectOfType<Player>().transform;
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
		masterVolume = PlayerPrefs.GetFloat("masterVolume", masterVolume);
		musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
		sfxVolume = PlayerPrefs.GetFloat("sfxVolume", sfxVolume);

	}


	void OnNewScene(Scene scene, LoadSceneMode mode)
	{
		if (playerT == null)
		{
			if (FindObjectOfType<Player>() != null)
			{
				playerT = FindObjectOfType<Player>().transform;
			}
		}
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
