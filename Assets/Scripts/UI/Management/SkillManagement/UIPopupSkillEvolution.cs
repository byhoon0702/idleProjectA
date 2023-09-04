using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
public class UIPopupSkillEvolution : UIBase
{
	[SerializeField] private UISkillSlot uiCurrentSlot;
	[SerializeField] private UISkillSlot uiNextSlot;
	[SerializeField]
	private GameObject objArrow;

	[SerializeField] private Button buttonClose;
	[SerializeField] private UIEconomyButton buttonUpgrade;
	public UIEconomyButton ButtonUpgrade => buttonUpgrade;
	[SerializeField] private Button buttonMax;

	[SerializeField] private TextMeshProUGUI textCurrentInfo;
	[SerializeField] private TextMeshProUGUI textNextInfo;

	private RuntimeData.SkillInfo itemInfo;
	private RuntimeData.SkillInfo nextItemInfo;
	private UIManagementSkill parent;

	private void Awake()
	{
		buttonClose.SetButtonEvent(OnClose);
		buttonUpgrade.onlyOnce = true;
		buttonUpgrade.SetEvent(OnClickEvolution);
	}

	public void OnUpdate(UIManagementSkill _parent, RuntimeData.SkillInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;

		itemInfo = info;

		Refresh();
	}

	public void Refresh()
	{
		uiCurrentSlot.OnUpdate(null, itemInfo);

		if (itemInfo.EvolutionLevel == itemInfo.EvolutionMaxLevel)
		{
			objArrow.SetActive(false);
			uiNextSlot.gameObject.SetActive(false);
			buttonUpgrade.gameObject.SetActive(false);
			buttonMax.gameObject.SetActive(true);
			return;
		}

		objArrow.SetActive(true);
		nextItemInfo = itemInfo.Clone();
		nextItemInfo.Evoltion(false);

		uiNextSlot.OnUpdate(null, nextItemInfo);
		uiNextSlot.gameObject.SetActive(true);
		itemInfo.MakeCompareDescritption(nextItemInfo, out string currentDesc, out string nextDesc);
		textCurrentInfo.text = currentDesc;
		textNextInfo.text = nextDesc;

		bool isMaxEvolution = itemInfo.EvolutionLevel >= PlatformManager.CommonData.SkillEvolutionNeedsList.Count;

		if (isMaxEvolution == false)
		{
			var data = PlatformManager.CommonData.SkillEvolutionNeedsList[itemInfo.EvolutionLevel];

			var currency = PlatformManager.UserDB.inventory.FindCurrency(data.type);

			buttonUpgrade.SetButton(currency.IconImage, $"{currency.Value.ToString()}/{data.count}", currency.Value >= data.count);
			buttonUpgrade.gameObject.SetActive(true);
			buttonMax.gameObject.SetActive(false);
		}
	}

	public bool OnClickEvolution()
	{
		if (PlatformManager.UserDB.skillContainer.SkillEvolution(itemInfo) == false)
		{
			return false;
		}

		parent.OnUpdate(itemInfo.Tid);
		OnClose();
		return true;
	}
}
