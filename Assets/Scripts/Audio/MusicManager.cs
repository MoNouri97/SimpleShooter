using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
	public AudioClip mainTheme;
	public AudioClip menuTheme;
	private void Start()
	{
		AudioManager.instance.PlayMusic(menuTheme, 2);
		SceneManager.sceneLoaded += UpdateMusic;

	}

	private void UpdateMusic(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game")
		{
			AudioManager.instance.PlayMusic(mainTheme, 2);
		}
	}
}