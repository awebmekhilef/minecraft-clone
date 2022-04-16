using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
	[SerializeField] Image _icon;
	[SerializeField] TMP_Text _stack;

	public void Set(ItemStack itemStack) {
		if(itemStack == null) {
			_icon.enabled = false;
			_stack.text = "";
			return;
		}

		_icon.enabled = true;

		_icon.sprite = BlockDatabase.Instance.GetBlockData(itemStack.BlockID).Icon;
		_stack.text = itemStack.StackSize.ToString();
	}
}
