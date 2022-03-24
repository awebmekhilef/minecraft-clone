using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	[SerializeField] Material _chunkMaterial;
	[SerializeField] Transform _viewer;
	[SerializeField] BlockData[] _blocks;

	public static Vector2 ViewerPos;

	Dictionary<Vector2, Chunk> _chunks = new Dictionary<Vector2, Chunk>();
	List<Chunk> _chunksViewedLastFrame = new List<Chunk>();

	void Update()
	{
		ViewerPos = new Vector2(_viewer.position.x, _viewer.position.z);

		// Disable all previously viewed chunks so the updated ones can be reenabled below
		for (int i = 0; i < _chunksViewedLastFrame.Count; i++)
			_chunksViewedLastFrame[i].SetVisible(false);

		_chunksViewedLastFrame.Clear();

		int currChunkPosX = Mathf.RoundToInt(ViewerPos.x / VoxelData.ChunkWidth);
		int currChunkPosY = Mathf.RoundToInt(ViewerPos.y / VoxelData.ChunkWidth);

		for (int xOffset = -VoxelData.ChunkViewDst; xOffset <= VoxelData.ChunkViewDst; xOffset++)
		{
			for (int yOffset = -VoxelData.ChunkViewDst; yOffset <= VoxelData.ChunkViewDst; yOffset++)
			{
				Vector2 viewedChunkPos = new Vector2(currChunkPosX + xOffset, currChunkPosY + yOffset);

				if (_chunks.ContainsKey(viewedChunkPos))
				{
					Chunk chunk = _chunks[viewedChunkPos];

					chunk.UpdateViewDst();
					if (chunk.IsVisible())
						_chunksViewedLastFrame.Add(chunk);
				}
				else
					_chunks.Add(viewedChunkPos, new Chunk(viewedChunkPos, this, _chunkMaterial));
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(new Vector3(ViewerPos.x, 0f, ViewerPos.y), VoxelData.ChunkWidth * VoxelData.ChunkViewDst);
	}

	public BlockData GetBlock(int id)
	{
		return _blocks[id];
	}
}
