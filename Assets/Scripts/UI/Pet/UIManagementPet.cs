using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public delegate void OnSelect(long tid);

public interface ISelectListener
{
	void SetSelectedTid(long tid);
	void AddSelectListener(OnSelect callback);
	void RemoveSelectListener(OnSelect callback);

}


public class UIManagementPet : MonoBehaviour, IUIClosable, ISelectListener
{
	[SerializeField] private UIManagementPetInfo petInfoUI;
	[SerializeField] private TextMeshProUGUI toOwnText;

	[Header("장착동료")]
	[SerializeField] private GameObject[] petSlotHighlights;
	[SerializeField] private UIPetSlot[] petSlots;

	[Header("아이템리스트")]
	[SerializeField] private UIPetSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("Button")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button upgradeAllButton;

	public event OnSelect onSelect;

	void OnEnable()
	{
		AddCloseListener();
	}
	void OnDisable()
	{
		RemoveCloseListener();
	}
	public void AddCloseListener()
	{
		GameUIManager.it.onClose += Close;
	}

	public void RemoveCloseListener()
	{
		GameUIManager.it.onClose -= Close;
	}
	private void Awake()
	{
		//closeButton.onClick.RemoveAllListeners();
		//closeButton.onClick.AddListener(Close);
	}
	//private void OnEnable()
	//{
	//	UIController.it.SetCoinEffectActivate(false);
	//}

	//private void OnDisable()
	//{
	//	UIController.it.SetCoinEffectActivate(true);
	//}


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

	public void SetSelectedTid(long tid)
	{
		if (exchangeSlot)
		{
			return;
		}

		selectedItemTid = tid;
	}

	public void UpdateEquipItem()
	{
		var petContainer = GameManager.UserDB.petContainer;
		for (int i = 0; i < petContainer.PetSlots.Length; i++)
		{

			var slot = petSlots[i];
			var slotData = petContainer.PetSlots[i];
			slot.gameObject.SetActive(true);
			slot.OnUpdate(this, slotData.item, () =>
			{
				if (exchangeSlot)
				{
					ExchangePet(slotData);
					return;
				}
				UpdateInfo();
			});
		}
	}

	public long selectedItemTid
	{ get; private set; }
	public void UpdateItemList(bool _refresh)
	{

		int countForMake = GameManager.UserDB.petContainer.petList.Count - itemRoot.childCount;

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
				if (i >= GameManager.UserDB.petContainer.petList.Count - 1)
				{
					child.gameObject.SetActive(false);
					continue;
				}

				child.gameObject.SetActive(true);
				UIPetSlot slot = child.GetComponent<UIPetSlot>();

				var info = GameManager.UserDB.petContainer.petList[i];
				slot.OnUpdate(this, info, () =>
				{
					selectedItemTid = info.tid;
					UpdateInfo();
				});
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
		exchangeSlot = false;
		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}
		RuntimeData.PetInfo info = null;
		info = GameManager.UserDB.petContainer.petList.Find(x => x.tid == selectedItemTid);
		if (info == null)
		{
			info = GameManager.UserDB.petContainer.petList[0];
			selectedItemTid = info.tid;
		}

		// petInfoUI.OnUpdate(this, info);

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



	public bool exchangeSlot;

	public void ExchangePet(PetSlot slot)
	{
		GameManager.UserDB.petContainer.Unequip(slot.itemTid);
		GameManager.UserDB.petContainer.Equip(selectedItemTid);

		for (int i = 0; i < petSlotHighlights.Length; i++)
		{
			petSlotHighlights[i].SetActive(false);
		}
		exchangeSlot = false;

		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateButton();
		UpdateInfo();

		SpawnManager.it.ChangePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		//StageManager.it.RetryStage();
	}
	public void EquipPet()
	{
		bool equipped = GameManager.UserDB.petContainer.Equip(selectedItemTid);

		if (equipped == false)
		{
			exchangeSlot = true;
			for (int i = 0; i < petSlotHighlights.Length; i++)
			{
				petSlotHighlights[i].SetActive(true);
			}
			return;
		}

		SpawnManager.it.AddPet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateButton();
		UpdateInfo();


	}

	public void UnEquipPet()
	{
		SpawnManager.it.RemovePet(GameManager.UserDB.petContainer.GetIndex(selectedItemTid));
		GameManager.UserDB.petContainer.Unequip(selectedItemTid);

		UpdateToOwn();
		UpdateEquipItem();
		UpdateItemList(false);
		UpdateButton();
		UpdateInfo();

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
