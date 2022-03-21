using UnityEngine;

public static class VoxelData
{
	// Need to duplicate vertices (8 -> 24) to have hard normal edges
	public static Vector3[] Vertices = new Vector3[] {
		// Back
        new Vector3(1, 0, 0),
		new Vector3(0, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(1, 1, 0),

        // Front
        new Vector3(0, 0, 1),
		new Vector3(1, 0, 1),
		new Vector3(1, 1, 1),
		new Vector3(0, 1, 1),

        // Right
        new Vector3(1, 0, 1),
		new Vector3(1, 0, 0),
		new Vector3(1, 1, 0),
		new Vector3(1, 1, 1),

        // Left
        new Vector3(0, 0, 0),
		new Vector3(0, 0, 1),
		new Vector3(0, 1, 1),
		new Vector3(0, 1, 0),

        // Top
        new Vector3(0, 1, 1),
		new Vector3(1, 1, 1),
		new Vector3(1, 1, 0),
		new Vector3(0, 1, 0),

        // Bottom
        new Vector3(0, 0, 0),
		new Vector3(1, 0, 0),
		new Vector3(1, 0, 1),
		new Vector3(0, 0, 1),
	};

	// Indices for the vertices array
	public static int[] Triangles = new int[] {
		0, 1, 2,
		2, 3, 0,

		4, 5, 6,
		6, 7, 4,

		8, 9, 10,
		10, 11, 8,

		12, 13, 14,
		14, 15, 12,

		16, 17, 18,
		18, 19, 16,

		20, 21, 22,
		22, 23, 20
	};
}
