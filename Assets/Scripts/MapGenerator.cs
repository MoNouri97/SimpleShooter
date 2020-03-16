using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Vector2 mapSize;
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	[Range(0, 1)] public float outlinePercent;
	[Range(0, 1)] public float obstaclePercent = .1f;

	Coord mapCenter;
	List<Coord> tilesCoords;
	Queue<Coord> shuffledCoords;
	public int seed;
	void Start()
	{
		GenerateMap();
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
				newTile.localScale = Vector3.one * (1 - outlinePercent);
				newTile.parent = mapHolder;
			}
		}

		// Creating Random Obstacles
		shuffledCoords = new Queue<Coord>(Utils.ShuffleArray(tilesCoords.ToArray(), seed));
		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
		bool[,] tileIsObstacle = new bool[(int)mapSize.x, (int)mapSize.y];
		for (int i = 0; i < obstacleCount; i++)
		{
			Coord randCoord = GetRandomCoord();
			Vector3 position = CoordToPosition(randCoord.x, randCoord.y);
			Transform obstacle = Instantiate(obstaclePrefab, position + Vector3.up * .5f, Quaternion.identity) as Transform;
			obstacle.parent = mapHolder;
		}

	}

	private Vector3 CoordToPosition(int x, int y)
	{
		return new Vector3(
			-mapSize.x / 2 + 0.5f + x,
			 0,
			 -mapSize.y / 2 + 0.5f + y
		);
	}

	public Coord GetRandomCoord()
	{
		Coord randCoord = shuffledCoords.Dequeue();
		shuffledCoords.Enqueue(randCoord);
		return randCoord;
	}

	public bool MapIsFullyAccesible()
	{

		return true;
	}
	public struct Coord
	{
		public int x;
		public int y;

		public Coord(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

}
