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
	MapGenerator map;

	private void Start()
	{
		map = FindObjectOfType<MapGenerator>();
		currentWave = -1;
		NextWave();
	}
	private void Update()
	{
		if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
		{

			enemiesToSpawn--;
			nextSpawnTime = Time.time + waves[currentWave].timeBetweenSpawns;

			StartCoroutine(SpawnEnemy());

		}
	}

	IEnumerator SpawnEnemy()
	{
		float spawnDelay = 1;
		float flashSpeed = 4;
		Transform randomTile = map.GetRandomOpenTile();
		Material mat = randomTile.GetComponent<Renderer>().material;
		Color initColor = mat.color;
		Color flashColor = Color.red;
		float timer = 0;
		while (timer < spawnDelay)
		{
			Debug.Log(timer + " " + spawnDelay);

			mat.color = Color.Lerp(initColor, flashColor, Mathf.PingPong(timer * flashSpeed, 1));
			timer += Time.deltaTime;
			yield return null;
		}

		Enemy spawnedEnemy = Instantiate(enemy, randomTile.position + Vector3.up, Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;
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
