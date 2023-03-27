using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HLoopDungeonDetailNightmareTowerDataSource : VIVID.LoopScrollDataSource
{
	public UIDungeonDetailNightmareTower owner;
	public List<GameStageInfo> stageInfos = new List<GameStageInfo>();

	public override void InitializeData(Transform transform, int idx)
	{
		transform.name = $"{idx}.Contents";
		var item = transform.GetComponent<UIItemDungeonDetailNightmareTower>();

		item.OnUpdate(owner, stageInfos[idx]);
		item.RefreshSelected();
	}
	public override void ProvideData(Transform transform, int idx)
	{
		transform.name = $"{idx}.Contents";
		var item = transform.GetComponent<UIItemDungeonDetailNightmareTower>();

		item.OnUpdate(owner, stageInfos[idx]);
		item.RefreshSelected();
	}
}

public class UIDungeonDetailNightmareTower : MonoBehaviour, IUIClosable
{
	[Header("List")]
	[SerializeField] private VIVID.LoopVerticalScrollRect scrollRect;
	private HLoopDungeonDetailNightmareTowerDataSource dataSource;

	[Header("버튼")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button moveButton;
	[SerializeField] private TextMeshProUGUI moveButtonText;


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
	}

	public void OnUpdate(List<GameStageInfo> _stageInfos)
	{
		// 가장 최근의 스테이지는 UI에서 선택이 되어있음
		var recentLv = UserInfo.stage.RecentStageLv(_stageInfos[0].WaveType);
		selectStage = StageManager.it.metaGameStage.GetStage(_stageInfos[0].WaveType, recentLv);


		// 리스트 초기화
		dataSource = new HLoopDungeonDetailNightmareTowerDataSource();
		dataSource.stageInfos = new List<GameStageInfo>();
		for(int i=0 ; i<_stageInfos.Count ; i++)
		{
			dataSource.stageInfos.Add(_stageInfos[i]);
		}

		//역순정렬 필요
		dataSource.stageInfos.Sort((a, b) =>
		{
			return b.StageLv.CompareTo(a.StageLv);
		});


		dataSource.owner = this;
		scrollRect.dataSource = dataSource;
		scrollRect.totalCount = _stageInfos.Count;

		scrollRect.RefillCells(in_is_init: true);
		scrollRect.RefreshCells();
	}

	public void SelectStage(GameStageInfo _stageInfo)
	{
		selectStage = _stageInfo;
		Refresh();
	}

	public void Refresh()
	{
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
