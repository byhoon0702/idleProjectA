﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPageEventDungeon : MonoBehaviour
{
	[SerializeField] private UIEconomyButton buttonPlay;
	public UIEconomyButton ButtonPlay => buttonPlay;
	[SerializeField] private UIEconomyButton buttonSweep;

	[SerializeField] private Image imageStage;
	[SerializeField] private TextMeshProUGUI textTitle;

	[SerializeField] private Image imageCurrency;
	[SerializeField] private TextMeshProUGUI textCurrency;

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private Button nextButton;
	[SerializeField] private Button prevButton;

	private List<RuntimeData.StageInfo> guardianList = new List<RuntimeData.StageInfo>();
	private RuntimeData.StageInfo currentInfo;
	private UIPopupEventPage _parent;
	int currentIndex = 0;

	private void Awake()
	{
		buttonPlay.SetButtonEvent(OnClickPlay);
		buttonSweep.SetButtonEvent(OnClickSweep);
	}

	private void OnClickNext()
	{
		if (currentIndex >= guardianList.Count - 1)
		{
			return;
		}
		currentIndex++;
		currentInfo = guardianList[currentIndex];
		OnUpdateUI();

	}

	private void OnClickPrev()
	{
		if (currentIndex == 0)
		{
			return;
		}
		currentIndex--;

		currentInfo = guardianList[currentIndex];
		OnUpdateUI();
	}

	private bool OnClickPlay()
	{
		//bool isOpen = PlatformManager.UserDB.contentsContainer.TryEnter(_battleData.contentType);
		//if (isOpen == false)
		//{
		//	return false;
		//}

		var currency = PlatformManager.UserDB.inventory.FindCurrency(_battleData.dungeonItemTid);
		if (currency.Check((IdleNumber)1) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_ticket"]);
			return false;
		}

		StageManager.it.PlayStage(currentInfo);

		return true;
	}

	private bool OnClickSweep()
	{
		if (currentInfo.isClear == false)
		{
			ToastUI.Instance.EnqueueKey("str_ui_need_stage_clear");
			return false;
		}
		var currency = PlatformManager.UserDB.inventory.FindCurrency(_battleData.dungeonItemTid);
		if (currency.Check((IdleNumber)1) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_sweep_ticket"]);
			return false;
		}

		currency.Pay((IdleNumber)1);
		PlatformManager.UserDB.AddRewards(currentInfo.StageClearReward, true);
		OnUpdateUI();
		return true;
	}

	BattleData _battleData;

	public void OnUpdate(UIPopupEventPage parent)
	{
		_parent = parent;

		var info = PlatformManager.UserDB.eventContainer.GetCurrentEvent();

		guardianList = info.stageList;
		currentInfo = guardianList[currentIndex];

		_battleData = DataManager.Get<EventBattleDataSheet>().Get(currentInfo.stageData.dungeonTid);

		OnUpdateUI();
	}

	void OnUpdateUI()
	{
		textTitle.text = $"{currentInfo.StageName} {currentInfo.StageNumber}";

		var scriptable = PlatformManager.UserDB.eventContainer.GetScriptableObject<DungeonItemObject>(_battleData.tid);
		if (scriptable != null)
		{
			imageStage.sprite = scriptable.ItemIcon;

		}

		PlatformManager.UserDB.inventory.GetScriptableObject(_battleData.dungeonItemTid, out CurrencyItemObject itemObject);
		if (itemObject != null)
		{
			var currencyType = itemObject.currencyType;
			var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

			imageCurrency.sprite = itemObject.ItemIcon;
			textCurrency.text = item.Value.ToString();
			buttonPlay.SetButton(itemObject.ItemIcon, $"1", item.Value > 0);
			buttonSweep.SetButton(itemObject.ItemIcon, $"1", item.Value > 0);
		}


		nextButton.gameObject.SetActive(currentIndex < guardianList.Count - 1);
		prevButton.gameObject.SetActive(currentIndex > 0);

		buttonSweep.interactable = currentInfo.isClear;
		SetGrid();
	}

	private void SetGrid()
	{
		currentInfo.SetStageReward((IdleNumber)0);
		int count = currentInfo.StageClearReward != null ? currentInfo.StageClearReward.Count : 0;
		content.CreateListCell(count, itemPrefab);
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < count)
			{
				child.gameObject.SetActive(true);
				UIItemReward reward = child.GetComponent<UIItemReward>();
				reward.Set(new AddItemInfo(currentInfo.StageClearReward[i]));
				reward.ShowChance(true);
			}
		}
	}
}
