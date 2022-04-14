using UnityEngine;

public class ClassicWorldGenerator : IWorldGenerator
{
	// TODO: Need to be moved to biome data
	public const int GroundHeight = 16;
	public const int TerrainHeight = 16;
		
	public const int TreeDensity = 30;

	public void GenerateChunk(Chunk chunk)
	{
		// Generate noise maps
		Noise.Init(4, 2f, 0.5f);
		float[,] noiseMap = Noise.Generate(Chunk.Width, Chunk.Width, 1f, new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));

		Noise.Init(4, 2f, 0.5f);
		float[,] biomeMap = Noise.Generate(Chunk.Width, Chunk.Width, 2f, new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));

		int[,] elevations = new int[Chunk.Width, Chunk.Width];

		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				// Calculate world space heights based on noise map
				elevations[x, z] = (int)(GroundHeight + noiseMap[x, z] * TerrainHeight);
			}
		}

		// Fill in ground blocks
		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				int maxHeight = elevations[x, z] - 1;

				IBiome biome = GetBiome(biomeMap[x, z]);

				chunk.SetBlock(x, maxHeight, z, biome.GetSurfaceBlock());
				chunk.SetBlock(x, 0, z, BlockID.Bedrock);

				for (int y = 1; y < maxHeight; y++)
				{
					if (y > maxHeight - 4)
						chunk.SetBlock(x, y, z, BlockID.Dirt);
					else
						chunk.SetBlock(x, y, z, BlockID.Stone);
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
