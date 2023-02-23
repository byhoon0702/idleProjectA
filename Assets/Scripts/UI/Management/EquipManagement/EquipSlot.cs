using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSlot : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI levelText;

	private UIManagementMain owner;
	private ItemType itemType;
	private long selectedItemTid;


	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIManagementMain _owner, ItemType _itemType, long _selectedItemTid)
	{
		owner = _owner;
		itemType = _itemType;
		selectedItemTid = _selectedItemTid;

		icon.gameObject.SetActive(false);
		levelText.gameObject.SetActive(false);


		if (selectedItemTid != 0)
		{
			icon.gameObject.SetActive(true);
			levelText.gameObject.SetActive(true);

			var itemData = DataManager.Get<ItemDataSheet>().Get(_selectedItemTid);
			icon.sprite = Resources.Load<Sprite>($"Icon/{itemData.Icon}");
			levelText.text = $"Lv. {Inventory.it.FindItemByTid(_selectedItemTid).Level}";
		}
	}

	private void OnClick()
	{
		owner.ShowEquipUi(itemType);
	}
}
