using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public const int Width = 8;
	public const int Height = 24;
	public const int MaxViewDst = 2;

	public const int GroundHeight = 12;
	public const int TerrainHeight = 12;

	// The physical representation in the game world
	GameObject _go;
	MeshFilter _meshFilter;

	// Blocks in the chunk
	BlockId[,,] _blocks = new BlockId[Width, Height, Width];

	// Mesh Data
	List<Vector3> _vertices = new List<Vector3>();
	List<Vector2> _uvs = new List<Vector2>();
	List<int> _triangles = new List<int>();

	// XZ chunk coordinates
	public Vector2Int Coords { get; private set; }

	// To make sure chunks are built after block ids are populated
	public bool HasBuiltMesh { get; private set; }

	// Whether or not the gameobject is enabled
	public bool IsVisible
	{
		get { return _go.activeSelf; }
		set { _go.SetActive(value); }
	}

	public Chunk(Vector2Int coords)
	{
		Coords = coords;

		InitGameObject();
		PopulateBlockIds();

		IsVisible = false;
	}

	void InitGameObject()
	{
		_go = new GameObject($"Chunk {Coords.x}, {Coords.y}");

		// Place gameobject at center of coord
		_go.transform.position = new Vector3(Coords.x * Width - Width / 2f, 0f, Coords.y * Width - Width / 2f);

		_meshFilter = _go.AddComponent<MeshFilter>();
		_go.AddComponent<MeshRenderer>().material = BlockDatabase.Instance.ChunkMaterial;
	}

	void PopulateBlockIds()
	{
		float[,] noiseMap = Noise.Generate(Width, Width, 15f, new Vector2(Coords.x * Width, Coords.y * Width));
		int[,] elevations = new int[Width, Width];

		for (int x = 0; x < Width; x++)
		{
			for (int z = 0; z < Width; z++)
			{
				elevations[x, z] = (int)(GroundHeight + noiseMap[x, z] * TerrainHeight);
			}
		}

		for (int x = 0; x < Width; x++)
		{
			for (int z = 0; z < Width; z++)
			{
				int maxHeight = elevations[x, z] - 1;

				_blocks[x, maxHeight, z] = BlockId.Grass;
				_blocks[x, 0, z] = BlockId.Bedrock;

				for (int y = 1; y < maxHeight; y++)
				{
					if (y > maxHeight - 4)
						_blocks[x, y, z] = BlockId.Dirt;
					else
						_blocks[x, y, z] = BlockId.Stone;
				}
			}
		}
	}

	public void BuildMesh()
	{
		HasBuiltMesh = true;

		AdjacentBlockChecks directions = new AdjacentBlockChecks();

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int z = 0; z < Width; z++)
				{
					BlockId blockId = _blocks[x, y, z];

					if (blockId == BlockId.Air)
						continue;

					BlockData data = BlockDatabase.Instance.GetBlockData(blockId);

					directions.Update(x, y, z);

					if(y != 0)
						AddFaceToMesh(BlockData.BottomFaces, new Vector3Int(x, y, z), directions.Down, data.TexBottom);

					AddFaceToMesh(BlockData.TopFaces, new Vector3Int(x, y, z), directions.Up, data.TexTop);
					AddFaceToMesh(BlockData.LeftFaces, new Vector3Int(x, y, z), directions.Left, data.TexSide);
					AddFaceToMesh(BlockData.RightFaces, new Vector3Int(x, y, z), directions.Right, data.TexSide);
					AddFaceToMesh(BlockData.FrontFaces, new Vector3Int(x, y, z), directions.Front, data.TexSide);
					AddFaceToMesh(BlockData.BackFaces, new Vector3Int(x, y, z), directions.Back, data.TexSide);
				}
			}
		}

		Mesh mesh = new Mesh()
		{
			vertices = _vertices.ToArray(),
			triangles = _triangles.ToArray(),
			uv = _uvs.ToArray(),
		};

		mesh.RecalculateNormals();

		_meshFilter.mesh = mesh;
	}

	void AddFaceToMesh(int[] faces, Vector3Int position, Vector3Int adjBlockPos, Vector2 texCoords)
	{
		if (!ShouldAddFace(adjBlockPos))
			return;

		_vertices.Add(position + BlockData.Vertices[faces[0]]);
		_vertices.Add(position + BlockData.Vertices[faces[1]]);
		_vertices.Add(position + BlockData.Vertices[faces[2]]);
		_vertices.Add(position + BlockData.Vertices[faces[3]]);

		Vector2[] uvCoords = TextureAtlas.GetUVCoords(texCoords);
		_uvs.Add(uvCoords[0]);
		_uvs.Add(uvCoords[1]);
		_uvs.Add(uvCoords[2]);
		_uvs.Add(uvCoords[3]);

		int currentVertex = _vertices.Count - 4;

		_triangles.Add(currentVertex + 0);
		_triangles.Add(currentVertex + 1);
		_triangles.Add(currentVertex + 2);
		_triangles.Add(currentVertex + 2);
		_triangles.Add(currentVertex + 1);
		_triangles.Add(currentVertex + 3);
	}

	public BlockId GetBlock(int x, int y, int z)
	{
		// Only check for Y level out of bounds since this function is only called when there is a guaranteed chunk
		// and the world position has been converted to the relative position (XZ)
		if (y < 0 || y > Height - 1)
			return BlockId.Air;

		return _blocks[x, y, z];
	}

	public int ToRelativeX(int x)
	{
		return x - Coords.x * Width;
	}

	public int ToRelativeZ(int z)
	{
		return z - Coords.y * Width;
	}

	bool IsOutOfBounds(int x, int y, int z)
	{
		return x < 0 || x > Width - 1 || y < 0 || y > Height - 1 || z < 0 || z > Width - 1;
	}

	// TODO: Only works for initally generated chunks
	bool ShouldAddFace(Vector3Int adjBlockPos)
	{
		if (IsOutOfBounds(adjBlockPos.x, adjBlockPos.y, adjBlockPos.z))
		{
			Vector3Int adjWorldPos = ToWorldPosition(adjBlockPos.x, adjBlockPos.y, adjBlockPos.z);
			return World.Instance.GetBlock(adjWorldPos.x, adjWorldPos.y, adjWorldPos.z) == BlockId.Air;
		}

		return _blocks[adjBlockPos.x, adjBlockPos.y, adjBlockPos.z] == BlockId.Air;
	}

	Vector3Int ToWorldPosition(int x, int y, int z)
	{
		return new Vector3Int(
			x + Coords.x * Width,
			y,
			z + Coords.y * Width
		);
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
