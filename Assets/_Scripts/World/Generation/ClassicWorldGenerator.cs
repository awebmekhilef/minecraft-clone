using UnityEngine;

public class ClassicWorldGenerator : IWorldGenerator
{
	public const int GroundHeight = 16;

	public void GenerateChunk(Chunk chunk)
	{
		// Generate noise maps
		Noise.Init(4, 2f, 0.5f);
		float[,] noiseMap = Noise.Generate(Chunk.Width, Chunk.Width, 1f, new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));

		Noise.Init(4, 2f, 0.5f);
		float[,] biomeMap = Noise.Generate(Chunk.Width, Chunk.Width, 2f, new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));

		// Fill in ground blocks
		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				IBiome biome = GetBiome(biomeMap[x, z]);

				// Generate world space elevation
				int maxHeight = (int)(GroundHeight + noiseMap[x, z] * biome.GetTerrainHeight()) - 1;

				// Set blocks
				chunk.SetBlock(x, maxHeight, z, biome.GetSurfaceBlock());
				chunk.SetBlock(x, 0, z, BlockID.Bedrock);

				for (int y = 1; y < maxHeight; y++)
				{
					if (y > maxHeight - 4)
						chunk.SetBlock(x, y, z, BlockID.Dirt);
					else
						chunk.SetBlock(x, y, z, BlockID.Stone);
				}

				// Create trees
				if (Random.Range(0, biome.GetTreeProbability()) == 0)
				{
					biome.MakeTree(chunk, x, maxHeight, z);
				}
			}
		}
	}

	public IBiome GetBiome(float value)
	{
		if (value > 0.5f)
			return new ForestBiome();
		else
			return new DesertBiome();
	}
}
