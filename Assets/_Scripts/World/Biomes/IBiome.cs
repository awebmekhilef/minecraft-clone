
public interface IBiome
{
	public BlockID GetSurfaceBlock();
	int GetTreeProbability();
	int GetTerrainHeight();

	public void MakeTree(Chunk chunk, int x, int y, int z);
}

public class ForestBiome : IBiome
{
	public BlockID GetSurfaceBlock()
	{
		return BlockID.Grass;
	}

	public int GetTerrainHeight()
	{
		return 16;
	}

	public int GetTreeProbability()
	{
		return 30;
	}

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildOakTree(chunk, x, y, z);
	}
}

public class DesertBiome : IBiome
{
	public BlockID GetSurfaceBlock()
	{
		return BlockID.Sand;
	}

	public int GetTerrainHeight()
	{
		return 8;
	}

	public int GetTreeProbability()
	{
		return 175;
	}

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildCactus(chunk, x, y, z);
	}
}
