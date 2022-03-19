using UnityEngine;

public class TerrainDisplay : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] int _size;
	[SerializeField] int _seed;
	[SerializeField] int _heightMultiplier;
	[SerializeField] int _scale;
	[SerializeField] int _octaves;
	[SerializeField] float _persistence;
	[SerializeField] float _lacunarity;
	[SerializeField] Gradient _colorGradient;
	[SerializeField] Gradient _fallOffGradient;
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
		float[,] noiseMap = Noise.Generate(_size, _size, _seed, _scale, _octaves, _persistence, _lacunarity);

		for (int i = 0; i < _size; i++)
		{
			for (int j = 0; j < _size; j++)
			{
				float x = (float)i / _size * 2 - 1;
				float y = (float)j / _size * 2 - 1;

				float max = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
				noiseMap[i, j] = noiseMap[i, j] * _fallOffGradient.Evaluate(max).a;
			}
		}

		_meshFilter.mesh = MeshGenerator.GenerateMesh(noiseMap, _heightMultiplier, _heightCurve);
		_meshRenderer.sharedMaterial.mainTexture = TextureGenerator.GenerateColoredTexture(noiseMap, _colorGradient);
	}
}
