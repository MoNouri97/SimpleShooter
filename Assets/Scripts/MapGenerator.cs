using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Vector2 mapSize;
	public Transform tilePrefab;
	public float tileSize;
	public Transform obstaclePrefab;
	[Range(0, 1)] public float outlinePercent;
	[Range(0, 1)] public float obstaclePercent = .1f;
	public bool accessibleMap = false;
	Coord mapCenter;
	List<Coord> tilesCoords;
	Queue<Coord> shuffledCoords;
	public int seed;
	void Start()
	{
		//GenerateMap();
	}
	public void GenerateMap()
	{
		mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);
		tilesCoords = new List<Coord>();


		// geting the Map object & Clearing Previous obstacles
		string holderName = "GeneratedMap";
		Transform mapHolder = transform.Find(holderName);
		if (mapHolder)
		{
			DestroyImmediate(mapHolder.gameObject);
		}
		mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;

		// Creating Tiles
		Vector3 tilePos = Vector3.zero;
		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				//saving all coords
				tilesCoords.Add(new Coord(x, y));
				//instantiate
				tilePos = CoordToPosition(x, y);
				Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(90, 0, 0)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
			}
		}

		// Creating Random Obstacles
		shuffledCoords = new Queue<Coord>(Utils.ShuffleArray(tilesCoords.ToArray(), seed));
		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
		int currentObstacleCount = 0;
		bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];
		for (int i = 0; i < obstacleCount; i++)
		{
			Coord randCoord = GetRandomCoord();
			obstacleMap[randCoord.x, randCoord.y] = true;
			currentObstacleCount++;
			if (randCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
			{
				Vector3 position = CoordToPosition(randCoord.x, randCoord.y);
				Transform obstacle = Instantiate(obstaclePrefab, position + Vector3.up * .5f * tileSize, Quaternion.identity) as Transform;
				obstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;

				obstacle.parent = mapHolder;
			}
			else
			{
				obstacleMap[randCoord.x, randCoord.y] = false;
				currentObstacleCount--;

			}
		}

	}

	private Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(
			-mapSize.x / 2 + 0.5f + x,
			 0,
			 -mapSize.y / 2 + 0.5f + y
		) * tileSize;
	}

	public Coord GetRandomCoord()
	{
		Coord randCoord = shuffledCoords.Dequeue();
		shuffledCoords.Enqueue(randCoord);
		return randCoord;
	}

	public bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
	{
		if (!accessibleMap) return true;

		//checked tiles
		bool[,] flags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
		int accessibleTileCount = 1;
		int targetAccessibleCount = (int)(mapSize.x * mapSize.y) - currentObstacleCount;

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(mapCenter);
		flags[mapCenter.x, mapCenter.y] = true;
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

}
