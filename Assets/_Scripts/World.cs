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
				Vector2Int viewedChunkPos = new Vector2Int(xOffset + viewerPosX, zOffset + viewerPosZ);

				if (_chunks.ContainsKey(viewedChunkPos))
				{
					Chunk viewedChunk = _chunks[viewedChunkPos];

					viewedChunk.IsVisible = true;
					_chunksViewedLastFrame.Add(viewedChunk);

					// If chunk was just added build its mesh
					if (!viewedChunk.HasBuiltMesh)
						viewedChunk.BuildMesh();
				}
				else
					_chunks.Add(viewedChunkPos, new Chunk(viewedChunkPos));
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

	Chunk GetChunk(int x, int z)
	{
		if (_chunks.TryGetValue(new Vector2Int(x, z), out var chunk))
			return chunk;

		return null;
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
