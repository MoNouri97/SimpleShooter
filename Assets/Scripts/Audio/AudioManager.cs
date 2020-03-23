using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public float masterVolume = 1;
	public float sfxVolume = 1;
	public float musicVolume = .1f;

	AudioSource[] audioSources;
	int activeSource;
	Transform playerT;
	Transform listener;
	void Awake()
	{
		playerT = FindObjectOfType<Player>().transform;
		listener = FindObjectOfType<AudioListener>().transform;
		instance = (instance == null) ? this : instance;
		audioSources = new AudioSource[2];
		for (int i = 0; i < 2; i++)
		{
			GameObject newSrc = new GameObject("musicSrc " + i);
			audioSources[i] = newSrc.AddComponent<AudioSource>();
			newSrc.transform.parent = transform;
		}
	}

	// Update is called once per frame
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
		PlaySound(clip, playerT.position);
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1)
	{
		activeSource = 1 - activeSource;
		audioSources[activeSource].clip = clip;
		audioSources[activeSource].Play();

		StartCoroutine(MusicFade(fadeDuration));

	}

	IEnumerator MusicFade(float time)
	{
		float percent = 0;
		float speed = 1 / time;
		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			audioSources[activeSource].volume = Mathf.Lerp(0, musicVolume * masterVolume, percent);
			audioSources[1 - activeSource].volume = Mathf.Lerp(musicVolume * masterVolume, 0, percent);
			yield return null;
		}

	}
}
