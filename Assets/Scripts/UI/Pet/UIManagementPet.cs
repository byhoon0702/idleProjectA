using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public delegate void OnSelect(long tid);

public interface ISelectListener
{
	void AddSelectListener(OnSelect callback);
	void RemoveSelectListener(OnSelect callback);

}


public class UIManagementPet : MonoBehaviour, IUIClosable, ISelectListener
{
	[SerializeField] private UIManagementPetInfo petInfoUI;
	[SerializeField] private TextMeshProUGUI toOwnText;

	[Header("장착동료")]
	[SerializeField] private UIPetSlot[] petSlots;

	[Header("아이템리스트")]
	[SerializeField] private UIPetSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("Button")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button upgradeAllButton;

	public event OnSelect onSelect;


	private void Awake()
	{
		//closeButton.onClick.RemoveAllListeners();
		//closeButton.onClick.AddListener(Close);
	}
	private void OnEnable()
	{
		UIController.it.SetCoinEffectActivate(false);
	}

	private void OnDisable()
	{
		UIController.it.SetCoinEffectActivate(true);
	}


	public void OnUpdate(bool _refreshGrid)
	{
		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(_refreshGrid);
		UpdateButton();
		UpdateInfo();
	}
	public void AddSelectListener(OnSelect listener)
	{
		if (onSelect.IsRegistered(listener) == false)
		{
			onSelect += listener;
		}
	}
	public void RemoveSelectListener(OnSelect listener)
	{
		if (onSelect.IsRegistered(listener))
		{
			onSelect -= listener;
		}
	}

	public void UpdateEquipItem()
	{
		var petContainer = VGameManager.it.userDB.petContainer;
		for (int i = 0; i < petContainer.petSlot.Length; i++)
		{
			petSlots[i].gameObject.SetActive(true);
			petSlots[i].OnUpdate(this, petContainer.petSlot[i].item, () =>
			{
				UpdateInfo();
			});

		}
	}

	public long selectedItemTid;
	public void UpdateItemList(bool _refresh)
	{

		int countForMake = VGameManager.it.userDB.inventory.petList.Count - itemRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
			}
		}


		if (_refresh == false)
		{

			for (int i = 0; i < itemRoot.childCount; i++)
			{

				var child = itemRoot.GetChild(i);
				if (i >= VGameManager.it.userDB.inventory.petList.Count - 1)
				{
					child.gameObject.SetActive(false);
					continue;
				}

				child.gameObject.SetActive(true);
				UIPetSlot slot = child.GetComponent<UIPetSlot>();

				var info = VGameManager.it.userDB.inventory.petList[i];
				slot.OnUpdate(this, info, () =>
				{
					selectedItemTid = info.tid;
					UpdateInfo();
				});
				slot.ShowSlider(true);
			}
			return;
		}

		//foreach (var v in itemRoot.GetComponentsInChildren<UIPetSlot>())
		//{
		//	v.OnRefresh();
		//}
	}

	public void UpdateInfo()
	{

		RuntimeData.PetInfo info = null;
		info = VGameManager.UserDB.inventory.petList.Find(x => x.tid == selectedItemTid);
		if (info == null)
		{
			info = VGameManager.UserDB.inventory.petList[0];
			selectedItemTid = info.tid;
		}

		petInfoUI.OnUpdate(this, info);

		onSelect?.Invoke(selectedItemTid);
	}

	private void UpdateButton()
	{
		//upgradeAllButton.interactable = false;
	}

	private void UpdateToOwn()
	{
		//toOwnText.text = "보유효과: ??";
	}

	private int GetEmptySlot()
	{
		for (int i = 0; i < UserInfo.equip.pets.Length; i++)
		{
			if (UserInfo.equip.pets[i] == 0)
			{
				return i;
			}
		}

		return -1;
	}

	public void EquipPet()
	{
		VGameManager.UserDB.petContainer.Equip(selectedItemTid);

		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateButton();
		UpdateInfo();

		StageManager.it.RetryStage();
	}

	public void UnEquipPet()
	{
		VGameManager.UserDB.petContainer.Unequip(selectedItemTid);

		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateButton();
		UpdateInfo();
		StageManager.it.RetryStage();
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		UIController.it.InactivateAllBottomToggle();
		gameObject.SetActive(false);
	}
}
