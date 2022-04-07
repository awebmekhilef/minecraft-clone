using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDatabase : Singleton<BlockDatabase>
{
	public Material ChunkMaterial;

	Dictionary<BlockID, BlockData> _blocks = new Dictionary<BlockID, BlockData>();

	protected override void Awake()
	{
		base.Awake();

		_blocks = Resources.LoadAll<BlockData>("Blocks").ToDictionary(b => b.ID, b => b); ;
	}

	public BlockData GetBlockData(BlockID blockID){
		return _blocks[blockID];
	}
}
