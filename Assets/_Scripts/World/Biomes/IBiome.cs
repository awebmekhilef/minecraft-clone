
/*
 * An alternate data driven biome architecture would be to have a Biome scriptable object
 * that takes an abstract TreeGenerator scriptable object as a parameter. 
 * Concrete classes can override MakeTree() functionality.
 * However.. the current system works fine so there's no need to change it.
 */

public interface IBiome
{
	public BlockID SurfaceBlock { get; }

	public BlockID SubSurfaceBlock { get; }

	int TreeProbability { get; }

	public void MakeTree(Chunk chunk, int x, int y, int z);
}

public class ForestBiome : IBiome
{
	public BlockID SurfaceBlock => BlockID.Grass;

	public BlockID SubSurfaceBlock => BlockID.Dirt;

	public int TreeProbability => 30;

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildOakTree(chunk, x, y, z);
	}
}

public class DesertBiome : IBiome
{
	public BlockID SurfaceBlock => BlockID.Sand;

	public BlockID SubSurfaceBlock => BlockID.Sand;

	public int TreeProbability => 175;

	public void MakeTree(Chunk chunk, int x, int y, int z)
	{
		TreeGenerator.BuildCactus(chunk, x, y, z);
	}
}
