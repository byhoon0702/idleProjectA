using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemMap : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI subTitleText;
	[SerializeField] private UIItemBase[] uiItemRewards;
	[SerializeField] private Button selectButton;
	[SerializeField] private Button moveButton;
	[SerializeField] private GameObject selectedObj;
	[SerializeField] private GameObject lockObj;


	private UIMap owner;
	private GameStageInfo stageInfo;



	private void Awake()
	{
		selectButton.onClick.RemoveAllListeners();
		selectButton.onClick.AddListener(OnSelectButtonClick);

		moveButton.onClick.RemoveAllListeners();
		moveButton.onClick.AddListener(OnMoveButtonClick);
	}

	public void OnUpdate(UIMap _owner, GameStageInfo _stageInfo)
	{
		owner = _owner;
		stageInfo = _stageInfo;
		subTitleText.text = _stageInfo.StageSubTitle;

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

		bool isLock = UserInfo.stage.RecentStageLv(WaveType.Normal) < stageInfo.StageLv;
		lockObj.gameObject.SetActive(isLock);
		moveButton.gameObject.SetActive(isLock == false);
	}

	public void RefreshSelected()
	{
		selectedObj.SetActive(owner.ShowStageLv == stageInfo.StageLv);
	}

	private void OnSelectButtonClick()
	{
		owner.SelectStage(stageInfo.StageLv);
	}

	private void OnMoveButtonClick()
	{
		UserInfo.stage.SetPlayingStageLv(WaveType.Normal, stageInfo.StageLv);
		StageManager.it.PlayStage(stageInfo);

		owner.Close();
	}
}
