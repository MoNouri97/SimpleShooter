using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public AudioClip mainTheme;
	public AudioClip menuTheme;
	private void Start()
	{
		AudioManager.instance.PlayMusic(menuTheme, 2);

	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			AudioManager.instance.PlayMusic(mainTheme, 3);
		}
	}
}