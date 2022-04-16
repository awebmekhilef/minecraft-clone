using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	[SerializeField] InventoryUI _hotbar;

	Inventory _inventory;

	void Start()
	{
		_inventory = new Inventory(8);
		_hotbar.Init(_inventory);

		_inventory.Add(BlockID.Dirt, 64);
		_inventory.Add(BlockID.Brick, 64);
		_inventory.Add(BlockID.Glass, 64);
		_inventory.Add(BlockID.Cobblestone, 64);
		_inventory.Add(BlockID.WoodLog, 64);
		_inventory.Add(BlockID.WoodPlank, 64);
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
