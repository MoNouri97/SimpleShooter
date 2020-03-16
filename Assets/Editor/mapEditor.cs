using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class mapEditor : Editor
{
	float nextUp = 0;
	bool realTime = true;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		MapGenerator map = (MapGenerator)target;
		// if (base.DrawDefaultInspector())
		// {
		// 	map.GenerateMap();
		// }

		if (GUILayout.Toggle(realTime, "realTime"))
		{
			realTime = true;
			float time = Time.time;
			if (time > nextUp)
			{
				nextUp = time + .1f;
				map.GenerateMap();
			}
		}
		else
		{
			realTime = false;
		}
		if (GUILayout.Button("Generate"))
		{
			map.GenerateMap();
		}



	}

}
