using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;



public class UIManagementSkill : UIBase, ISelectListener
{

	[SerializeField] private UIManagementSkillInfo uiManagementSkillInfo;
	public UIManagementSkillInfo UiManagementSkillInfo => uiManagementSkillInfo;


	[Header("장착스킬")]
	[SerializeField] private GameObject objSkillSlotRoot;
	public GameObject ObjSkillSlotRoot => objSkillSlotRoot;
	[SerializeField] private GameObject[] uiSkillSlotHighlights;
	[SerializeField] private UISkillSlot[] uiSkillSlots;
	public UISkillSlot[] UiSkillSlots => uiSkillSlots;

	[Header("아이템리스트")]
	[SerializeField] private UISkillSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;
	[Header("팝업")]
	[SerializeField] private UIPopupSkillLevelup uiPopupSkillLevelup;
	public UIPopupSkillLevelup UiPopupSkillLevelup => uiPopupSkillLevelup;
	[SerializeField] private UIPopupSkillEvolution uiPopupSkillEvolution;
	public UIPopupSkillEvolution UiPopupSkillEvolution => uiPopupSkillEvolution;


	public event OnSelect onSelect;
	public long selectedItemTid { get; private set; }
	private RuntimeData.SkillInfo selectedInfo;
	private bool exchangeSlot;
	private List<RuntimeData.SkillInfo> filtered;

	public Transform Find(int index)
	{
		return itemRoot.GetChild(index);
	}
	public void OnUpdate(long tid)
	{
		objSkillSlotRoot.SetActive(false);
		filtered = new List<RuntimeData.SkillInfo>();
		var list = PlatformManager.UserDB.skillContainer.skillList;

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].HideInUI)
			{
				continue;
			}

			filtered.Add(list[i]);
		}

		selectedItemTid = tid;
		if (selectedItemTid == 0)
		{
			selectedItemTid = DefaultSelectTid();
		}

		UpdateInfo();
		UpdateEquipItem();
		UpdateItem();
		UpdateButton();

	}
	private long DefaultSelectTid()
	{
		long tid = filtered[0].Tid;
		return tid;
	}

	public void ShowHighlight(bool onoff)
	{
		for (int i = 0; i < uiSkillSlotHighlights.Length; i++)
		{
			uiSkillSlotHighlights[i].SetActive(onoff);
		}

	}

	public void UpdateEquipItem()
	{

		var skillContainer = PlatformManager.UserDB.skillContainer;
		for (int i = 0; i < skillContainer.skillSlot.Length; i++)
		{
			var slot = uiSkillSlots[i];
			var slotData = skillContainer[i];
			slot.gameObject.SetActive(true);
			slot.OnUpdate(null, slotData.item, () =>
			{
				ExchangeSkill(slotData);
				UpdateInfo();
				objSkillSlotRoot.SetActive(false);
			});
		}
	}

	public void UpdateItem()
	{

		int countForMake = filtered.Count - itemRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
			}
		}

		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (i > filtered.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UISkillSlot slot = child.GetComponent<UISkillSlot>();

			var info = filtered[i];

			slot.OnUpdate(this, info, () =>
			{
				selectedItemTid = info.Tid;

				UpdateInfo();
			});
			slot.ShowSlider(true);
		}
	}

	private void UpdateButton()
	{
		//summonButton.interactable = false;
		//upgradeButton.interactable = false;
	}

	public void UpdateInfo()
	{
		exchangeSlot = false;
		ShowHighlight(false);
		selectedInfo = filtered.Find(x => x.Tid == selectedItemTid);
		uiManagementSkillInfo.OnUpdate(this, selectedInfo);
		onSelect?.Invoke(selectedItemTid);
	}

	public void ExchangeSkill(SkillSlot slotData)
	{
		slotData.Unequip();
		slotData.Equip(UnitManager.it.Player, selectedItemTid);

		UpdateInfo();
		UpdateEquipItem();
		UpdateItem();
		UpdateButton();
		UIController.it.SkillGlobal.OnUpdate();
	}


	public void EquipSkill()
	{
		objSkillSlotRoot.SetActive(true);
		exchangeSlot = true;
		//int emptySlotIndex = GetEmptySlot();

		//if (emptySlotIndex == -1)
		//{
		//	exchangeSlot = true;
		//	ShowHighlight(true);
		//	return;
		//}

		//VPlatformManager.UserDB.skillContainer.Equip(UnitManager.it.Player, selectedItemTid);
		//UpdateEquipItem();
		//UpdateItem();
		//UpdateButton();
		//UpdateInfo();

		//UnitGlobal.it.playerSkillModule.InitSkill(UserInfo.equip.skills);
		//UnitGlobal.it.playerSkillModule.SetUnit(UnitManager.it.Player);
		//UIController.it.SkillGlobal.OnUpdate();
	}

	public void UnEquipSkill()
	{

		PlatformManager.UserDB.skillContainer.UnEquip(selectedItemTid);

		UpdateEquipItem();
		UpdateItem();
		UpdateButton();
		UpdateInfo();
		UIController.it.SkillGlobal.OnUpdate();
	}

	public void SetSelectedTid(long tid)
	{
		if (exchangeSlot)
		{
			return;
		}
		selectedItemTid = tid;
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

	public void OnClickShowTree()
	{

	}

	public void OnClickLevelUp()
	{
		if (uiPopupSkillLevelup.Activate())
		{
			uiPopupSkillLevelup.OnUpdate(this, selectedInfo);
		}
	}
	public void OnClickEvolution()
	{
		if (uiPopupSkillEvolution.Activate())
		{
			uiPopupSkillEvolution.OnUpdate(this, selectedInfo);
		}
	}

	protected override void OnClose()
	{
		if (objSkillSlotRoot.activeInHierarchy)
		{
			objSkillSlotRoot.SetActive(true);
			return;
		}
		base.OnClose();
	}
}
