using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World>
{
	[SerializeField] Transform _viewer;

	Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	List<Chunk> _chunksViewedLastFrame = new List<Chunk>();

	public Vector2Int PlayerChunk { get; private set; }

	void Update()
	{
		int viewerPosX = Mathf.FloorToInt(_viewer.position.x / Chunk.Width);
		int viewerPosZ = Mathf.FloorToInt(_viewer.position.z / Chunk.Width);

		PlayerChunk = new Vector2Int(viewerPosX, viewerPosZ);

		for (int i = 0; i < _chunksViewedLastFrame.Count; i++)
			_chunksViewedLastFrame[i].IsVisible = false;

		_chunksViewedLastFrame.Clear();

		for (int xOffset = -Chunk.MaxViewDst; xOffset <= Chunk.MaxViewDst; xOffset++)
		{
			for (int zOffset = -Chunk.MaxViewDst; zOffset <= Chunk.MaxViewDst; zOffset++)
			{
				Vector2Int viewedChunkCoords = new Vector2Int(xOffset + viewerPosX, zOffset + viewerPosZ);

				if (_chunks.ContainsKey(viewedChunkCoords))
				{
					Chunk viewedChunk = _chunks[viewedChunkCoords];

					viewedChunk.IsVisible = true;
					_chunksViewedLastFrame.Add(viewedChunk);

					// If chunk was just added build its mesh
					if (!viewedChunk.HasInitializedMesh)
					{
						// Load adjacent chunk data the first time mesh is built
						LoadChunk(viewedChunkCoords.x + 1, viewedChunkCoords.y);
						LoadChunk(viewedChunkCoords.x - 1, viewedChunkCoords.y);
						LoadChunk(viewedChunkCoords.x, viewedChunkCoords.y + 1);
						LoadChunk(viewedChunkCoords.x, viewedChunkCoords.y - 1);

						viewedChunk.BuildMesh();
					}
				}
				else
					_chunks.Add(viewedChunkCoords, new Chunk(viewedChunkCoords));
			}
		}
	}

	public BlockID GetBlock(int x, int y, int z)
	{
		Chunk chunk = GetChunkFor(x, z);

		if (chunk == null)
			return BlockID.Air;

		return chunk.GetBlock(chunk.ToRelativeX(x), y, chunk.ToRelativeZ(z));
	}

	public void SetBlock(int x, int y, int z, BlockID blockID)
	{
		Chunk chunk = GetChunkFor(x, z);

		if (chunk == null)
			return;

		int relX = chunk.ToRelativeX(x);
		int relZ = chunk.ToRelativeZ(z);

		chunk.SetBlock(relX, y, relZ, blockID);
		chunk.BuildMesh();

		// Only build the mesh if adjacent chunks have been loaded
		void AddToUpdateList(Vector2Int coords)
		{
			if (_chunks[coords].HasInitializedMesh)
				_chunks[coords].BuildMesh();
		}

		// Update adjacent chunks if block is at border
		if (relX == 0)
			AddToUpdateList(chunk.Coords + Vector2Int.left);
		else if (relX == Chunk.Width - 1)
			AddToUpdateList(chunk.Coords + Vector2Int.right);

		if (relZ == 0)
			AddToUpdateList(chunk.Coords + Vector2Int.down);
		else if (relZ == Chunk.Width - 1)
			AddToUpdateList(chunk.Coords + Vector2Int.up);
	}

	public Chunk GetChunkFor(int x, int z)
	{
		return GetChunk(
			Mathf.FloorToInt(x / (float)Chunk.Width),
			Mathf.FloorToInt(z / (float)Chunk.Width)
		);
	}

	public Chunk GetChunk(int x, int z)
	{
		if (_chunks.TryGetValue(new Vector2Int(x, z), out var chunk))
			return chunk;

		return null;
	}

	void LoadChunk(int x, int z)
	{
		Vector2Int coords = new Vector2Int(x, z);

		if (_chunks.ContainsKey(coords))
			return;

		_chunks.Add(coords, new Chunk(coords));
	}

	void OnDrawGizmos()
	{
		foreach (var kvp in _chunks)
		{
			Chunk chunk = kvp.Value;

			Gizmos.color = chunk.IsVisible ? Color.blue : Color.white;

			// Draw chunk boundaries
			Util.DrawBounds(
				new Bounds(
					new Vector3(chunk.Coords.x * Chunk.Width + Chunk.Width / 2f, Chunk.Height / 2f, chunk.Coords.y * Chunk.Width + Chunk.Width / 2f),
					new Vector3(Chunk.Width, Chunk.Height, Chunk.Width))
			);
		}
	}
}
