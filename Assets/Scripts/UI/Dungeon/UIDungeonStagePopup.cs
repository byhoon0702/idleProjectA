using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIDungeonStagePopup : UIDungeonPopup
{
	[SerializeField] private Button buttonPrevStage;
	[SerializeField] private Button buttonNextStage;
	[SerializeField] protected TextMeshProUGUI textStageNumber;
	[SerializeField] protected TextMeshProUGUI textRecord;

	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		buttonPrevStage.onClick.RemoveAllListeners();
		buttonPrevStage.onClick.AddListener(OnClickPrevStage);
		buttonNextStage.onClick.RemoveAllListeners();
		buttonNextStage.onClick.AddListener(OnClickNextStage);
	}

	public void OnClickNextStage()
	{
		var stageInfo = PlatformManager.UserDB.stageContainer.GetStage(currentStageInfo.stageData.dungeonTid, currentStageInfo.StageNumber + 1);
		if (stageInfo == null)
		{
			return;
		}

		currentStageInfo = stageInfo;
		UpdateStageInfo();
	}

	public void OnClickPrevStage()
	{
		var stageInfo = PlatformManager.UserDB.stageContainer.GetStage(currentStageInfo.stageData.dungeonTid, currentStageInfo.StageNumber - 1);
		if (stageInfo == null)
		{
			return;
		}

		currentStageInfo = stageInfo;
		UpdateStageInfo();
	}
	protected override void UpdateStageInfo()
	{
		var stageContainer = PlatformManager.UserDB.stageContainer;
		uiTextTitle.SetKey(currentStageInfo.Name);



		var record = stageContainer.GetStageRecordData(currentStageInfo);
		textRecord.text = record != null ? record.cumulativeDamage : "0";

		var scriptable = stageContainer.GetScriptableObject<DungeonItemObject>(dungeonData.tid);
		if (scriptable != null)
		{
			imageDungeon.sprite = scriptable.ItemIcon;
		}

		textStageNumber.text = currentStageInfo.StageNumber.ToString();


		PlatformManager.UserDB.inventory.GetScriptableObject(dungeonData.dungeonItemTid, out CurrencyItemObject itemObject);
		if (itemObject != null)
		{
			currencyType = itemObject.currencyType;
			var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

			imageTicket.sprite = itemObject.ItemIcon;
			textTicket.text = $"{item.Value.ToString()}";
		}

		bool canPlay = true;
		var lastStage = stageContainer.GetStage(currentStageInfo.stageData.dungeonTid, currentStageInfo.StageNumber - 1);
		if (lastStage != null)
		{
			canPlay = lastStage.isClear;
		}

		buttonEnter.interactable = canPlay;
		buttonSweep.interactable = currentStageInfo.isClear;

		if (currentStageInfo.Rule is StageInfinity)
		{
			ShowKillReward();
		}
		else
		{

			ShowReward();
		}

	}
}
