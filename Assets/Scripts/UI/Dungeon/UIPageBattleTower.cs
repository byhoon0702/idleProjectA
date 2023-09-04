using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPageBattleTower : UIPageBattle, ISelectListener
{
	[SerializeField] private TextMeshProUGUI textRewardLabel;
	[SerializeField] private Button buttonPlay;
	public Button ButtonPlay => buttonPlay;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private Transform contentReward;
	[SerializeField] private GameObject itemPrefabReward;

	private List<RuntimeData.StageInfo> stageList;
	public RuntimeData.StageInfo currentInfo;
	event OnSelect onSelect;

	public void AddSelectListener(OnSelect callback)
	{
		onSelect += callback;
	}
	public void RemoveSelectListener(OnSelect callback)
	{
		onSelect -= callback;
	}

	public void SetSelectedTid(long stageNumber)
	{
		currentInfo = stageList.Find(x => x.StageNumber == stageNumber);
		SetReward();
		if (stageNumber == lastStage.stageNumber + 1)
		{
			buttonPlay.interactable = true;
		}
		else
		{
			buttonPlay.interactable = false;
		}

		onSelect?.Invoke(stageNumber);
	}

	void Awake()
	{
		buttonPlay.SetButtonEvent(OnClickPlay);
	}
	public override void OnUpdate(UIManagementBattle _parent)
	{
		parent = _parent;


		SetFloorGrid();
		SetReward();
	}

	public void OnClickPlay()
	{
		if (currentInfo.isClear)
		{
			return;
		}

		if (currentInfo.StageNumber - 1 != lastStage.stageNumber)
		{
			return;
		}

		StageManager.it.PlayStage(currentInfo);

		GameUIManager.it.Close();//uiController.InactiveAllMainUI();
	}
	StageRecordData lastStage;
	private void SetFloorGrid()
	{
		lastStage = PlatformManager.UserDB.stageContainer.GetLastStage(StageType.Tower, 0);
		stageList = PlatformManager.UserDB.stageContainer.GetStageList(StageType.Tower, 0);

		int index = Mathf.Max(0, lastStage.stageNumber);
		currentInfo = stageList[index];
		content.CreateListCell(stageList.Count, itemPrefab);

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < stageList.Count)
			{
				UIItemTowerFloor towerFloor = child.GetComponent<UIItemTowerFloor>();
				towerFloor.OnUpdate(this, stageList[i]);
				towerFloor.OnSelect(currentInfo.StageNumber);
				towerFloor.gameObject.SetActive(true);
			}
		}
		SetReward();
	}

	private void SetReward()
	{
		if (currentInfo == null)
		{
			return;
		}

		textRewardLabel.text = $"{currentInfo.StageNumber}층\n클리어보상";
		currentInfo.SetStageReward((IdleNumber)currentInfo.StageNumber);
		int count = currentInfo.StageClearReward != null ? currentInfo.StageClearReward.Count : 0;

		contentReward.CreateListCell(count, itemPrefabReward);
		for (int i = 0; i < contentReward.childCount; i++)
		{
			Transform child = contentReward.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < count)
			{
				UIItemReward reward = child.GetComponent<UIItemReward>();
				reward.Set(new AddItemInfo(currentInfo.StageClearReward[i]));

				child.gameObject.SetActive(true);
			}
		}

	}
}
