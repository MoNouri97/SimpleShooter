using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
	#region Refs
	public float fadeTime;

	[Header("Game Over")]
	public Image fadeBG;
	public GameObject gameOverUI;
	[Header("banner")]
	public RectTransform banner;
	public Text waveTitle;
	public Text waveCount;



	[Header("Refs")]
	public Spawner spawner;
	public Player player;

	#endregion

	void Awake()
	{
		player.OnDeath += OnGameOver;
		spawner.OnNewWave += OnNewWave;
	}



	void OnNewWave(int wave)
	{
		StopCoroutine("AnimateBanner");
		string title = "- Wave " + (wave + 1) + " -";
		waveTitle.text = title;

		string count = (spawner.waves[wave].infinite) ? "Infinite" : (spawner.waves[wave].enemyCount) + "";
		waveCount.text = "Enemies: " + count;

		StartCoroutine("AnimateBanner");
	}
	void OnGameOver()
	{
		StartCoroutine(Fade(Color.clear, Color.black, fadeTime));
	}

	IEnumerator Fade(Color from, Color to, float time)
	{
		float percent = 0;
		float speed = 1 / time;
		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			fadeBG.color = Color.Lerp(from, to, percent);

			yield return null;
		}
		gameOverUI.SetActive(true);

	}

	IEnumerator AnimateBanner()
	{
		float delay = 1f;
		float direction = 1; //1:up , -1:down
		float percent = 0;
		float speed = 2.5f;
		do
		{

			percent += Time.deltaTime * speed * direction;
			banner.anchoredPosition = Vector2.up * Mathf.Lerp(-140, 0, percent);

			if (percent >= 1)
			{
				direction = -1;
				yield return new WaitForSeconds(delay);

			}
			yield return null;
		} while (percent > 0);

	}
	public void StartNewGame()
	{
		SceneManager.LoadScene("Game");

	}
}
