using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HLoopDungeonDetailStandardDataSource : VIVID.LoopScrollDataSource
{
	public UIDungeonDetailStandard owner;
	public List<GameStageInfo> stageInfos = new List<GameStageInfo>();

	public override void InitializeData(Transform transform, int idx)
	{
		transform.name = $"{idx}.Contents";
		var item = transform.GetComponent<UIItemDungeonDetailStandard>();

		item.OnUpdate(owner, stageInfos[idx]);
		item.RefreshSelected();
	}
	public override void ProvideData(Transform transform, int idx)
	{
		transform.name = $"{idx}.Contents";
		var item = transform.GetComponent<UIItemDungeonDetailStandard>();

		item.OnUpdate(owner, stageInfos[idx]);
		item.RefreshSelected();
	}
}

public class UIDungeonDetailStandard : MonoBehaviour, IUIClosable
{
	[Header("List")]
	[SerializeField] private VIVID.LoopVerticalScrollRect scrollRect;
	private HLoopDungeonDetailStandardDataSource dataSource;

	[Header("Info")]
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI combatPowerText;

	[Header("보상")]
	[SerializeField] private UIItemBase[] uiItemRewardList;
	[SerializeField] private TextMeshProUGUI[] probabilityText;

	[Header("버튼")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button moveButton;
	[SerializeField] private Button autoButton;
	[SerializeField] private TextMeshProUGUI moveButtonText;
	[SerializeField] private TextMeshProUGUI autoButtonText;


	/// <summary>
	/// 선택된 스테이지
	/// </summary>
	private GameStageInfo selectStage;
	public int ShowStageLv => selectStage.StageLv;




	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		moveButton.onClick.RemoveAllListeners();
		moveButton.onClick.AddListener(OnMoveButtonClick);

		autoButton.onClick.RemoveAllListeners();
		autoButton.onClick.AddListener(OnAutoButtonClick);
	}

	public void OnUpdate(List<GameStageInfo> _stageInfos)
	{
		// 가장 최근의 스테이지는 UI에서 선택이 되어있음
		var recentLv = UserInfo.stage.RecentStageLv(_stageInfos[0].WaveType);
		selectStage = StageManager.it.metaGameStage.GetStage(_stageInfos[0].WaveType, recentLv);
		UpdateInfo(selectStage);
		UpdateButton();


		// 리스트 초기화
		dataSource = new HLoopDungeonDetailStandardDataSource();
		dataSource.stageInfos = _stageInfos;

		dataSource.owner = this;
		scrollRect.dataSource = dataSource;
		scrollRect.totalCount = _stageInfos.Count;

		scrollRect.RefillCells(in_is_init: true);
	}

	public void SelectStage(GameStageInfo _stageInfo)
	{
		selectStage = _stageInfo;

		UpdateInfo(selectStage);
		Refresh();
	}

	private void UpdateInfo(GameStageInfo _stageInfo)
	{
		titleText.text = $"{_stageInfo.StageName} {_stageInfo.StageSubTitle}";
		combatPowerText.text = $"권장: {_stageInfo.BestCombatPower.ToString()}";

		var rewards = _stageInfo.GetAllRewards();
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
			}
		}
	}

	private void UpdateButton()
	{
		var item = Inventory.it.FindItemByTid(selectStage.ConsumeItemTid);

		if (item != null)
		{
			if (item is ItemMoney)
			{
				var itemMoney = item as ItemMoney;

				moveButtonText.text = $"이동 {itemMoney.Count.ToString()} / {itemMoney.SystemMax.ToString()}";
				autoButtonText.text = $"소탕 {itemMoney.Count.ToString()} / {itemMoney.SystemMax.ToString()}";
			}
			else
			{
				moveButtonText.text = $"이동 {item.Count.ToString()}";
				autoButtonText.text = $"소탕 {item.Count.ToString()}";
			}
		}
		else
		{
			moveButtonText.text = $"이동";
			autoButtonText.text = $"소탕";
		}
	}

	public void Refresh()
	{
		UpdateButton();
		scrollRect.RefreshCells();
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	private void OnMoveButtonClick()
	{
		var playResult = selectStage.Play();
		if (playResult.Fail())
		{
			PopAlert.Create(playResult);
			return;
		}

		UIController.it.UIDungeonList.Close();
	}

	private void OnAutoButtonClick()
	{

	}
}
