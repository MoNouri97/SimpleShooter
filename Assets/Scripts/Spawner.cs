using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public bool devMode;
	public bool isDisabled;

	public Enemy enemy;
	public Wave[] waves;
	public PickUp[] pickUps;
	int pickUpsSpawned;

	LivingEntity playerEntity;
	Transform playerT;
	int enemiesToSpawn;
	int enemiesAlive;
	float nextSpawnTime;
	int currentWave;

	//this is a comment

	public event System.Action<int> OnNewWave;
	MapGenerator map;
	float campTime = 2;
	float NextCampCheckTime;
	Vector3 campPosOld;
	float campThreshold = 1.5f;
	bool isCamping;
	//Singleton
	public static Spawner instance { get; private set; }

	private void Awake()
	{
		if (instance == null) instance = this;
	}
	private void Start()
	{
		playerEntity = FindObjectOfType<Player>();
		playerT = playerEntity.transform;
		playerEntity.OnDeath += OnPlayerDeath;

		NextCampCheckTime = campTime + Time.time;
		campPosOld = playerT.position;

		map = FindObjectOfType<MapGenerator>();
		currentWave = -1;
		NextWave();
	}
	private void Update()
	{

		#region dev mode : skipping levels
		if (devMode && Input.GetButtonDown("Fire2"))
		{
			if (currentWave + 1 == waves.Length) return;
			StopCoroutine("SpawnEnemy");
			foreach (Enemy e in FindObjectsOfType<Enemy>())
			{
				GameObject.Destroy(e.gameObject);
			}
			NextWave();
		}
		#endregion


		if (isDisabled) return;
		if (Time.time > NextCampCheckTime)
		{

			NextCampCheckTime = Time.time + campTime;
			isCamping = Vector3.Distance(campPosOld, playerT.position) < campThreshold;
			campPosOld = playerT.position;
		}
		if ((enemiesToSpawn <= 0 && !waves[currentWave].infinite) || Time.time <= nextSpawnTime)
		{
			return;
		}

		enemiesToSpawn--;
		nextSpawnTime = Time.time + waves[currentWave].timeBetweenSpawns;

		StartCoroutine("SpawnEnemy");

	}

	void ResetPlayerPosition()
	{
		playerT.position = map.PositonToTile(Vector3.zero).position + Vector3.up * 5;
	}
	IEnumerator SpawnEnemy()
	{


		Wave wave = waves[currentWave];
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
		if (!spawnTile)
		{
			Debug.Log("error", spawnTile);
			yield break;
		}
		mat.color = initColor;
		Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
		spawnedEnemy.setProperties(wave.moveSpeed, wave.hitsToKillPlayer, wave.health, wave.color);
		spawnedEnemy.OnDeath += OnEnemyDeath;
	}
	void NextWave()
	{
		//clearing pickups
		foreach (PickUp e in FindObjectsOfType<PickUp>())
		{
			GameObject.Destroy(e.gameObject);
		}
		pickUpsSpawned = 0;

		AudioManager.instance.PlaySound(clip: "Level Completed");
		currentWave++;
		// no more waves
		if (currentWave >= waves.Length)
		{
			return;
		}

		enemiesToSpawn = waves[currentWave].enemyCount;
		enemiesAlive = enemiesToSpawn;
		if (OnNewWave != null)
		{
			OnNewWave(currentWave);
			ResetPlayerPosition();
		}
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
			return;
		}
		else
		{
			// chance to spawn a pickup
			bool rand = Random.Range(0f, 1f) < 0.2f;
			if (rand)
			{
				StartCoroutine(SpawnPickup(1, 10));
			}
		}
	}

	IEnumerator SpawnPickup(float spawnDelay = 1, float flashSpeed = 4)
	{
		Wave wave = waves[currentWave];
		int random = Random.Range(0, pickUps.Length);
		PickUp pickup = pickUps[random];

		//select tile
		Transform spawnTile = map.GetRandomOpenTile();

		//geting material
		Material mat = spawnTile.GetComponent<Renderer>().material;
		Color initColor = mat.color;
		Color flashColor = Color.green;
		float timer = 0;
		while (timer < spawnDelay)
		{
			mat.color = Color.Lerp(initColor, flashColor, Mathf.PingPong(timer * flashSpeed, 1));
			timer += Time.deltaTime;
			yield return null;
		}
		if (!spawnTile)
		{
			Debug.Log("error", spawnTile);
			yield break;
		}
		mat.color = initColor;
		PickUp spawnedEnemy = Instantiate(pickup, spawnTile.position + Vector3.up, Quaternion.identity) as PickUp;

	}


	[System.Serializable]
	public struct Wave
	{
		public bool infinite;
		public int enemyCount;
		public float timeBetweenSpawns;
		public float moveSpeed;
		public int hitsToKillPlayer;
		public float health;
		public Color color;
	}

}