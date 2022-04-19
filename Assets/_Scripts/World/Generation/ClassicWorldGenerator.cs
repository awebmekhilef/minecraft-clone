using UnityEngine;

public class ClassicWorldGenerator : IWorldGenerator
{
	public const int GroundHeight = 16;

	// TODO: Should be different for each biome
	public const int TerrainHeight = 16;

	public void GenerateChunk(Chunk chunk)
	{
		// Generate noise maps
		float[,] noiseMap = Noise.Generate(Chunk.Width, Chunk.Width,
										   1f, 4, 2f, 0.5f,
										   new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));
		float[,] biomeMap = Noise.Generate(Chunk.Width, Chunk.Width,
										   2f, 4, 2f, 0.5f,
										   new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));

		// Fill in ground blocks
		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				IBiome biome = GetBiome(biomeMap[x, z]);

				// Generate world space elevation
				int maxHeight = (int)(GroundHeight + noiseMap[x, z] * TerrainHeight) - 1;

				// Set blocks
				chunk.SetBlock(x, maxHeight, z, biome.SurfaceBlock);
				chunk.SetBlock(x, 0, z, BlockID.Bedrock);

				for (int y = 1; y < maxHeight; y++)
				{
					if (y > maxHeight - 4)
						chunk.SetBlock(x, y, z, biome.SubSurfaceBlock);
					else
						chunk.SetBlock(x, y, z, BlockID.Stone);
				}

				// Create trees
				if (Random.Range(0, biome.TreeProbability) == 0)
					biome.MakeTree(chunk, x, maxHeight, z);
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
