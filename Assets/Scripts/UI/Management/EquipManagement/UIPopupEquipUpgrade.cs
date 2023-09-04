using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipUpgrade : UIBase
{


	[SerializeField] private UIEquipSlot currentItemSlot;
	[SerializeField] private UIEquipSlot nextItemSlot;

	[SerializeField] private Button buttonUpgrade;
	public Button ButtonUpgrade => buttonUpgrade;

	[SerializeField] private Button buttonMinus;
	[SerializeField] private Button buttonPlus;
	[SerializeField] private TextMeshProUGUI textUpgradeCount;

	[SerializeField] private TextMeshProUGUI textCurrentCount;
	[SerializeField] private TextMeshProUGUI textNextCount;


	private UIManagementEquip parent;
	private RuntimeData.EquipItemInfo itemInfo;
	private RuntimeData.EquipItemInfo nextItemInfo;

	private int upgradeCount;
	private bool canUpgrade = false;

	private void Awake()
	{
		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickUpgrade);

		buttonMinus.onClick.RemoveAllListeners();
		buttonMinus.onClick.AddListener(OnClickMinus);
		buttonPlus.onClick.RemoveAllListeners();
		buttonPlus.onClick.AddListener(OnClickPlus);
	}
	public void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo info)
	{
		Activate();

		parent = _parent;

		itemInfo = info;

		nextItemInfo = PlatformManager.UserDB.equipContainer.FindNextEquipItem(itemInfo);

		if (nextItemInfo == null || nextItemInfo.Tid == itemInfo.Tid)
		{
			//다음 아이템이 없음
			return;
		}

		upgradeCount = itemInfo.Count / EquipContainer.needCount;
		canUpgrade = upgradeCount > 0;
		UpdateCount();

		currentItemSlot.OnUpdate(null, itemInfo);
		nextItemSlot.OnUpdate(null, nextItemInfo);
	}

	public void OnClickUpgrade()
	{
		if (nextItemInfo == null)
		{
			return;
		}
		if (itemInfo.Count < 1)
		{
			return;
		}
		if (upgradeCount == 0)
		{
			return;
		}

		PlatformManager.UserDB.equipContainer.UpgradeEquipItem(ref itemInfo, ref nextItemInfo, upgradeCount);
		PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.UPGRADE_WEAPON, 0, (IdleNumber)1);
		currentItemSlot.OnUpdate(null, itemInfo);
		nextItemSlot.OnUpdate(null, nextItemInfo);

		parent.OnUpdateEquip(itemInfo.type, itemInfo.Tid);
		OnClose();
	}

	public void UpdateCount()
	{
		textCurrentCount.text = $"<color=red>-{upgradeCount * EquipContainer.needCount}</color>";
		textNextCount.text = $"<color=green>+{upgradeCount}</color>";
		textUpgradeCount.text = $"{upgradeCount}";
	}
	public void OnClickMinus()
	{
		int temp = Mathf.Max(0, upgradeCount - 1);
		int result = temp * EquipContainer.needCount;
		if (itemInfo.Count < result)
		{
			canUpgrade = false;
		}
		else
		{
			canUpgrade = true;

		}
		upgradeCount = temp;
		UpdateCount();
	}

	public void OnClickPlus()
	{
		int temp = Mathf.Min(itemInfo.Count / EquipContainer.needCount, upgradeCount + 1);

		int result = temp * EquipContainer.needCount;

		if (itemInfo.Count < result)
		{
			canUpgrade = false;
		}
		else
		{
			canUpgrade = true;
		}

		upgradeCount = temp;
		UpdateCount();
	}

}
