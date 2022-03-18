using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[Header("Terrain")]
	[SerializeField] int _xSize;
	[SerializeField] int _zSize;
	[SerializeField] float _minHeight;
	[SerializeField] float _maxHeight;
	[SerializeField] Gradient _gradient;

	[Header("Noise")]
	//[SerializeField] float _octaves;
	//[SerializeField] float _lacunarity;
	//[SerializeField] float _persistence;
	[SerializeField] float _scale;

	[Header("Debug")]
	[SerializeField] bool _drawDebugVertices;

	Vector3[] _vertices;
	int[] _triangles;
	Vector2[] _uvs;
	Color[] _colors;

	MeshFilter _meshFilter;
	Mesh _mesh;

	void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();
		_mesh = new Mesh();
	}

	void Update()
	{
		GenerateMesh();
		_meshFilter.mesh = _mesh;
	}

	void OnDrawGizmos()
	{
		if (!_drawDebugVertices || _vertices == null)
			return;

		foreach (var vertex in _vertices)
			Gizmos.DrawSphere(vertex, 0.1f);
	}

	void GenerateMesh()
	{
		_mesh.Clear();

		int vertexCount = (_xSize + 1) * (_zSize + 1);

		_vertices = new Vector3[vertexCount];
		_uvs = new Vector2[vertexCount];
		_colors = new Color[vertexCount];

		for (int z = 0, i = 0; z < _zSize + 1; z++)
		{
			for (int x = 0; x < _xSize + 1; x++, i++)
			{
				float t = Mathf.PerlinNoise(x / _scale, z / _scale);
				float y = Mathf.Lerp(_minHeight, _maxHeight, t);

				_vertices[i] = new Vector3(x, y, z);
				_uvs[i] = new Vector2(x / _xSize, z / _zSize);
				_colors[i] = _gradient.Evaluate(t);
			}
		}

		_triangles = new int[_xSize * _zSize * 6];

		int vert = 0;
		int tris = 0;

		// Loop left to right, bottom to top
		for (int z = 0; z < _zSize; z++)
		{
			for (int x = 0; x < _xSize; x++)
			{
				_triangles[tris + 0] = vert;
				_triangles[tris + 1] = vert + _xSize + 1;
				_triangles[tris + 2] = vert + 1;
				_triangles[tris + 3] = vert + 1;
				_triangles[tris + 4] = vert + _xSize + 1;
				_triangles[tris + 5] = vert + _xSize + 2;

				vert++;
				tris += 6;
			}

			// Skip the last vertex in the row
			vert++;
		}

		_mesh.vertices = _vertices;
		_mesh.triangles = _triangles;
		_mesh.uv = _uvs;
		_mesh.colors = _colors;

		_mesh.RecalculateNormals();
	}
}
