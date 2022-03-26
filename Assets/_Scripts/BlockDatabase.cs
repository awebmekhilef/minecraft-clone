using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDatabase : Singleton<BlockDatabase>
{
	public Material ChunkMaterial;

	Dictionary<BlockId, BlockData> _blocks = new Dictionary<BlockId, BlockData>();

	protected override void Awake()
	{
		base.Awake();

		_blocks = Resources.LoadAll<BlockData>("Blocks").ToDictionary(b => b.Id, b => b); ;
	}

	public BlockData GetBlockData(BlockId blockId){
		return _blocks[blockId];
	}
}
