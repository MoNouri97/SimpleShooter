using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerUI : MonoBehaviour
{
	[SerializeField] MapGenerator mapGenerator;
	MapGenerator.Map map;
	void Start()
	{
		map = new MapGenerator.Map(new Vector2Int(10, 10), 30, 0, 0, 0.5f, Color.magenta, Color.blue);
		mapGenerator.maps[0] = map;
		mapGenerator.mapIndex = 0;
		mapGenerator.GenerateMap();
	}

	public void UpdateMap()
	{
		mapGenerator.GenerateMap();
	}


	#region Size


	public void setMapSizeX(string sizeX)
	{
		SetSize(sizeX);
	}
	public void setMapSizeY(string sizeX)
	{
		SetSize(sizeX, false);
	}

	private void SetSize(string size, bool thisIsX = true)
	{
		int val = (int)GetNumber(size, 0, 17);
		if (thisIsX)
		{
			map.mapSize.x = val;
		}
		else
		{
			map.mapSize.y = val;
		}
		UpdateMap();
	}


	#endregion

	#region Height
	public void SetMinHeight(string height)
	{
		float toSet = GetNumber(height, 0, 1, true);
		map.minObstacleHeight = toSet;
		UpdateMap();
	}
	public void SetMaxHeight(string height)
	{
		float toSet = GetNumber(height, 0, 3, true);
		map.maxObstacleHeight = toSet;
		UpdateMap();
	}
	public void SetMaxHeight(float height)
	{
		map.maxObstacleHeight = height;
		// map.minObstacleHeight = Mathf.Clamp(height - 5, 0, height);
		UpdateMap();
	}
	#endregion

	#region Obstacles
	public void SetPercent(string per)
	{
		float p = GetNumber(per, 0, 100, false) / 100;
		Debug.Log(p);
		map.obstaclePercent = p;
		UpdateMap();
	}
	public void SetSeed(string s)
	{
		map.seed = (int)GetNumber(s, 0, 100, false);
		UpdateMap();
	}
	#endregion
	private float GetNumber(string val, float min, float max, bool isFloat = false)
	{
		float r = 0;
		try
		{
			r = isFloat ? int.Parse(val) : float.Parse(val);
			r = Mathf.Clamp(r, min, max);
		}
		catch (System.Exception)
		{
			Debug.Log("Nan");
		}
		Debug.Log(r);
		return r;
	}
}
