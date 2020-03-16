using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class mapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		MapGenerator map = (MapGenerator)target;

		if (GUILayout.Button("Genrate"))
		{
			Debug.Log("btn");
			map.GenerateMap();
		}

	}

}
