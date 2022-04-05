using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World>
{
	[SerializeField] Transform _viewer;

	Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	List<Chunk> _chunksViewedLastFrame = new List<Chunk>();

	void Update()
	{
		int viewerPosX = Mathf.RoundToInt(_viewer.position.x / Chunk.Width);
		int viewerPosZ = Mathf.RoundToInt(_viewer.position.z / Chunk.Width);

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
					if (!viewedChunk.HasBuiltMesh)
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

	public BlockId GetBlock(int x, int y, int z)
	{
		Chunk chunk = GetChunkFor(x, z);

		if (chunk == null)
			return BlockId.Air;

		return chunk.GetBlock(chunk.ToRelativeX(x), y, chunk.ToRelativeZ(z));
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
					new Vector3(chunk.Coords.x * Chunk.Width, Chunk.Height / 2f, chunk.Coords.y * Chunk.Width),
					new Vector3(Chunk.Width, Chunk.Height, Chunk.Width))
			);
		}
	}
}
