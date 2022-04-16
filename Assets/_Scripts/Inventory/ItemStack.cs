
public class ItemStack
{
	public BlockID BlockID { get; private set; }

	// TODO: Check stack 64?
	public int StackSize;

	public ItemStack(BlockID blockID, int amount)
	{
		BlockID = blockID;
		StackSize = amount;
	}
}
