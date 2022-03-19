using UnityEngine;

public class MapDisplay : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] int _width;
	[SerializeField] int _height;
	[SerializeField] int _scale;
	[SerializeField] Gradient _gradient;

	MeshFilter _meshFilter;
	MeshRenderer _meshRenderer;

	void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();
		_meshRenderer = GetComponent<MeshRenderer>();

		float[,] noiseMap = Noise.Generate(_width, _height, _scale);
		_meshFilter.mesh = MeshGenerator.GenerateMesh(noiseMap);
		_meshRenderer.material.mainTexture = TextureGenerator.GenerateColoredTexture(noiseMap, _gradient);
	}
}
