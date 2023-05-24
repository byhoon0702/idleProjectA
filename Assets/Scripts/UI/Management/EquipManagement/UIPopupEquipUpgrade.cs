using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipUpgrade : MonoBehaviour
{


	[SerializeField] private UIEquipSlot currentItemSlot;
	[SerializeField] private UIEquipSlot nextItemSlot;

	[SerializeField] private Button buttonClose;
	[SerializeField] private Button buttonUpgrade;

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
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClose);
		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickUpgrade);

		buttonMinus.onClick.RemoveAllListeners();
		buttonMinus.onClick.AddListener(OnClickMinus);
		buttonPlus.onClick.RemoveAllListeners();
		buttonPlus.onClick.AddListener(OnClickPlus);
	}
	public void OnUpdate(UIManagementEquip _parent, RuntimeData.EquipItemInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;

		itemInfo = info;

		nextItemInfo = GameManager.UserDB.equipContainer.FindNextEquipItem(itemInfo);

		if (nextItemInfo == null || nextItemInfo.tid == itemInfo.tid)
		{
			//다음 아이템이 없음
			return;
		}

		upgradeCount = itemInfo.count / EquipContainer.needCount;
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
		if (itemInfo.count < 1)
		{
			return;
		}
		if (upgradeCount == 0)
		{
			return;
		}

		GameManager.UserDB.equipContainer.UpgradeEquipItem(ref itemInfo, ref nextItemInfo, upgradeCount);

		currentItemSlot.OnUpdate(null, itemInfo);
		nextItemSlot.OnUpdate(null, nextItemInfo);

		parent.OnUpdateEquip(itemInfo.type, itemInfo.tid);
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
		if (itemInfo.count < result)
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
		int temp = Mathf.Min(itemInfo.count / EquipContainer.needCount, upgradeCount + 1);

		int result = temp * EquipContainer.needCount;

		if (itemInfo.count < result)
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


	public void OnClose()
	{
		gameObject.SetActive(false);
	}
}
