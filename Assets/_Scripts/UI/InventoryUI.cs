using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	Inventory _inventory;

	ItemSlotUI[] _itemSlotUIs;

	void Awake()
	{
		_itemSlotUIs = GetComponentsInChildren<ItemSlotUI>();
	}

	public void Init(Inventory inventory)
	{
		_inventory = inventory;

		_inventory.OnItemsUpdated += OnItemsUpdated;
	}

	private void OnItemsUpdated(ItemStack[] items)
	{
		for (int i = 0; i < _itemSlotUIs.Length; i++)
			_itemSlotUIs[i].Set(items[i]);
	}
}
