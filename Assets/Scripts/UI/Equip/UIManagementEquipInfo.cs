using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIManagementEquipInfo : UIManagementBaseInfo<RuntimeData.EquipItemInfo>
{
	[SerializeField] private UIManagementEquip.EquipPage page;

	[SerializeField] private UIEquipSlot uiSlot;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private TextMeshProUGUI[] ownText;

	[SerializeField] private Button equipButton;
	public Button EquipButton => equipButton;
	[SerializeField] private Button levelupButton;
	public Button LevelupButton => levelupButton;
	[SerializeField] private Button upgradeButton;
	public Button UpgradeButton => upgradeButton;
	[SerializeField] private Button breakthroughButton;
	public Button BreakthroughButton => breakthroughButton;

	[SerializeField] private Button upgradeAllButton;

	private UIManagementEquip parent;
	public UIManagementEquip Parent => parent;
	private RuntimeData.EquipItemInfo equipInfo;
	public RuntimeData.EquipItemInfo SelectedInfo => equipInfo;

	private void Awake()
	{
		equipButton.SetButtonEvent(OnEquipButtonClick);
		levelupButton.SetButtonEvent(OnClickShowLevelUp);
		upgradeButton.SetButtonEvent(OnClickShowUpgrade);
		breakthroughButton.SetButtonEvent(OnClickBreakThrough);
		upgradeAllButton.SetButtonEvent(OnUpgradeAllButtonClick);
	}

	public override void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo _info)
	{
		parent = _parent;
		equipInfo = _info;
		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateButton();
	}

	public void UpdateItemInfo()
	{
		uiSlot.OnUpdate(parent, equipInfo, null);
		nameText.text = PlatformManager.Language[equipInfo.rawData.name];

	}

	public void UpdateItemLevelupInfo()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < equipInfo.equipAbilities.Count; i++)
		{
			string tail = equipInfo.equipAbilities[i].tailChar;
			sb.Append($"{equipInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{equipInfo.equipAbilities[i].GetValue(equipInfo.Level).ToString()}{tail}</color>");
			sb.Append('\n');
		}
		equipText.text = $"{sb.ToString()}";




		for (int i = 0; i < ownText.Length; i++)
		{
			sb.Clear();
			ownText[i].text = "";
			if (i < equipInfo.ownedAbilities.Count)
			{
				string tail = equipInfo.ownedAbilities[i].tailChar;
				sb.Append($"{equipInfo.ownedAbilities[i].type.ToUIString()}");
				sb.Append($" <color=yellow>{equipInfo.ownedAbilities[i].GetValue(equipInfo.Level).ToString()}{tail}</color>");
				sb.Append('\n');

				ownText[i].text = $"{sb.ToString()}";
			}
		}


	}
	public void UpdateButton()
	{
		if (equipInfo != null)
		{
			bool buttonActive = equipInfo.unlock;
			bool equipped = equipInfo.Tid != PlatformManager.UserDB.equipContainer.GetSlot(equipInfo.rawData.equipType).itemTid;
			bool levelupable = ItemLevelupable();

			equipButton.interactable = buttonActive && equipped;
			levelupButton.gameObject.SetActive(levelupable);
			breakthroughButton.gameObject.SetActive(buttonActive && equipInfo.Level > 0 && equipInfo.IsMaxLevel());
			upgradeButton.gameObject.SetActive(buttonActive && equipInfo.isLastItem == false);
		}
		else
		{
			equipButton.interactable = false;
		}

		//upgradeAllButton.interactable = false;
	}
	public bool ItemLevelupable()
	{


		if (equipInfo == null && equipInfo.Count <= 0)
		{
			return false;
		}
		if (equipInfo.IsMaxLevel())
		{
			return false;
		}

		return true;
	}


	/// <summary>
	/// 실제 착용정보가 바뀜
	/// </summary>
	private void OnEquipButtonClick()
	{
		PlatformManager.UserDB.equipContainer.GetSlot(parent.equipType).Equip(equipInfo);
		//parent.UpdateItems(true);

		UpdateItemInfo();
		UpdateItemLevelupInfo();
		UpdateButton();
		parent.OnUpdateEquip(parent.equipType, equipInfo.Tid);
		PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EQUIP_ITEM, equipInfo.Tid, (IdleNumber)1);
	}

	public void OnClickShowLevelUp()
	{
		parent.UiPopupEquipLevelup.OnUpdate(parent, equipInfo);
	}

	public void OnClickShowUpgrade()
	{
		parent.UiPopupEquipUpgrade.OnUpdate(parent, equipInfo);
	}

	public void OnClickBreakThrough()
	{
		parent.UiPopupEquipBreakthrough.OnUpdate(parent, equipInfo);
	}

	private void OnUpgradeAllButtonClick()
	{
		PlatformManager.UserDB.equipContainer.UpgradeAll(parent.equipType);
		parent.OnUpdateEquip(parent.equipType, equipInfo.Tid);

	}
}
