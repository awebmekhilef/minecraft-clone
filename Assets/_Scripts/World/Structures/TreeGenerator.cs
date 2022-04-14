using UnityEngine;

public static class TreeGenerator
{
	public static void BuildOakTree(Chunk chunk, int x, int y, int z)
	{
		int height = Random.Range(4, 7);

		// Make leaves
		for (int xx = -2; xx <= 2; xx++)
		{
			for (int zz = -2; zz <= 2; zz++)
			{
				chunk.SetBlock(x + xx, height + y - 1, z + zz, BlockID.Leaves);
				chunk.SetBlock(x + xx, height + y, z + zz, BlockID.Leaves);

				if (Mathf.Abs(xx) < 2 && Mathf.Abs(zz) < 2)
				{
					chunk.SetBlock(x + xx, height + y + 1, z + zz, BlockID.Leaves);

					if (xx == 0 || zz == 0)
						chunk.SetBlock(x + xx, height + y + 2, z + zz, BlockID.Leaves);
				}
			}
		}

		// Make trunk
		for (int i = 0; i < height; i++)
			chunk.SetBlock(x, y + i, z, BlockID.WoodLog);
	}

	public static void BuildCactus(Chunk chunk, int x, int y, int z) {
		int height = Random.Range(3, 5);

		// Make trunk
		for (int i = 0; i < height; i++)
			chunk.SetBlock(x, y + i, z, BlockID.Cactus);
	}
}
