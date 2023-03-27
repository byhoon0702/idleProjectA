using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemDungeonDetailStandard : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dungeonNameText;
	[SerializeField] private TextMeshProUGUI bestCombatPowerText;
	[SerializeField] private UIItemBase[] uiItemRewards;
	[SerializeField] private Button selectButton;
	[SerializeField] private GameObject selectedObj;
	[SerializeField] private GameObject lockObj;


	private UIDungeonDetailStandard owner;
	private GameStageInfo stageInfo;



	private void Awake()
	{
		selectButton.onClick.RemoveAllListeners();
		selectButton.onClick.AddListener(OnSelectButtonClick);
	}

	public void OnUpdate(UIDungeonDetailStandard _owner, GameStageInfo _stageInfo)
	{
		owner = _owner;
		stageInfo = _stageInfo;
		dungeonNameText.text = $"{stageInfo.StageName} {stageInfo.StageSubTitle}";
		bestCombatPowerText.text = $"권장: {stageInfo.BestCombatPower.ToString()}";

		// UI에 표시될 주요보상
		var mainReward = _stageInfo.GetMainRewards();

		for(int i=0 ; i<uiItemRewards.Length ; i++)
		{
			if(mainReward.Count <= i)
			{
				uiItemRewards[i].gameObject.SetActive(false);
			}
			else
			{
				uiItemRewards[i].gameObject.SetActive(true);
				uiItemRewards[i].OnUpdate(mainReward[i]);
			}
		}

		bool isLock = UserInfo.stage.RecentStageLv(_stageInfo.WaveType) < stageInfo.StageLv;
		lockObj.gameObject.SetActive(isLock);
	}

	public void RefreshSelected()
	{
		selectedObj.SetActive(owner.ShowStageLv == stageInfo.StageLv);
	}

	private void OnSelectButtonClick()
	{
		owner.SelectStage(stageInfo);
	}
}
