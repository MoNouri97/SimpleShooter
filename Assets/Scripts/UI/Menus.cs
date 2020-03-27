﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class Menus : MonoBehaviour
{
	public GameObject optionMenu;
	public GameObject mainMenu;

	[Header("Options")]
	public Slider master;
	public Slider music;
	public Slider sfx;
	public Toggle fullScreen;
	public Toggle quality;

	// public Dropdown res;
	// List<OptionData> resList = new List<OptionData>();
	// Resolution[] resolutions;


	private void Start()
	{
		Init();
		// resolutions = Screen.resolutions;
		// foreach (Resolution r in resolutions)
		// {
		// 	OptionData data = new OptionData(r.width + "x" + r.height);
		// 	resList.Add(data);
		// }
		// res.AddOptions(resList);
	}
	public void Play()
	{
		SceneManager.LoadScene("Game");
	}


	public void ShowOptions()
	{
		mainMenu.SetActive(false);
		optionMenu.SetActive(true);
	}

	public void ShowMain()
	{
		optionMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void Resume()
	{
		gameObject.SetActive(false);
		optionMenu.SetActive(false);
		mainMenu.SetActive(false);
		Player.isPaused = false;
		Time.timeScale = 1;

	}
	public void PauseGame()
	{
		gameObject.SetActive(true);
		mainMenu.SetActive(true);
		Time.timeScale = 0;
		Player.isPaused = true;
	}

	#region Options

	public void Init()
	{
		master.value = AudioManager.instance.masterVolume;
		music.value = AudioManager.instance.musicVolume;
		sfx.value = AudioManager.instance.sfxVolume;
		fullScreen.isOn = PlayerPrefs.GetInt("fullscreen", 0) > 0;
		quality.isOn = PlayerPrefs.GetInt("quality", 0) > 0;
	}
	public void SetVolume(float value, string channel)
	{
		switch (channel)
		{
			case "master":
				AudioManager.instance.SetMasterVolume(value); break;
			case "sfx":
				AudioManager.instance.SetSfxVolume(value); break;
			case "music":
				AudioManager.instance.SetMusicVolume(value); break;
			default: break;
		}
	}
	public void SetMasterVolume(float value) => SetVolume(value, "master");
	public void SetMusicVolume(float value) => SetVolume(value, "music");
	public void SetSfxVolume(float value) => SetVolume(value, "sfx");

	public void SetFullscreen(bool val)
	{
		// print("fullscreen:" + val + Screen.resolutions[Screen.resolutions.Length - 1]);
		Screen.fullScreen = val;
		PlayerPrefs.SetInt("fullscreen", (val) ? 1 : 0);
	}

	public void SetQuality(bool val)
	{
		PostP.instance.SetQuality(val);
		PlayerPrefs.SetInt("quality", (val) ? 1 : 0);

	}
	// public void SetResolution(int i)
	// {
	// 	// print("res:" + resolutions[i].width + "x" + resolutions[i].height);
	// 	Screen.SetResolution(resolutions[i].width, resolutions[i].width, Screen.fullScreen);

	// }
	#endregion

}