using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemStage : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textStageTitle;
	[SerializeField] private Button buttonMove;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private GameObject objLocked;
	[SerializeField] private GameObject objCleared;

	private RuntimeData.StageInfo info;
	private List<RuntimeData.RewardInfo> rewardInfos = new List<RuntimeData.RewardInfo>();

	private bool isLocked;
	private void Awake()
	{
		buttonMove.onClick.RemoveAllListeners();
		buttonMove.onClick.AddListener(OnClickMoveStage);
	}
	public void OnClickMoveStage()
	{
		if (isLocked)
		{
			return;
		}


		StageManager.it.PlayStage(info);
	}

	public void OnUpdate(RuntimeData.StageInfo _info)
	{
		info = _info;

		var lastPlay = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage();

		isLocked = true;

		isLocked = lastPlay.StageNumber < info.StageNumber;


#if UNITY_EDITOR
		isLocked = PlatformManager.ConfigMeta.CheckContent;
#endif


		objCleared.SetActive(info.isClear);
		objLocked.SetActive(isLocked);
		textStageTitle.text = $"STAGE {info.StageNumber} {info.StageName}";

		SetGrid();
	}

	private void SetGrid()
	{
		rewardInfos.Clear();
		for (int i = 0; i < info.stageData.monsterReward.Count; i++)
		{
			RuntimeData.RewardInfo reward = new RuntimeData.RewardInfo(info.stageData.monsterReward[i]);
			reward.UpdateCount(info.StageNumber - 1);
			rewardInfos.Add(reward);
		}
		content.CreateListCell(rewardInfos.Count, itemPrefab, UpdateGrid);
	}

	private void UpdateGrid()
	{
		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < rewardInfos.Count)
			{
				UIItemReward itemreward = child.GetComponent<UIItemReward>();
				itemreward.Set(new AddItemInfo(rewardInfos[i]));
				child.gameObject.SetActive(true);
			}
		}

	}
}
