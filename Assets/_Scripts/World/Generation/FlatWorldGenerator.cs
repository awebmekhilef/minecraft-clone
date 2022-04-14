using UnityEngine;

public class FlatWorldGenerator : IWorldGenerator
{
	public void GenerateChunk(Chunk chunk)
	{
		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				chunk.SetBlock(x, 0, z, BlockID.Bedrock);
				chunk.SetBlock(x, 1, z, BlockID.Dirt);
				chunk.SetBlock(x, 2, z, BlockID.Dirt);
				chunk.SetBlock(x, 3, z, BlockID.Grass);
			}
		}
	}
}
