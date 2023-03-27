using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class UIMap : MonoBehaviour, IUIClosable
{
	[Header("스테이지 리스트")]
	[SerializeField] private UIItemMap prefab;
	[SerializeField] private RectTransform stageListLayout;
	[SerializeField] private Button easyButton;
	[SerializeField] private Button normalButton;
	[SerializeField] private Button hardButton;
	[SerializeField] private Button nightmareButton;

	[Header("타이틀")]
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private Image bgImage;
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;
	[SerializeField] private Button closeButton;

	[Header("보상리스트")]
	[SerializeField] private UIItemBase[] uiItemRewardList;
	[SerializeField] private TextMeshProUGUI[] probabilityText;


	private StageDifficulty showDifficult;
	private int showAreaIndex;

	
	/// <summary>
	/// 선택된 스테이지
	/// </summary>
	private int showStageLevel;
	public int ShowStageLv => showStageLevel;

	private List<GameStageInfo> showStageInfos = new List<GameStageInfo>();
	private List<UIItemMap> uiStageInfos = new List<UIItemMap>();



	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(() => 
		{
			if(Closable())
			{
				Close();
			}
		});


		easyButton.onClick.RemoveAllListeners();
		easyButton.onClick.AddListener(() => OnDifficultChange(StageDifficulty.Easy));
		normalButton.onClick.RemoveAllListeners();
		normalButton.onClick.AddListener(() => OnDifficultChange(StageDifficulty.Normal));
		hardButton.onClick.RemoveAllListeners();
		hardButton.onClick.AddListener(() => OnDifficultChange(StageDifficulty.Hard));
		nightmareButton.onClick.RemoveAllListeners();
		nightmareButton.onClick.AddListener(() => OnDifficultChange(StageDifficulty.Nightmare));


		leftButton.onClick.RemoveAllListeners();
		leftButton.onClick.AddListener(OnLeftButtonClick);
		rightButton.onClick.RemoveAllListeners();
		rightButton.onClick.AddListener(OnRightButtonClick);
	}

	public void OnUpdate(int _stageLevel)
	{
		var stageInfo = StageManager.it.metaGameStage.GetStage(WaveType.Normal, _stageLevel);

		OnUpdate(stageInfo.Difficult, stageInfo.AreaIndex, _stageLevel);
	}

	/// <summary>
	/// stageIndex가 0이면 마지막 스테이지로 자동셋팅됨
	/// </summary>
	public void OnUpdate(StageDifficulty _difficult, int _areaIndex, int _stageLevel = 0)
	{
		showDifficult = _difficult;
		showAreaIndex = _areaIndex;
		showStageLevel = _stageLevel;

		showStageInfos = StageManager.it.metaGameStage.GetStages(WaveType.Normal, showDifficult, showAreaIndex);

		if(showStageLevel == 0)
		{
			// 스테이지 레벨이 지정되지 않았을경우 열려있는 마지막 스테이지의 정보를 가져옴
			showStageLevel = showStageInfos[0].StageLv; // (아예 못찾으면 첫번째꺼 사용)
			for (int i=showStageInfos.Count - 1 ; i >= 0 ; i--)
			{
				if (showStageInfos[i].IsStageOpend())
				{
					showStageLevel = showStageInfos[i].StageLv;
					break;
				}
			}
		}
			 
		UpdateStageList();
		UpdateTitle();
		UpdateRewards();
	}

	private void UpdateStageList()
	{
		foreach (var uiItem in uiStageInfos)
		{
			Destroy(uiItem.gameObject);
		}
		uiStageInfos.Clear();


		for (int i = 0 ; i < showStageInfos.Count ; i++)
		{
			UIItemMap uiItem = Instantiate(prefab, stageListLayout);
			uiStageInfos.Add(uiItem);

			uiItem.OnUpdate(this, showStageInfos[i]);
			uiItem.RefreshSelected();

		}
	}

	private void UpdateTitle()
	{
		foreach (var stageInfo in showStageInfos)
		{
			if (stageInfo.StageLv == showStageLevel)
			{
				titleText.text = stageInfo.StageName;
				break;
			}
		}
	}

	private void UpdateRewards()
	{
		for (int i = 0 ; i < uiItemRewardList.Length ; i++)
		{
			uiItemRewardList[i].gameObject.SetActive(false);
		}


		foreach(var stageInfo in showStageInfos)
		{
			if(stageInfo.StageLv == showStageLevel)
			{
				var rewards = stageInfo.GetAllRewards();

				for (int i = 0 ; i < uiItemRewardList.Length ; i++)
				{
					if (rewards.Count <= i)
					{
						uiItemRewardList[i].gameObject.SetActive(false);
					}
					else
					{
						uiItemRewardList[i].gameObject.SetActive(true);
						uiItemRewardList[i].OnUpdate(rewards[i]);

						probabilityText[i].text = $"{(rewards[i].DropRatio * 100).ToString("F2")}%";
					}
				}
			}
		}
	}

	public void SelectStage(int _stageLv)
	{
		showStageLevel = _stageLv;

		UpdateTitle();
		UpdateRewards();

		foreach (var uiItem in uiStageInfos)
		{
			uiItem.RefreshSelected();
		}
	}

	private void OnLeftButtonClick()
	{
		int newAreaIndex = Mathf.Clamp(showAreaIndex - 1, 0, showAreaIndex);

		if(newAreaIndex == showAreaIndex)
		{
			return;
		}

		OnUpdate(showDifficult, newAreaIndex);
	}

	private void OnRightButtonClick()
	{
		int newAreaIndex = Mathf.Clamp(showAreaIndex + 1, 0, StageManager.it.metaGameStage.GetLastAreaIndex(WaveType.Normal, showDifficult));

		if (newAreaIndex == showAreaIndex)
		{
			return;
		}

		OnUpdate(showDifficult, newAreaIndex);
	}

	private void OnDifficultChange(StageDifficulty _difficult)
	{
		if(showDifficult == _difficult)
		{
			return;
		}

		OnUpdate(_difficult, showAreaIndex);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
