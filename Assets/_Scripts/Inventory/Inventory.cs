using System;

public class Inventory
{
	ItemStack[] _items = new ItemStack[8];

	public event Action<ItemStack[]> OnItemsUpdated;

	public void Add(BlockID blockID, int amount)
	{
		for (int i = 0; i < _items.Length; i++)
		{
			ItemStack itemStack = _items[i];

			if (itemStack != null && itemStack.BlockID == blockID)
			{
				itemStack.StackSize += amount;
				OnItemsUpdated?.Invoke(_items);

				return;
			}
		}

		for (int i = 0; i < _items.Length; i++)
		{
			if (_items[i] == null)
			{
				_items[i] = new ItemStack(blockID, amount);
				OnItemsUpdated?.Invoke(_items);

				return;
			}
		}
	}

	public void Remove(BlockID blockID, int amount)
	{
		for (int i = 0; i < _items.Length; i++)
		{
			ItemStack itemStack = _items[i];

			if (itemStack != null && itemStack.BlockID == blockID)
			{
				itemStack.StackSize -= amount;
				if (itemStack.StackSize < 1)
					_items[i] = null;

				OnItemsUpdated?.Invoke(_items);

				return;
			}
		}
	}
}
