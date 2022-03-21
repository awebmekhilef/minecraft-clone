using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
	void Start()
	{
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();

		// Add the vertices, triangles and uvs for each face
		int vert = 0;
		for (int i = 0; i < 6; i++)
		{
			// Add the position and uv coord for each vertex
			for (int j = 0; j < 4; j++)
			{
				vertices.Add(VoxelData.Vertices[VoxelData.Triangles[i, j]]);
				uvs.Add(VoxelData.UVs[j]);
			}

			// Follow the pattern defined in VoxelData
			triangles.Add(vert + 0);
			triangles.Add(vert + 1);
			triangles.Add(vert + 2);
			triangles.Add(vert + 2);
			triangles.Add(vert + 1);
			triangles.Add(vert + 3);

			vert += 4;
		}

		Mesh mesh = new Mesh()
		{
			vertices = vertices.ToArray(),
			triangles = triangles.ToArray(),
			uv = uvs.ToArray(),
		};

		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = mesh;
	}
}
