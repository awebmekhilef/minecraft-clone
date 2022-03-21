using UnityEngine;

public static class VoxelData
{
	public const int ChunkSize = 5;
	public const int ChunkHeight = 5;

	public static Vector3[] Vertices = new Vector3[] {
		new Vector3(0, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(1, 1, 0),
		new Vector3(1, 0, 0),

		new Vector3(0, 0, 1),
		new Vector3(0, 1, 1),
		new Vector3(1, 1, 1),
		new Vector3(1, 0, 1),
	};

	// Indices for each vertex (skip unnecessary duplicates)
	public static int[,] Triangles = new int[,] {
		{ 0, 1, 3, /* 3, 1, */ 2 }, // Front
		{ 7, 6, 4, /* 4, 6, */ 5 }, // Back
		{ 3, 2, 7, /* 7, 2, */ 6 }, // Right
		{ 4, 5, 0, /* 0, 5, */ 1 }, // Left
		{ 1, 5, 2, /* 2, 5, */ 6 }, // Top
		{ 4, 0, 7, /* 7, 0, */ 3 }, // Bottom
	};

	// UV coordinatefor every triangle point
	public static Vector2[] UVs = new Vector2[] {
		new Vector2(0, 0),
		new Vector2(0, 1),
		new Vector2(1, 0),
		new Vector2(1, 1),
	};
}
