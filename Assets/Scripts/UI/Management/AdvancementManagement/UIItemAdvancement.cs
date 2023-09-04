using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using System;

public class UIItemAdvancement : MonoBehaviour
{
	[SerializeField] private UIItemReward _itemReward;
	[SerializeField] private Transform pivot;
	[SerializeField] private Image _imageFrame;
	[SerializeField] private UIStatsInfoCell[] statInfos;
	[SerializeField] private TextMeshProUGUI textCondition;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private UIEconomyButton buttonActivate;
	public UIEconomyButton ButtonActivate => buttonActivate;
	[SerializeField] private GameObject _objectComplete;

	private UnitAnimation unitCostume;
	private RuntimeData.AdvancementInfo info;
	private Action onclick;

	private UIManagementAdvancement parent;
	private bool _conditinFulfilled;
	private string _conditonString;

	private void Awake()
	{
		buttonActivate.onlyOnce = true;

		buttonActivate.SetButtonEvent(OnClickActivate);

	}
	public void OnUpdate(UIManagementAdvancement _parent, RuntimeData.AdvancementInfo _info)
	{
		parent = _parent;
		info = _info;

		var data = DataManager.Get<CostumeDataSheet>().Get(info.rawData.costumeTid);
		_imageFrame.sprite = parent.ImageGrade[(int)data.itemGrade];

		for (int i = 0; i < statInfos.Length; i++)
		{
			if (i < info.AbilityList.Count)
			{
				statInfos[i].gameObject.SetActive(true);
				statInfos[i].OnUpdate(info.AbilityList[i].type.ToUIString(), $"<color=green>+{info.AbilityList[i].Value.ToString()}%</color>");
			}
			else
			{
				statInfos[i].gameObject.SetActive(false);
			}
		}

		textTitle.text = PlatformManager.Language[info.rawData.name];
		IdleNumber cost = (IdleNumber)info.rawData.currencyValue;
		if (info.Currency != null)
		{
			var item = PlatformManager.UserDB.inventory.FindCurrency(info.Currency.currencyType);
			buttonActivate.SetButton(info.Currency.ItemIcon, $"{item.Value.ToString()}/ {cost.ToString()}", item.Value >= cost);
		}
		CreateUnitForUI();
		UpdateInfo();

		_itemReward.Set(new AddItemInfo(data.tid, (IdleNumber)1, RewardCategory.Costume));
	}

	public void UpdateInfo()
	{
		_conditonString = "";
		int stageNumber = info.rawData.normalStageNumber;

		_conditinFulfilled = PlatformManager.UserDB.stageContainer.GetNormalStage(stageNumber).isClear;

		_conditonString = $"{PlatformManager.Language["str_ui_stage_youth_condition"]} STAGE {stageNumber} 클리어";
		textCondition.text = _conditonString;

		if (_conditinFulfilled)
		{
			textCondition.color = Color.green;
		}
		else
		{
			textCondition.color = Color.red;
		}



		bool iscompleted = PlatformManager.UserDB.advancementContainer.IsCompletedPreAdvancement(info);


		_objectComplete.SetActive(info.GotAdvancement);
		buttonActivate.gameObject.SetActive(!info.GotAdvancement);
		buttonActivate.SetInteractable(iscompleted && _conditinFulfilled);
	}

	public void CreateUnitForUI()
	{
		if (info.Costume == null || info.Costume.CostumeObject == null)
		{
			return;
		}

		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}

		var obj = Instantiate(info.Costume.CostumeObject) as GameObject;
		obj.transform.SetParent(pivot);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		unitCostume = obj.GetComponent<UnitAnimation>();
		unitCostume.Init();
		unitCostume.SetMaskInteraction(SpriteMaskInteraction.VisibleInsideMask);
		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	public bool OnClickActivate()
	{
		if (_conditinFulfilled == false)
		{
			ToastUI.Instance.Enqueue(_conditonString);
			return false;
		}

		bool gotMoney = PlatformManager.UserDB.inventory.FindCurrency(info.Currency.currencyType).Check((IdleNumber)info.rawData.currencyValue);
		if (gotMoney == false)
		{
			ToastUI.Instance.Enqueue("재화 부족");
			return false;
		}

		var stageInfo = PlatformManager.UserDB.stageContainer.GetStage(info.rawData.battleTid, info.rawData.stageNumber);
		StageManager.it.PlayStage(stageInfo);

		parent.Refresh();

		GameUIManager.it.Close();
		return true;
	}
}
