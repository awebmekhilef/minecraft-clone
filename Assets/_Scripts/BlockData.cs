using UnityEngine;

[CreateAssetMenu]
public class BlockData : ScriptableObject
{
	public BlockID ID;

	public Vector2 TexTop, TexSide, TexBottom;

	public bool IsOpaque;

	public static Vector3[] Vertices = new Vector3[]
	{
		new Vector3(0, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(1, 1, 0),
		new Vector3(1, 0, 0),
		new Vector3(0, 0, 1),
		new Vector3(0, 1, 1),
		new Vector3(1, 1, 1),
		new Vector3(1, 0, 1),
	};

	public static int[] FrontFaces = new int[]
	{ 0, 1, 3, /* 3, 1, */ 2 };

	public static int[] BackFaces = new int[]
	{ 7, 6, 4, /* 4, 6, */ 5 };

	public static int[] RightFaces = new int[]
	{ 3, 2, 7, /* 7, 2, */ 6 };

	public static int[] LeftFaces = new int[]
	{ 4, 5, 0, /* 0, 5, */ 1 };

	public static int[] TopFaces = new int[]
	{ 1, 5, 2, /* 2, 5, */ 6 };

	public static int[] BottomFaces = new int[]
	{ 4, 0, 7, /* 7, 0, */ 3 };
}

public enum BlockID
{
	Air,
	Grass,
	Dirt,
	Stone,
	Bedrock,
	Brick,
	Glass,
	WoodLog,
	WoodPlank,
	Cobblestone,
	Leaves,
}
