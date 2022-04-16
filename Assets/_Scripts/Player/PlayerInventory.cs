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

		_inventory.Remove(BlockID.Brick, 10);
	}

	void Update()
	{
		if (Input.GetButtonDown("Place"))
			_inventory.Remove(_inventory.Get(_hotbar.CurrentlySelectedSlot), 1);
	}
}
