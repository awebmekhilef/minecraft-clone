using UnityEngine;

public class Cube : MonoBehaviour
{
	void Start()
	{
		Mesh mesh = new Mesh()
		{
			vertices = VoxelData.Vertices,
			triangles = VoxelData.Triangles,
		};

		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = mesh;
	}
}
