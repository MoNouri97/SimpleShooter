using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public AudioClip mainTheme;
	public AudioClip menuTheme;
	AudioClip clipToPlay;

	void Start()
	{
		AudioManager.instance.PlayMusic(menuTheme, 2);
		SceneManager.sceneLoaded += UpdateMusic;

	}

	void UpdateMusic(Scene scene, LoadSceneMode mode)
	{
		switch (scene.name)
		{
			case "Game":
				clipToPlay = mainTheme;
				break;
			case "Menu":
				clipToPlay = menuTheme;
				break;
		}
		Invoke("PlayMusic", .2f);
	}
	public void PlayMusic()
	{
		AudioManager.instance.PlayMusic(clipToPlay, 2);
		Invoke("PlayMusic", clipToPlay.length);
	}
}