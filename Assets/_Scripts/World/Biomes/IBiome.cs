
public interface IBiome
{
	public BlockID SurfaceBlock { get; }

	int TreeProbability { get; }

	int TerrainHeight { get; }

	public void MakeTree(Chunk chunk, int x, int y, int z);
}

public class ForestBiome : IBiome
{
	public BlockID SurfaceBlock => BlockID.Grass;

	public int TerrainHeight => 16;

	public int TreeProbability => 30;

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildOakTree(chunk, x, y, z);
	}
}

public class DesertBiome : IBiome
{
	public BlockID SurfaceBlock => BlockID.Sand;

	public int TerrainHeight => 8;

	public int TreeProbability => 175;

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildCactus(chunk, x, y, z);
	}
}
