using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Score : MonoBehaviour
{
	#region Refs & vars
	public TextMeshProUGUI scoreCount;
	public TextMeshProUGUI streakCount;
	public float streakResetTime;
	Player player;

	#endregion
	int score;
	int streak;
	bool onStreak;
	float lastKillTime;
	void Start()
	{
		streak = 0;
		Enemy.OnDeathStatic += OnEnemyKilled;
		player = GetComponent<UI>().player;
		player.OnDeath += OnPlayerDeath;
	}
	private void Update()
	{
		if (lastKillTime + streakResetTime <= Time.time)
		{
			streakCount.gameObject.SetActive(false);
			onStreak = false;
		}

	}
	public void OnEnemyKilled()
	{
		if (onStreak)
		{
			streak++;
			streakCount.gameObject.SetActive(true);

		}
		else
		{
			onStreak = true;
			streak = 1;
		}
		lastKillTime = Time.time;

		streakCount.text = "x" + streak;
		score += 5 * streak;
		scoreCount.text = (score).ToString("D6");
	}

	public void OnPlayerDeath()
	{
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}

	public void StartNewGame()
	{
		SceneManager.LoadScene("Game");

		score = 0;
		streak = 0;
		scoreCount.text = (score).ToString("D6");

	}
}
