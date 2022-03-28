using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	[SerializeField] Transform _viewer;

	Dictionary<Vector2, Chunk> _chunks = new Dictionary<Vector2, Chunk>();
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
				Vector2 viewedChunkPos = new Vector2(xOffset + viewerPosX, zOffset + viewerPosZ);

				if (_chunks.ContainsKey(viewedChunkPos))
				{
					Chunk viewedChunk = _chunks[viewedChunkPos];

					viewedChunk.IsVisible = Vector2.Distance(viewedChunk.Coords, new Vector2(viewerPosX, viewerPosZ)) <= Chunk.MaxViewDst;

					if (viewedChunk.IsVisible)
						_chunksViewedLastFrame.Add(viewedChunk);
				}
				else
					_chunks.Add(viewedChunkPos, new Chunk(viewedChunkPos));
			}
		}
	}

	void OnDrawGizmos()
	{
		foreach (var kvp in _chunks)
		{
			Chunk chunk = kvp.Value;

			Gizmos.color = chunk.IsVisible ? Color.blue : Color.white;
			Extensions.DrawBounds(new Bounds(chunk.Center, Chunk.Volume));
		}
	}
}
