using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Enemy enemy;
	public Wave[] waves;
	int enemiesToSpawn;
	int enemiesAlive;
	float nextSpawnTime;
	int currentWave;


	[System.Serializable]
	public struct Wave
	{
		public int enemyCount;
		public float timeBetweenSpawns;
	}

	private void Start()
	{
		currentWave = -1;
		NextWave();
	}
	private void Update()
	{
		if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
		{
			enemiesToSpawn--;
			nextSpawnTime = Time.time + waves[currentWave].timeBetweenSpawns;

			Enemy spawnedEnemy = Instantiate(enemy, Vector3.one, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}

	void NextWave()
	{
		currentWave++;
		// no more waves
		if (currentWave >= waves.Length)
		{
			return;
		}
		enemiesToSpawn = waves[currentWave].enemyCount;
		enemiesAlive = enemiesToSpawn;

		print("Wave" + (currentWave + 1));
	}

	void OnEnemyDeath()
	{
		if (--enemiesAlive == 0)
		{
			NextWave();
		}
	}
}
