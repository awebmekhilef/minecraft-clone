using UnityEngine;

public class ClassicWorldGenerator : IWorldGenerator
{
	// TODO: Need to be moved to biome classes
	public const int GroundHeight = 16;
	public const int TerrainHeight = 16;

	public const int TreeDensity = 30;

	public void GenerateChunk(Chunk chunk)
	{
		// Generate elevations
		float[,] noiseMap = Noise.Generate(Chunk.Width, Chunk.Width, 25f, new Vector2(chunk.Coords.x * Chunk.Width, chunk.Coords.y * Chunk.Width));
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

				chunk.SetBlock(x, maxHeight, z, BlockID.Grass);
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

		// Generate trees
		for (int x = 0; x < Chunk.Width; x++)
		{
			for (int z = 0; z < Chunk.Width; z++)
			{
				if (Random.Range(0, TreeDensity) == 0)
				{
					TreeGenerator.BuildTree(chunk, x, elevations[x, z], z);
				}
			}
		}
	}
}
