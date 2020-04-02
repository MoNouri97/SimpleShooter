
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class Menus : MonoBehaviour
{
	public GameObject pauseMenu;
	public GameObject optionMenu;
	public GameObject mainMenu;

	[Header("Options")]
	public Slider master;
	public Slider music;
	public Slider sfx;
	public Toggle fullScreen;
	public Toggle quality;
	public Toggle followCam;

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
	public void PlayTraining()
	{
		SceneManager.LoadScene("Training");
	}
	public void BackToMain()
	{
		SceneManager.LoadScene("MainMenu");
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
		Cursor.visible = false;
		pauseMenu.SetActive(false);
		Player.isPaused = false;
		Time.timeScale = 1;

	}
	public void PauseGame()
	{
		Cursor.visible = true;
		pauseMenu.SetActive(true);
		ShowMain();
		Time.timeScale = 0;
		Player.isPaused = true;
	}

	#region Options

	public void Init()
	{
		// getting user config and
		// initializing the UI
		master.value = AudioManager.instance.masterVolume;
		music.value = AudioManager.instance.musicVolume;
		sfx.value = AudioManager.instance.sfxVolume;

		fullScreen.isOn = PlayerPrefs.GetInt("fullscreen", 0) > 0;
		SetFullscreen(fullScreen.isOn);

		quality.isOn = PlayerPrefs.GetInt("quality", 1) > 0;
		SetQuality(quality.isOn);

		followCam.isOn = PlayerPrefs.GetInt("followCam", 1) > 0;
		SetCam(followCam.isOn);
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
		// if (val)
		// {
		// 	Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1]);
		// }
		Screen.fullScreen = val;

		PlayerPrefs.SetInt("fullscreen", (val) ? 1 : 0);
	}

	public void SetQuality(bool val)
	{
		PostP.instance.SetQuality(val);
		PlayerPrefs.SetInt("quality", (val) ? 1 : 0);

	}

	public void SetCam(bool val)
	{
		FollowPlayer followPlayer = FindObjectOfType<FollowPlayer>();
		followPlayer.enabled = val;
		PlayerPrefs.SetInt("followCam", (val) ? 1 : 0);
		if (!val)
		{
			followPlayer.transform.position = followPlayer.originalPos;
		}
	}

	// public void SetResolution(int i)
	// {
	// 	// print("res:" + resolutions[i].width + "x" + resolutions[i].height);
	// 	Screen.SetResolution(resolutions[i].width, resolutions[i].width, Screen.fullScreen);

	// }
	#endregion

}
