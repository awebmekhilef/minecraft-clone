using UnityEngine;

public class MapDisplay : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] int scale;

	MeshFilter _meshFilter;

	void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();

		float[,] noiseMap = Noise.Generate(width, height, scale);
		_meshFilter.mesh = MeshGenerator.GenerateMesh(noiseMap);
	}
}
