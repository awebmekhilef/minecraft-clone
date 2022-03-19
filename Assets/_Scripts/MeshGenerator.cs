using UnityEngine;

public static class MeshGenerator
{
	public static Mesh GenerateMesh(float[,] heightMap)
	{
		// Each element is a vertex
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		Vector3[] vertices = new Vector3[width * height];
		int[] triangles = new int[(width - 1) * (height - 1) * 6];
		Vector2[] uvs = new Vector2[width * height];

		int currVert = 0;
		int currTris = 0;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				vertices[currVert] = new Vector3(x, heightMap[x, y], y);
				uvs[currVert] = new Vector2(x / width, y / height);

				if (x < width - 1 && y < height - 1)
				{
					triangles[currTris + 0] = currVert;
					triangles[currTris + 1] = currVert + width + 1;
					triangles[currTris + 2] = currVert + 1;
					triangles[currTris + 3] = currVert + width;
					triangles[currTris + 4] = currVert + width + 1;
					triangles[currTris + 5] = currVert;

					currTris += 6;
				}

				currVert++;
			}
		}

		Mesh mesh = new Mesh()
		{
			vertices = vertices,
			triangles = triangles,
			uv = uvs,
		};

		mesh.RecalculateNormals();

		return mesh;
	}
}
