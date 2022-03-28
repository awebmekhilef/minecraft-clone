using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	[SerializeField] Transform _viewer;

	Dictionary<Vector2, Chunk> _chunks = new Dictionary<Vector2, Chunk>();
	List<Chunk> _chunksViewedLastFrame = new List<Chunk>();

	void Update()
	{
		Vector2 viewerPos = new Vector2((int)(_viewer.position.x / Chunk.Width), (int)(_viewer.position.z / Chunk.Width));

		for (int i = 0; i < _chunksViewedLastFrame.Count; i++)
			_chunksViewedLastFrame[i].IsVisible = false;

		_chunksViewedLastFrame.Clear();

		for (int xOffset = -Chunk.MaxViewDst; xOffset <= Chunk.MaxViewDst; xOffset++)
		{
			for (int zOffset = -Chunk.MaxViewDst; zOffset <= Chunk.MaxViewDst; zOffset++)
			{
				Vector2 viewedChunkPos = new Vector2(xOffset + viewerPos.x, zOffset + viewerPos.y);

				if (_chunks.ContainsKey(viewedChunkPos))
				{
					Chunk viewedChunk = _chunks[viewedChunkPos];

					viewedChunk.IsVisible = Vector2.Distance(viewedChunk.Coords, viewerPos) <= Chunk.MaxViewDst;

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

		foreach (var chunk in _chunks)
		{
			Gizmos.color = chunk.Value.IsVisible ? Color.blue : Color.white;
			Gizmos.DrawSphere(new Vector3(chunk.Key.x * Chunk.Width, Chunk.Height / 2f, chunk.Key.y * Chunk.Width), 0.5f);
		}
	}
}
