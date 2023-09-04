using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDungeonPopup : UIBase
{
	[SerializeField] protected Image imageDungeon;
	[SerializeField] protected UITextMeshPro uiTextTitle;
	[SerializeField] protected Button buttonEnter;
	public Button ButtonEnter => buttonEnter;
	[SerializeField] protected Button buttonSweep;

	[SerializeField] protected UITextMeshPro uitextMostDamage;
	[SerializeField] protected Image imageTicket;
	[SerializeField] protected Image imageAdTicket;
	[SerializeField] protected TextMeshProUGUI textTicket;

	[SerializeField] protected ScrollRect scrollRect;
	[SerializeField] protected Transform content;
	[SerializeField] GameObject itemPrefab;

	protected UIManagementBattle parent;
	protected BattleData dungeonData;

	protected RuntimeData.StageInfo currentStageInfo;

	protected CurrencyType currencyType;

	protected List<RuntimeData.RewardInfo> _rewardList = new List<RuntimeData.RewardInfo>();
	protected List<RuntimeData.RewardInfo> _displayRewardList = new List<RuntimeData.RewardInfo>();
	protected virtual void Awake()
	{
		buttonEnter.onClick.RemoveAllListeners();
		buttonEnter.onClick.AddListener(OnClickEnter);
		buttonSweep.onClick.RemoveAllListeners();
		buttonSweep.onClick.AddListener(OnClickSweep);
	}

	public void Init(UIManagementBattle _parent, BattleData _dungeonData)
	{
		gameObject.SetActive(true);
		parent = _parent;
		dungeonData = _dungeonData;
		currentStageInfo = PlatformManager.UserDB.stageContainer.LastPlayedStage(dungeonData.stageType, dungeonData.tid);
		UpdateStageInfo();


	}

	protected virtual void ShowReward()
	{
		_rewardList.Clear();
		_displayRewardList.Clear();
		currentStageInfo.SetStageReward((IdleNumber)currentStageInfo.StageNumber - 1);

		var list = currentStageInfo.GetStageRewardList();
		_rewardList = new List<RuntimeData.RewardInfo>(list);
		_displayRewardList = new List<RuntimeData.RewardInfo>(currentStageInfo.StageClearReward);
		CreateList();
	}
	protected virtual void ShowKillReward()
	{
		_rewardList.Clear();
		_displayRewardList.Clear();
		var stageContainer = PlatformManager.UserDB.stageContainer;
		var record = stageContainer.GetStageRecordData(currentStageInfo);

		int killcount = 0;
		if (currentStageInfo.Rule is StageInfinity)
		{
			//killcount = 0;
			killcount = record != null ? record.killCount : 0;
		}

		currentStageInfo.SetReward((IdleNumber)1);

		if (currentStageInfo.MonsterExp != null)
		{
			_displayRewardList.Add(currentStageInfo.MonsterExp.Clone());
		}
		if (currentStageInfo.MonsterGold != null)
		{
			_displayRewardList.Add(currentStageInfo.MonsterGold.Clone());
		}
		_displayRewardList.AddRange(new List<RuntimeData.RewardInfo>(currentStageInfo.MonsterReward));


		currentStageInfo.SetReward((IdleNumber)killcount);
		if (currentStageInfo.MonsterExp != null)
		{
			_rewardList.Add(currentStageInfo.MonsterExp.Clone());
		}
		if (currentStageInfo.MonsterGold != null)
		{
			_rewardList.Add(currentStageInfo.MonsterGold.Clone());
		}
		_rewardList.AddRange(new List<RuntimeData.RewardInfo>(currentStageInfo.MonsterReward));

		CreateList();
	}

	protected void CreateList()
	{
		content.CreateListCell(_displayRewardList.Count, itemPrefab);

		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);


			if (i < _displayRewardList.Count)
			{
				if (_displayRewardList[i] == null)
				{
					continue;
				}
				child.gameObject.SetActive(true);
				UIItemReward uiItemReward = child.GetComponent<UIItemReward>();
				uiItemReward.Set(new AddItemInfo(_displayRewardList[i]));
			}
		}
	}
	protected virtual void UpdateStageInfo()
	{
		uiTextTitle.SetKey(currentStageInfo.Name);

		string damage = "0";

		var stageContainer = PlatformManager.UserDB.stageContainer;
		var record = stageContainer.GetStageRecordData(currentStageInfo);

		if (currentStageInfo.Rule is StageImmortal)
		{
			damage = record != null ? record.cumulativeDamage : "0";
			uitextMostDamage.SetKey("str_ui_best_damage").Append($" {damage}");
		}
		else if (currentStageInfo.Rule is StageInfinity)
		{
			int killcount = 0;

			killcount = record != null ? record.killCount : 0;

			uitextMostDamage.SetKey("str_ui_best_killcount").Append($" {killcount}");
		}
		else
		{
			uitextMostDamage.SetKey("");
			uitextMostDamage.TextmeshPro.text = "";
		}

		var scriptable = stageContainer.GetScriptableObject<DungeonItemObject>(dungeonData.tid);
		if (scriptable != null)
		{
			imageDungeon.sprite = scriptable.ItemIcon;
		}

		PlatformManager.UserDB.inventory.GetScriptableObject(dungeonData.dungeonItemTid, out CurrencyItemObject itemObject);

		if (itemObject != null)
		{
			currencyType = itemObject.currencyType;
			var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

			imageTicket.sprite = itemObject.ItemIcon;
			textTicket.text = $"{item.Value.ToString()}";
		}

		if (currentStageInfo.Rule is StageInfinity)
		{
			ShowKillReward();
		}
		else
		{

			ShowReward();
		}

	}

	public void OnClickSweep()
	{
		var currency = PlatformManager.UserDB.inventory.FindCurrency(dungeonData.dungeonItemTid);
		if (currency.Check((IdleNumber)1) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_sweep_ticket"]);
			return;
		}

		currency.Pay((IdleNumber)1);
		UpdateStageInfo();

		PlatformManager.UserDB.AddRewards(_rewardList, true);
		parent.OnUpdate();
	}

	public void OnClickEnter()
	{
		var currencyItem = PlatformManager.UserDB.inventory.FindCurrency(currencyType);

		IdleNumber needCount = (IdleNumber)1;
		if (currencyItem.Check(needCount) == false)
		{

			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_ticket"]);
			return;
		}

		StageManager.it.PlayStage(currentStageInfo);
		Close();
		parent.Close();
	}


	protected override void OnEnable()
	{
		base.OnEnable();
	}
	protected override void OnDisable()
	{
		base.OnDisable();


	}


}
