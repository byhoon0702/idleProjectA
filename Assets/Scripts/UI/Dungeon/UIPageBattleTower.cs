using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerStageDataSource : LoopScrollDataSource, LoopScrollPrefabSource
{
	public Transform parent;
	public List<RuntimeData.StageInfo> stageInfos = new List<RuntimeData.StageInfo>();
	public GameObject prefab;

	public UIPageBattleTower selectListener;
	Stack<Transform> pool = new Stack<Transform>();
	public GameObject GetObject(int index)
	{
		if (pool.Count == 0)
		{
			return Object.Instantiate(prefab);
		}

		Transform candidate = pool.Pop();
		candidate.gameObject.SetActive(true);
		return candidate.gameObject;
	}
	public void ProvideData(Transform transform, int index)
	{
		UIItemTowerFloor towerFloor = transform.GetComponent<UIItemTowerFloor>();
		towerFloor.OnUpdate(selectListener, stageInfos[index]);
		towerFloor.OnSelect(selectListener.selectedStageNumber);
		towerFloor.gameObject.SetActive(true);
	}
	public void ReturnObject(Transform transform)
	{
		transform.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
		transform.gameObject.SetActive(false);
		transform.SetParent(parent, false);
		pool.Push(transform);
	}
}


public class UIPageBattleTower : UIPageBattle//, ISelectListener
{
	[SerializeField] private TextMeshProUGUI textRewardLabel;
	[SerializeField] private Button buttonPlay;
	public Button ButtonPlay => buttonPlay;

	[SerializeField] private Button buttonMax;


	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private Transform contentReward;
	[SerializeField] private GameObject itemPrefabReward;

	[SerializeField] private LoopVerticalScrollRect loopVerticalScrollRect;

	public RuntimeData.StageInfo currentInfo { get; private set; }

	private TowerStageDataSource dataSource;
	private List<RuntimeData.StageInfo> stageList;

	private StageRecordData lastStage;
	private int maxFloor;

	private bool allFloorClear = false;

	event OnSelect onSelect;
	public long selectedStageNumber;
	public void AddSelectListener(OnSelect callback)
	{
		//onSelect += callback;
	}
	public void RemoveSelectListener(OnSelect callback)
	{
		//onSelect -= callback;
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

		selectedStageNumber = stageNumber;
		loopVerticalScrollRect.RefreshCells();
		//onSelect?.Invoke(stageNumber);
	}

	void Awake()
	{
		buttonPlay.SetButtonEvent(OnClickPlay);
		buttonMax.SetButtonEvent(OnClickMax);
	}


	public override void OnUpdate(UIManagementBattle _parent)
	{
		parent = _parent;

		SetFloorGrid();
		SetReward();

		buttonPlay.gameObject.SetActive(allFloorClear == false);
		buttonMax.gameObject.SetActive(allFloorClear);
	}
	public void OnClickMax()
	{
		ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_tower_max_floor_clear"]);
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

		GameUIManager.it.Close();
	}

	private void SetFloorGrid()
	{
		lastStage = PlatformManager.UserDB.stageContainer.GetLastStage(StageType.Tower, 0);
		stageList = PlatformManager.UserDB.stageContainer.GetStageList(StageType.Tower, 0);

		stageList.Reverse();
		maxFloor = stageList.Count;

		allFloorClear = maxFloor == lastStage.stageNumber;
		int index = stageList.FindIndex(x => x.StageNumber == lastStage.stageNumber);
		if (index < 0)
		{
			index = maxFloor - 1;
		}

		currentInfo = stageList[index];

		dataSource = new TowerStageDataSource();
		dataSource.stageInfos = stageList;
		dataSource.parent = loopVerticalScrollRect.transform;
		dataSource.prefab = itemPrefab;
		dataSource.selectListener = this;
		selectedStageNumber = currentInfo.StageNumber;

		loopVerticalScrollRect.prefabSource = dataSource;
		loopVerticalScrollRect.dataSource = dataSource;
		loopVerticalScrollRect.totalCount = stageList.Count;


		if (index >= maxFloor - 5)
		{
			loopVerticalScrollRect.RefillCells();
			//loopVerticalScrollRect.SrollToCellWithinTime(index, 0.001f);
		}
		else
		{
			loopVerticalScrollRect.RefillCellsFromEnd(index);
		}

		///loopVerticalScrollRect.RefillCellsFromEnd
		///loopVerticalScrollRect.SrollToCellWithinTime(index, 0.001f);

		SetReward();
	}

	private void SetReward()
	{
		if (currentInfo == null)
		{
			return;
		}

		textRewardLabel.text = $"{currentInfo.StageNumber}층\n클리어보상";
		currentInfo.SetStageReward((IdleNumber)currentInfo.StageNumber - 1);
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
