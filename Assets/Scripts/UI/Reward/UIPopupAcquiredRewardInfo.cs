using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupAcquiredRewardInfo : UIBase
{
	[SerializeField] private Button buttonClear;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private TextMeshProUGUI textPlayTime;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (StageManager.it != null)
		{
			StageManager.it.OnStageTimeSpan += OnStageTimeSpan;
			StageManager.it.OnStageRewardAcquired += SetGrid;
		}
	}



	protected override void OnDisable()
	{
		base.OnDisable();
		if (StageManager.it != null)
		{
			StageManager.it.OnStageTimeSpan -= OnStageTimeSpan;
			StageManager.it.OnStageRewardAcquired -= SetGrid;
		}

	}

	void Awake()
	{
		buttonClear.onClick.RemoveAllListeners();
		buttonClear.onClick.AddListener(OnClearInfo);
	}
	void OnClearInfo()
	{
		StageManager.it.stagePlayTick = Time.realtimeSinceStartup;
		StageManager.it.stageAcquiredReward.Clear();

		SetGrid();
	}

	private void OnStageTimeSpan(double obj)
	{
		System.TimeSpan ts = System.TimeSpan.FromSeconds(obj);

		textPlayTime.text = $"진행 시간 : {ts.Hours.ToString("00")}:{ts.Minutes.ToString("00")}:{ts.Seconds.ToString("00")}";
	}

	public void Show()
	{
		gameObject.SetActive(true);
		SetGrid();
	}
	void SetGrid()
	{
		int count = StageManager.it.stageAcquiredReward.Count;
		AddItemInfo[] array = new AddItemInfo[count];
		StageManager.it.stageAcquiredReward.Values.CopyTo(array, 0);
		content.CreateListCell(count, itemPrefab, null);

		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < array.Length)
			{
				UIItemReward item = child.GetComponent<UIItemReward>();
				item.Set(array[i]);
				child.gameObject.SetActive(true);
			}
		}

	}


}
