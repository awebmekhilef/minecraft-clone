using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainDisplay))]
public class TerrainDisplayEditor : Editor
{
	public override void OnInspectorGUI()
	{
		TerrainDisplay terrainDisplay = (TerrainDisplay)target;

		if (DrawDefaultInspector())
			terrainDisplay.GenerateTerrain();

		GUILayout.Space(15);

		if (GUILayout.Button("Generate"))
			terrainDisplay.GenerateTerrain();
	}
}
