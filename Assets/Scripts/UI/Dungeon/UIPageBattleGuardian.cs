using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class UIPageBattleGuardian : UIPageBattle
{
	[SerializeField] private Button buttonNext;
	[SerializeField] private Button buttonPrev;

	//[SerializeField] private Button buttonPlay;
	[SerializeField] private UIEconomyButton buttonPlay;
	public UIEconomyButton ButtonPlay => buttonPlay;
	[SerializeField] private UIEconomyButton buttonSweep;

	[SerializeField] private Image imageStage;
	[SerializeField] private TextMeshProUGUI textTitle;

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	private List<RuntimeData.StageInfo> guardianList = new List<RuntimeData.StageInfo>();
	RuntimeData.StageInfo currentInfo;

	int currentIndex = 0;

	private void Awake()
	{
		//buttonNext.SetButtonEvent(OnClickNext);
		//buttonPrev.SetButtonEvent(OnClickPrev);
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
		bool isOpen = PlatformManager.UserDB.contentsContainer.TryEnter(_battleData.contentType);
		if (isOpen == false)
		{
			return false;
		}

		var currency = PlatformManager.UserDB.inventory.FindCurrency(_battleData.dungeonItemTid);
		if (currency.Check((IdleNumber)1) == false)
		{
			return false;
		}

		StageManager.it.PlayStage(currentInfo);
		parent.Close();
		return true;
	}

	private bool OnClickSweep()
	{
		var currency = PlatformManager.UserDB.inventory.FindCurrency(_battleData.dungeonItemTid);
		if (currency.Check((IdleNumber)1) == false)
		{
			return false;
		}

		currency.Pay((IdleNumber)1);
		PlatformManager.UserDB.AddRewards(currentInfo.StageClearReward, true);
		return true;
	}

	BattleData _battleData;

	public override void OnUpdate(UIManagementBattle _parent)
	{
		parent = _parent;
		guardianList = PlatformManager.UserDB.stageContainer.GetStageList(StageType.Guardian, 0);
		currentInfo = guardianList[currentIndex];

		_battleData = DataManager.Get<BattleDataSheet>().Get(currentInfo.stageData.dungeonTid);

		OnUpdateUI();
	}

	void OnUpdateUI()
	{
		textTitle.text = currentInfo.StageName;

		var scriptable = PlatformManager.UserDB.stageContainer.GetScriptableObject<DungeonItemObject>(_battleData.tid);
		if (scriptable != null)
		{
			imageStage.sprite = scriptable.ItemIcon;

		}


		PlatformManager.UserDB.inventory.GetScriptableObject(_battleData.dungeonItemTid, out CurrencyItemObject itemObject);
		if (itemObject != null)
		{
			var currencyType = itemObject.currencyType;
			var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

			buttonPlay.SetButton(itemObject.ItemIcon, $"{item.Value.ToString()}", item.Value > 0);
			buttonSweep.SetButton(itemObject.ItemIcon, $"{item.Value.ToString()}", item.Value > 0);
		}

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
