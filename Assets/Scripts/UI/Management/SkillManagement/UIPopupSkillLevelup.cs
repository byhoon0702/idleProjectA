using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupSkillLevelup : UIBase
{
	[SerializeField] private UISkillSlot uiSkillSlot;


	[SerializeField] Image imageButtonCurrency;
	[SerializeField] protected TextMeshProUGUI textMeshButtonCurrency;

	[SerializeField] protected Button buttonMax;
	[SerializeField] protected UIEconomyButton buttonUpgrade;
	public UIEconomyButton ButtonUpgrade => buttonUpgrade;

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textSkill;
	[SerializeField] protected TextMeshProUGUI textNextSkill;


	private RuntimeData.SkillInfo itemInfo;
	private UIManagementSkill parent;
	protected virtual void Awake()
	{
		buttonUpgrade.SetButtonEvent(OnClickLevelUp);
	}


	public void OnUpdate(UIManagementSkill _parent, RuntimeData.SkillInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;
		itemInfo = info;
		textMeshProName.text = PlatformManager.Language[itemInfo.rawData.name];
		OnUpdateInfo();

	}

	public void OnUpdateInfo()
	{
		uiSkillSlot.OnUpdate(null, itemInfo, null);
		UpdateItemLevelupInfo();

		int value = itemInfo.LevelUpNeedCount();

		buttonUpgrade.SetButton(itemInfo.IconImage, $"{itemInfo.Count}/{value.ToString()}", itemInfo.Count >= value);

		buttonMax.gameObject.SetActive(itemInfo.IsMax());
		buttonUpgrade.gameObject.SetActive(itemInfo.IsMax() == false);
	}

	public void UpdateItemLevelupInfo()
	{
		RuntimeData.SkillInfo nextInfo = itemInfo.Clone();
		nextInfo.SetLevel(nextInfo.Level + 1);
		itemInfo.MakeCompareDescritption(nextInfo, out string currentDesc, out string nextDesc);

		textSkill.text = currentDesc;
		textNextSkill.text = nextDesc;

	}

	public bool OnClickLevelUp()
	{
		if (ItemLevelupable() == false)
		{
			return false;
		}

		itemInfo.LevelUp();
		parent.OnUpdate(itemInfo.Tid);
		OnUpdateInfo();
		return true;
	}

	public bool ItemLevelupable()
	{
		if (itemInfo.unlock == false)
		{
			return false;
		}
		if (itemInfo.CanLevelUp(out string message) == false)
		{
			ToastUI.Instance.Enqueue(message);
			return false;
		}

		return true;
	}
}
