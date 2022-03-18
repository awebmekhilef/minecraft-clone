using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[Header("Terrain")]
	[SerializeField] int _xSize;
	[SerializeField] int _zSize;
	[SerializeField] float _scale;
	[SerializeField] Gradient _gradient;

	[Header("Debug")]
	[SerializeField] bool _drawDebugVertices;

	Vector3[] _vertices;
	int[] _triangles;
	Vector2[] _uvs;
	Color[] _colors;

	MeshFilter _meshFilter;

	void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();

		int vertexCount = (_xSize + 1) * (_zSize + 1);

		_vertices = new Vector3[vertexCount];
		_uvs = new Vector2[vertexCount];
		_colors = new Color[vertexCount];

		float[,] noiseMap = Noise.Generate(_xSize + 1, _zSize + 1, _scale);

		for (int z = 0; z < _zSize + 1; z++)
		{
			for (int x = 0; x < _xSize + 1; x++)
			{
				int i = z * (_xSize + 1) + x;
				float y = noiseMap[x, z];

				_vertices[i] = new Vector3(x, y, z);
				_uvs[i] = new Vector2(x / _xSize, z / _zSize);
				_colors[i] = _gradient.Evaluate(y);
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

		Mesh mesh = new Mesh()
		{
			vertices = _vertices,
			triangles = _triangles,
			uv = _uvs,
			colors = _colors,
		};

		mesh.RecalculateNormals();

		_meshFilter.mesh = mesh;
	}

	void OnDrawGizmos()
	{
		if (!_drawDebugVertices || _vertices == null)
			return;

		foreach (var vertex in _vertices)
			Gizmos.DrawSphere(vertex, 0.1f);
	}
}
