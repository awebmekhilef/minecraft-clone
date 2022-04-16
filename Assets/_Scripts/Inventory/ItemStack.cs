using UnityEngine;

public class ItemStack
{
	public BlockID BlockID { get; private set; }

	public int StackSize;

	public ItemStack(BlockID blockID, int amount)
	{
		BlockID = blockID;
		StackSize = amount;
	}
}
