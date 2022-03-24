using UnityEngine;
using System.Collections.Generic;
using System;

public class Chunk
{
	List<Vector3> _vertices = new List<Vector3>();
	List<Vector2> _uvs = new List<Vector2>();
	List<int> _triangles = new List<int>();

	// The current vertex being added
	int _vert = 0;

	// The physical representation in the game world
	GameObject _go;

	// 2D world position
	Vector2 _position;

	World _world;

	// For now blocks are either solid or not, replace with block ids later on
	int[,,] _blocks = new int[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

	public Chunk(Vector2 coords, World world, Material material)
	{
		_world = world;

		// Set block ids
		LoopOverChunk((x, y, z) =>
		{
			//_blocks[x, y, z] = 3;
			if (y == 0)
				_blocks[x, y, z] = 3;
			else if (y > 0 && y <= VoxelData.ChunkHeight - 4)
				_blocks[x, y, z] = 2;
			else if (y > VoxelData.ChunkHeight - 4 && y < VoxelData.ChunkHeight - 1)
				_blocks[x, y, z] = 1;
			else
				_blocks[x, y, z] = 0;
		});

		// Create mesh
		LoopOverChunk((x, y, z) => AddBlockMesh(new Vector3(x, y, z)));

		Mesh mesh = new Mesh()
		{
			vertices = _vertices.ToArray(),
			triangles = _triangles.ToArray(),
			uv = _uvs.ToArray(),
		};

		mesh.RecalculateNormals();

		// Create gameobject
		_go = new GameObject($"Chunk {coords.x}, {coords.y}");
		_go.transform.position = new Vector3(coords.x * VoxelData.ChunkWidth, 0f, coords.y * VoxelData.ChunkWidth);
		_go.AddComponent<MeshFilter>().mesh = mesh;
		_go.AddComponent<MeshRenderer>().material = material;

		_position = new Vector2(_go.transform.position.x + VoxelData.ChunkWidth / 2f, _go.transform.position.z + VoxelData.ChunkWidth / 2f);

		SetVisible(false);
	}

	// Disables gameobject if distance to viewer less than max view distance
	public void UpdateViewDst()
	{
		float dst = Vector2.Distance(_position, World.ViewerPos);
		SetVisible(dst <= VoxelData.ChunkViewDst * VoxelData.ChunkWidth);
	}

	public void SetVisible(bool visible)
	{
		_go.SetActive(visible);
	}

	public bool IsVisible()
	{
		return _go.activeSelf;
	}

	void LoopOverChunk(Action<int, int, int> action)
	{
		for (int x = 0; x < VoxelData.ChunkWidth; x++)
		{
			for (int y = 0; y < VoxelData.ChunkHeight; y++)
			{
				for (int z = 0; z < VoxelData.ChunkWidth; z++)
				{
					action(x, y, z);
				}
			}
		}
	}

	void AddBlockMesh(Vector3 position)
	{
		// For each face add the vertices, triangles and uvs
		for (int i = 0; i < 6; i++)
		{
			// Skip the face if it is inside the chunk (not visible)
			if (IsInsideChunk(Vector3Int.FloorToInt(position) + VoxelData.NeighborChecks[i]))
				continue;

			// For each vertex add the position
			for (int j = 0; j < 4; j++)
				_vertices.Add(position + VoxelData.Vertices[VoxelData.Triangles[i, j]]);

			SetTexture(_blocks[(int)position.x, (int)position.y, (int)position.z], i);

			// Follow the pattern defined in VoxelData.Triangles
			_triangles.Add(_vert + 0);
			_triangles.Add(_vert + 1);
			_triangles.Add(_vert + 2);
			_triangles.Add(_vert + 2);
			_triangles.Add(_vert + 1);
			_triangles.Add(_vert + 3);

			_vert += 4;
		}
	}

	// may god have mercy on whoever has to read this shit...
	void SetTexture(int blockId, int face)
	{
		BlockData data = _world.GetBlock(blockId);

		int textureIndex;
		if (face == 5)
			textureIndex = data.TexBottom;
		else if (face == 4)
			textureIndex = data.TexTop;
		else
			textureIndex = data.TexSide;

		int x = textureIndex % VoxelData.TextureRows;
		int y = (textureIndex - x) / VoxelData.TextureRows;

		float normalizedWidth = 1f / VoxelData.TextureRows;
		float normalizedHeight = 1f / VoxelData.TextureCols;

		float uvX = x * normalizedWidth;
		float uvY = normalizedHeight - (y * normalizedHeight);

		_uvs.Add(new Vector2(uvX, uvY));
		_uvs.Add(new Vector2(uvX, uvY + normalizedHeight));
		_uvs.Add(new Vector2(uvX + normalizedWidth, uvY));
		_uvs.Add(new Vector2(uvX + normalizedWidth, uvY + normalizedHeight));
	}

	bool IsInsideChunk(Vector3Int position)
	{
		if (position.x < 0 || position.x > _blocks.GetLength(0) - 1 ||
			position.y < 0 || position.y > _blocks.GetLength(1) - 1 ||
			position.z < 0 || position.z > _blocks.GetLength(2) - 1)
			return false;

		return _blocks[position.x, position.y, position.z] != -1;
	}
}
