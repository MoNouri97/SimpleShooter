using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	#region Params
	[Header("Prefabs")]
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Transform navMeshFloor;
	public Transform navMeshMask;
	public Transform floor; // the floor behind the tiles used for color and collision

	public int mapIndex;
	public Map[] maps;
	[Header("Variables")]
	public Vector2 maxMapSize;
	public float tileSize;
	[Range(0, 1)] public float outlinePercent;
	public bool accessibleMap = false;
	public Map trainingMap;

	#endregion
	Queue<Coord> shuffledCoords;
	Queue<Coord> shuffledOpenCoords;

	Transform[,] tileMap;
	Map currentMap;
	void Start()
	{
		Spawner spawner = FindObjectOfType<Spawner>();
		if (spawner != null)
		{
			spawner.OnNewWave += OnNewWave;
		}
	}

	void OnNewWave(int waveIndex)
	{
		mapIndex = waveIndex;
		GenerateMap();
	}
	public void GenerateMap(bool fromEditor = false)
	{
		currentMap = (mapIndex >= 0) ? maps[mapIndex] : trainingMap;
		System.Random rng = new System.Random(currentMap.seed);

		#region Init*/
		List<Coord> tilesCoords = new List<Coord>();
		tileMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
		// geting the Map object & Clearing Previous obstacles
		string holderName = "GeneratedMap";
		Transform mapHolder = transform.Find(holderName);
		if (mapHolder)
		{
			if (fromEditor)
			{
				DestroyImmediate(mapHolder.gameObject);
			}
			else
			{
				Destroy(mapHolder.gameObject);
			}
		}
		mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;
		#endregion

		// Creating Tiles
		#region  Creating Tiles
		Vector3 tilePos = Vector3.zero;
		for (int x = 0; x < currentMap.mapSize.x; x++)
		{
			for (int y = 0; y < currentMap.mapSize.y; y++)
			{
				//saving all coords
				tilesCoords.Add(new Coord(x, y));
				//spawn
				tilePos = CoordToPosition(x, y);
				Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(90, 0, 0)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
				//save tile
				tileMap[x, y] = newTile;
			}
		}
		#endregion

		// after tilesCoords are saved
		List<Coord> openCoords = new List<Coord>(tilesCoords);
		shuffledCoords = new Queue<Coord>(Utils.ShuffleArray(tilesCoords.ToArray(), currentMap.seed));

		// Creating Random Obstacles
		#region Creating Random Obstacles

		int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
		int currentObstacleCount = 0;
		bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

		for (int i = 0; i < obstacleCount; i++)
		{
			Coord randCoord = GetRandomCoord();
			obstacleMap[randCoord.x, randCoord.y] = true;
			currentObstacleCount++;
			if (randCoord != currentMap.center && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
			{
				float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float)rng.NextDouble());
				Vector3 position = CoordToPosition(randCoord.x, randCoord.y);
				Transform obstacle = Instantiate(obstaclePrefab, position + (Vector3.up * obstacleHeight / 2f), Quaternion.identity) as Transform;
				obstacle.parent = mapHolder;
				obstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

				Renderer obstacleRenderer = obstacle.GetComponent<Renderer>();
				Material obstacleMat = new Material(obstacleRenderer.sharedMaterial);
				float colorPercent = randCoord.y / (float)currentMap.mapSize.y;
				obstacleMat.color = Color.Lerp(currentMap.fgColor, currentMap.bgColor, colorPercent);
				obstacleRenderer.sharedMaterial = obstacleMat;

				openCoords.Remove(randCoord);


			}
			else
			{
				obstacleMap[randCoord.x, randCoord.y] = false;
				currentObstacleCount--;

			}
		}
		#endregion

		shuffledOpenCoords = new Queue<Coord>(Utils.ShuffleArray(openCoords.ToArray(), currentMap.seed));

		// NavMesh
		#region NavMesh*/
		navMeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
		// LeftMask
		Transform maskLeft = Instantiate(navMeshMask, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
		maskLeft.parent = mapHolder;
		maskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;
		// RightMask
		Transform maskRight = Instantiate(navMeshMask, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
		maskRight.parent = mapHolder;
		maskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;
		// TopMask
		Transform maskTop = Instantiate(navMeshMask, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
		maskTop.parent = mapHolder;
		maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;
		// BottomMask
		Transform BottomMask = Instantiate(navMeshMask, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
		BottomMask.parent = mapHolder;
		BottomMask.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2f) * tileSize;

		floor.localScale = new Vector3(currentMap.mapSize.x, currentMap.mapSize.y, 0.1f) * tileSize;
		#endregion

	}

	public Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(
			-currentMap.mapSize.x / 2f + 0.5f + x,
			 0,
			 -currentMap.mapSize.y / 2f + 0.5f + y
		) * tileSize;
	}
	public Transform PositonToTile(Vector3 pos)
	{
		if (pos == Vector3.zero)
		{
			Vector3 vector3 = CoordToPosition(currentMap.center.x, currentMap.center.y);
			if (vector3 != Vector3.zero)
			{
				return PositonToTile(vector3);
			}
		}
		int x = Mathf.RoundToInt(pos.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
		int y = Mathf.RoundToInt(pos.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
		x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
		y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
		return tileMap[x, y];
	}
	public Coord GetRandomCoord()
	{
		Coord randCoord = shuffledCoords.Dequeue();
		shuffledCoords.Enqueue(randCoord);
		return randCoord;
	}
	public Transform GetRandomOpenTile()
	{
		Coord randCoord = shuffledOpenCoords.Dequeue();
		shuffledOpenCoords.Enqueue(randCoord);
		return tileMap[randCoord.x, randCoord.y];
	}

	public bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
	{
		if (!accessibleMap) return true;

		//checked tiles
		bool[,] flags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
		int accessibleTileCount = 1;
		int targetAccessibleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y) - currentObstacleCount;

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(currentMap.center);
		flags[currentMap.center.x, currentMap.center.y] = true;
		while (queue.Count > 0)
		{

			Coord tile = queue.Dequeue();
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					int neighborX = tile.x + x;
					int neighborY = tile.y + y;

					// only vertical and horizontal neighbor 
					if (x != 0 & y != 0) continue;
					// outside of bounds
					if (neighborX < 0 || neighborX >= obstacleMap.GetLength(0) ||
							neighborY < 0 || neighborY >= obstacleMap.GetLength(1))
					{
						continue;
					}
					// already checked or obstacle
					if (flags[neighborX, neighborY] || obstacleMap[neighborX, neighborY]) continue;

					flags[neighborX, neighborY] = true;
					queue.Enqueue(new Coord(neighborX, neighborY));
					accessibleTileCount++;
				}
			}
		}


		return accessibleTileCount == targetAccessibleCount;
	}


	public void StartTraining()
	{
		currentMap = trainingMap;
	}

	[System.Serializable]
	public struct Coord
	{
		public int x;
		public int y;

		public static bool operator ==(Coord c1, Coord c2) => c1.x == c2.x && c1.y == c2.y;
		public static bool operator !=(Coord c1, Coord c2) => !(c1 == c2);
		public Coord(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}

	[System.Serializable]
	public class Map
	{
		public Coord mapSize;
		[Range(0, 1)]
		public float obstaclePercent;
		public int seed;
		public float minObstacleHeight;
		public float maxObstacleHeight;
		public Color bgColor;
		public Color fgColor;
		public Coord center { get => new Coord(mapSize.x / 2, mapSize.y / 2); }
	}
}
