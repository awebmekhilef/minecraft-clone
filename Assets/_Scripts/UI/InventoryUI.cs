using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] ItemSlotUI _itemSlotPrefab;

	Inventory _inventory;
	ItemSlotUI[] _itemSlotUIs;

	int _currIndex = 0;
	public int CurrentlySelectedSlot => _currIndex;

	public void Init(Inventory inventory)
	{
		_inventory = inventory;

		_itemSlotUIs = new ItemSlotUI[_inventory.Size];

		for (int i = 0; i < _inventory.Size; i++)
			_itemSlotUIs[i] = Instantiate(_itemSlotPrefab, transform);

		_inventory.OnItemsUpdated += OnItemsUpdated;
	}

	void Update()
	{
		_currIndex -= System.Math.Sign(Input.GetAxis("Mouse ScrollWheel"));

		if (_currIndex > _inventory.Size - 1)
			_currIndex = 0;
		else if (_currIndex < 0)
			_currIndex = _inventory.Size - 1;

		for (int i = 0; i < _itemSlotUIs.Length; i++)
			_itemSlotUIs[i].Selected(i == _currIndex);
	}

	private void OnItemsUpdated(ItemStack[] items)
	{
		for (int i = 0; i < _itemSlotUIs.Length; i++)
			_itemSlotUIs[i].Set(items[i]);
	}
}
