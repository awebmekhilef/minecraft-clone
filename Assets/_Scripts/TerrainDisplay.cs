using UnityEngine;

public class TerrainDisplay : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] int _width;
	[SerializeField] int _height;
	[SerializeField] int _seed;
	[SerializeField] int _heightMultiplier;
	[SerializeField] int _scale;
	[SerializeField] int _octaves;
	[SerializeField] float _persistence;
	[SerializeField] float _lacunarity;
	[SerializeField] Gradient _gradient;
	[SerializeField] AnimationCurve _heightCurve;

	MeshFilter _meshFilter;
	MeshRenderer _meshRenderer;

	void OnValidate()
	{
		_meshFilter = GetComponent<MeshFilter>();
		_meshRenderer = GetComponent<MeshRenderer>();
	}

	public void GenerateTerrain()
	{
		float[,] noiseMap = Noise.Generate(_width, _height, _seed, _scale, _octaves, _persistence, _lacunarity);
		_meshFilter.mesh = MeshGenerator.GenerateMesh(noiseMap, _heightMultiplier, _heightCurve);
		_meshRenderer.sharedMaterial.mainTexture = TextureGenerator.GenerateColoredTexture(noiseMap, _gradient);
	}
}
