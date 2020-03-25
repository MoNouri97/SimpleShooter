
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
	public GameObject optionMenu;
	public GameObject mainMenu;
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


	#region Options

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
	#endregion

}
