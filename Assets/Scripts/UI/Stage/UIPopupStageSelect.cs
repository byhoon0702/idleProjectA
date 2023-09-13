using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageListDataSource : LoopScrollDataSource, LoopScrollPrefabSource
{
	public Transform parent;
	public List<RuntimeData.StageInfo> stageInfos = new List<RuntimeData.StageInfo>();
	public GameObject prefab;
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

	public void ProvideData(Transform transform, int idx)
	{
		UIItemStage item = transform.GetComponent<UIItemStage>();
		item.OnUpdate(stageInfos[idx]);
	}

	public void ReturnObject(Transform trans)
	{
		// Use `DestroyImmediate` here if you don't need Pool
		trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
		trans.gameObject.SetActive(false);
		trans.SetParent(parent, false);
		pool.Push(trans);
	}
}


public class UIPopupStageSelect : UIBase
{
	[SerializeField] private TextMeshProUGUI textAreaName;
	[SerializeField] private Image imageArea;
	[SerializeField] private Button buttonNextArea;
	[SerializeField] private Button buttonPrevArea;

	[SerializeField] private LoopVerticalScrollRect loopVerticalScrollRect;
	[SerializeField] private ScrollRect scroll;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;


	private List<RuntimeData.StageInfo> stageinfos;
	private int currentAreaNumber = 1;
	private StageListDataSource dataSource;
	private void Awake()
	{
		buttonNextArea.onClick.RemoveAllListeners();
		buttonNextArea.onClick.AddListener(OnClickNext);
		buttonPrevArea.onClick.RemoveAllListeners();
		buttonPrevArea.onClick.AddListener(OnClickPrev);
	}
	private void OnClickNext()
	{
		int area = currentAreaNumber + 1;

		var nextArea = PlatformManager.UserDB.stageContainer.stageDataList[StageType.Normal].Find(x => x.AreaNumber == area);
		if (nextArea == null)
		{
			return;
		}

		currentAreaNumber = area;
		OnUpdate();
	}
	private void OnClickPrev()
	{
		int area = currentAreaNumber - 1;

		if (area < 1)
		{
			return;
		}

		currentAreaNumber = area;
		OnUpdate();
	}

	public void Show()
	{
		gameObject.SetActive(true);
		if (StageManager.it.CurrentStage.StageType == StageType.Normal)
		{
			currentAreaNumber = StageManager.it.CurrentStage.AreaNumber;
		}
		else
		{
			currentAreaNumber = PlatformManager.UserDB.stageContainer.LastPlayedNormalStage().AreaNumber;
		}

		OnUpdate();
	}

	public void OnUpdate()
	{
		stageinfos = PlatformManager.UserDB.stageContainer.stageDataList[StageType.Normal].FindAll(x => x.AreaNumber == currentAreaNumber);
		imageArea.sprite = stageinfos[0].Icon;
		imageArea.SetNativeSize();
		textAreaName.text = PlatformManager.Language[stageinfos[0].stageData.name];
		SetGrid();
	}
	private void SetGrid()
	{
		dataSource = new StageListDataSource();
		dataSource.stageInfos = stageinfos;
		dataSource.parent = loopVerticalScrollRect.transform;
		dataSource.prefab = itemPrefab;

		loopVerticalScrollRect.prefabSource = dataSource;
		loopVerticalScrollRect.dataSource = dataSource;
		loopVerticalScrollRect.totalCount = stageinfos.Count;
		loopVerticalScrollRect.RefillCells();
		//content.CreateListCell(stageinfos.Count, itemPrefab, UpdateGrid);
	}

	private void UpdateGrid()
	{
		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < stageinfos.Count)
			{
				child.gameObject.SetActive(true);
				UIItemStage itemstage = child.GetComponent<UIItemStage>();
				itemstage.OnUpdate(stageinfos[i]);
			}
		}
	}
}
