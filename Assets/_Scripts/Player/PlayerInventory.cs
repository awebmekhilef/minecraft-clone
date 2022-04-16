using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] InventoryUI _hotbar;

	Inventory _inventory;

	void Start()
	{
		_inventory = new Inventory(8);
		_hotbar.Init(_inventory);

		_inventory.Add(BlockID.Dirt, 5);
		_inventory.Add(BlockID.Brick, 10);
		_inventory.Add(BlockID.Brick, 10);
		_inventory.Add(BlockID.Stone, 5);

		_inventory.Remove(BlockID.Brick, 20);
	}

	public BlockID PlaceBlock() {
		BlockID blockID = _inventory.Get(_hotbar.CurrentlySelectedSlot);
		_inventory.Remove(blockID, 1);

		return blockID;
	}

	public void AddBlock(BlockID blockID) {
		_inventory.Add(blockID, 1);
	}
}
