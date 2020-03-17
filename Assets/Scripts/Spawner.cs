using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Enemy enemy;
	public Wave[] waves;

	LivingEntity playerEntity;
	Transform playerT;
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
	float campTime = 2;
	float NextCampCheckTime;
	Vector3 campPosOld;
	float campThreshold = 1.5f;
	bool isCamping;
	bool isDisabled;


	private void Start()
	{
		playerEntity = FindObjectOfType<Player>();
		playerT = playerEntity.transform;

		NextCampCheckTime = campTime + Time.time;
		campPosOld = playerT.position;

		map = FindObjectOfType<MapGenerator>();
		currentWave = -1;
		NextWave();
	}
	private void Update()
	{
		if (isDisabled) return;
		if (Time.time > NextCampCheckTime)
		{

			NextCampCheckTime = Time.time + campTime;
			isCamping = Vector3.Distance(campPosOld, playerT.position) < campThreshold;
			campPosOld = playerT.position;
		}
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
		//select tile
		Transform spawnTile = map.GetRandomOpenTile();
		if (isCamping)
		{
			spawnTile = map.PositonToTile(playerT.position);
		}
		//geting material
		Material mat = spawnTile.GetComponent<Renderer>().material;
		Color initColor = mat.color;
		Color flashColor = Color.red;
		float timer = 0;
		while (timer < spawnDelay)
		{
			mat.color = Color.Lerp(initColor, flashColor, Mathf.PingPong(timer * flashSpeed, 1));
			timer += Time.deltaTime;
			yield return null;
		}

		Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
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

	void OnPlayerDeath()
	{
		isDisabled = true;
	}
	void OnEnemyDeath()
	{
		if (--enemiesAlive == 0)
		{
			NextWave();
		}
	}
}