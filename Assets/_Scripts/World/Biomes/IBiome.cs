
public interface IBiome
{
	public BlockID GetSurfaceBlock();
}

public class ForestBiome : IBiome
{
	public BlockID GetSurfaceBlock()
	{
		return BlockID.Grass;
	}
}

public class DesertBiome : IBiome
{
	public BlockID GetSurfaceBlock()
	{
		return BlockID.Sand;
	}
}
