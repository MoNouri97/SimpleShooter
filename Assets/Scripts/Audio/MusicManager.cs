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
		SceneManager.sceneLoaded += UpdateMusic;
		UpdateMusic(SceneManager.GetActiveScene());
	}

	void UpdateMusic(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
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