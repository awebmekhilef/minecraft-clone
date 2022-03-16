using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] int _xSize;
	[SerializeField] int _zSize;

	MeshFilter _meshFilter;

	void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();

		Vector3[] vertices = new Vector3[] {
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 1),
			new Vector3(1, 0, 1),
			new Vector3(1, 0, 0),
		};

		int[] triangles = new int[] {
			0, 1, 3, 3, 1, 2
		};

		Vector2[] uv = new Vector2[] {
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0),
		};

		Mesh mesh = new Mesh()
		{
			vertices = vertices,
			triangles = triangles,
			uv = uv,
		};

		mesh.RecalculateNormals();

		_meshFilter.mesh = mesh;
	}
}
