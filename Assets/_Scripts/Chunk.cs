using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public const int Width = 5;
	public const int Height = 10;

	// The physical representation in the game world
	GameObject _go;

	MeshFilter _meshFilter;

	// Blocks in the chunk
	BlockId[,,] _blocks = new BlockId[Width, Height, Width];

	// Mesh Data
	List<Vector3> _vertices = new List<Vector3>();
	List<int> _triangles = new List<int>();

	public Chunk()
	{
		InitGameObject();
		PopulateBlockIds();
		BuildMesh();
	}

	void InitGameObject()
	{
		_go = new GameObject("Chunk");
		_meshFilter = _go.AddComponent<MeshFilter>();
		_go.AddComponent<MeshRenderer>();
	}

	void PopulateBlockIds()
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int z = 0; z < Width; z++)
				{
					_blocks[x, y, z] = BlockId.Dirt;
				}
			}
		}
	}

	void BuildMesh()
	{
		AdjacentBlockChecks directions = new AdjacentBlockChecks();

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int z = 0; z < Width; z++)
				{
					directions.Update(x, y, z);

					AddFaceToMesh(BlockData.TopFaces, new Vector3Int(x, y, z), directions.Up);
					AddFaceToMesh(BlockData.BottomFaces, new Vector3Int(x, y, z), directions.Down);
					AddFaceToMesh(BlockData.LeftFaces, new Vector3Int(x, y, z), directions.Left);
					AddFaceToMesh(BlockData.RightFaces, new Vector3Int(x, y, z), directions.Right);
					AddFaceToMesh(BlockData.FrontFaces, new Vector3Int(x, y, z), directions.Front);
					AddFaceToMesh(BlockData.BackFaces, new Vector3Int(x, y, z), directions.Back);
				}
			}
		}

		Mesh mesh = new Mesh()
		{
			vertices = _vertices.ToArray(),
			triangles = _triangles.ToArray()
		};

		_meshFilter.mesh = mesh;
	}

	void AddFaceToMesh(int[] faces, Vector3Int position, Vector3Int adjBlockDirection)
	{
		if (GetBlock(adjBlockDirection.x, adjBlockDirection.y, adjBlockDirection.z) != BlockId.Air)
			return;

		_vertices.Add(position + BlockData.Vertices[faces[0]]);
		_vertices.Add(position + BlockData.Vertices[faces[1]]);
		_vertices.Add(position + BlockData.Vertices[faces[2]]);
		_vertices.Add(position + BlockData.Vertices[faces[3]]);

		int currentVertex = _vertices.Count - 4;

		_triangles.Add(currentVertex + 0);
		_triangles.Add(currentVertex + 1);
		_triangles.Add(currentVertex + 2);
		_triangles.Add(currentVertex + 2);
		_triangles.Add(currentVertex + 1);
		_triangles.Add(currentVertex + 3);
	}

	BlockId GetBlock(int x, int y, int z)
	{
		if (IsOutOfBounds(x, y, z))
			return BlockId.Air;

		return _blocks[x, y, z];
	}

	bool IsOutOfBounds(int x, int y, int z)
	{
		return x < 0 || x > Width - 1 || y < 0 || y > Height - 1 || z < 0 || z > Width - 1;
	}

	struct AdjacentBlockChecks
	{
		public Vector3Int Up { get; private set; }
		public Vector3Int Down { get; private set; }
		public Vector3Int Left { get; private set; }
		public Vector3Int Right { get; private set; }
		public Vector3Int Front { get; private set; }
		public Vector3Int Back { get; private set; }

		public void Update(int x, int y, int z)
		{
			Up = new Vector3Int(x, y + 1, z);
			Down = new Vector3Int(x, y - 1, z);
			Left = new Vector3Int(x - 1, y, z);
			Right = new Vector3Int(x + 1, y, z);
			Front = new Vector3Int(x, y, z - 1);
			Back = new Vector3Int(x, y, z + 1);
		}
	}
}
