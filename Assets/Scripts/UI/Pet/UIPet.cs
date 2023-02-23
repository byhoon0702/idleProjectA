using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPet : MonoBehaviour, IUIClosable
{
	[SerializeField] private UIPetInfoPopup petInfoPopup;
	[SerializeField] private TextMeshProUGUI toOwnText;

	[Header("장착동료")]
	[SerializeField] private UIItemPetSlot[] petSlots;

	[Header("아이템리스트")]
	[SerializeField] private UIItemPetSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("Button")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button upgradeAllButton;




	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}
	private void OnEnable()
	{
		UIController.it.SetCoinEffectActivate(false);
	}

	private void OnDisable()
	{
		UIController.it.SetCoinEffectActivate(true);
		UserInfo.SaveUserData();
	}


	public void OnUpdate(bool _refreshGrid)
	{
		UpdateToOwn();
		UpdateEquipItem();
		UpdateItem(_refreshGrid);
		UpdateButton();
	}

	public void UpdateEquipItem()
	{
		for (int i = 0 ; i < UserInfo.pets.Length ; i++)
		{
			long itemTid = UserInfo.pets[i];
			if (itemTid == 0)
			{
				petSlots[i].gameObject.SetActive(false);
			}
			else
			{
				UIPetData uiData = new UIPetData();
				var result = uiData.Setup(itemTid);
				if(result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				petSlots[i].gameObject.SetActive(true);
				petSlots[i].OnUpdate(this, uiData);
			}
		}
	}

	public void UpdateItem(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemPetSlot>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Pet))
			{
				UIPetData uiData = new UIPetData();
				var result = uiData.Setup(v);
				if (result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemPetSlot>())
		{
			v.OnRefresh();
		}
	}

	private void UpdateButton()
	{
		upgradeAllButton.interactable = false;
	}

	private void UpdateToOwn()
	{
		toOwnText.text = "보유효과: ??";
	}

	private int GetEmptySlot()
	{
		for (int i = 0 ; i < UserInfo.pets.Length ; i++)
		{
			if (UserInfo.pets[i] == 0)
			{
				return i;
			}
		}

		return -1;
	}

	public void EquipPet(long _itemTid)
	{
		int emptySlotIndex = GetEmptySlot();

		if (emptySlotIndex == -1)
		{
			ToastUI.it.Create("비어있는 슬롯이 엄슴");
			return;
		}

		UserInfo.EquipPet(emptySlotIndex, _itemTid);

		UpdateEquipItem();
		UpdateItem(true);

		StageManager.it.ResetStage();
	}

	public void UnEquipPet(long _itemTid)
	{
		for (int i = 0 ; i < UserInfo.pets.Length ; i++)
		{
			if (UserInfo.pets[i] == _itemTid)
			{
				UserInfo.EquipPet(i, 0);
			}
		}

		UpdateEquipItem();
		UpdateItem(true);

		StageManager.it.ResetStage();
	}

	public void ShowItemInfo(UIPetData _uiData)
	{
		petInfoPopup.gameObject.SetActive(true);
		petInfoPopup.OnUpdate(this, _uiData);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
