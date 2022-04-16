using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
	[SerializeField] Image _icon;
	[SerializeField] TMP_Text _stack;

	Image _slotIcon;

	void Start()
	{
		_slotIcon = GetComponent<Image>();	
	}

	public void Set(ItemStack itemStack)
	{
		if (itemStack == null)
		{
			_icon.enabled = false;
			_stack.text = "";
			return;
		}

		_icon.enabled = true;

		_icon.sprite = BlockDatabase.Instance.GetBlockData(itemStack.BlockID).Icon;
		_stack.text = itemStack.StackSize.ToString();
	}

	public void Selected(bool selected)
	{
		_slotIcon.transform.localScale = selected ? Vector3.one * 1.25f : Vector3.one;
	}
}
