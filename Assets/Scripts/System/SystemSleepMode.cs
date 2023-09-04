using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemSleepMode : MonoBehaviour
{

	[SerializeField] private Slider sliderLock;
	[SerializeField] private TextMeshProUGUI textBattery;
	[SerializeField] private TextMeshProUGUI textPlayTime;
	[SerializeField] private TextMeshProUGUI textTimeNow;
	[SerializeField] private TextMeshProUGUI textStageTitle;
	[SerializeField] private TextMeshProUGUI textGold;
	[SerializeField] private TextMeshProUGUI textExp;

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera uiCamera;

	private List<AddItemInfo> addItemInfos;
	private IdleNumber _gold;
	private IdleNumber _exp;
	public Dictionary<long, AddItemInfo> stageAcquiredReward { get; private set; } = new Dictionary<long, AddItemInfo>();
	private System.DateTime time;

	private void OnEnable()
	{
		if (GameManager.it != null)
		{
			GameManager.it.OnSleepModeAcquiredGold += OnUpdateGold;
			GameManager.it.OnSleepModeAcquiredExp += OnUpdateExp;
			GameManager.it.OnSleepModeAcquiredItem += SetGrid;
		}
	}
	private void OnDisable()
	{
		if (GameManager.it != null)
		{
			GameManager.it.OnSleepModeAcquiredGold -= OnUpdateGold;
			GameManager.it.OnSleepModeAcquiredExp -= OnUpdateExp;
			GameManager.it.OnSleepModeAcquiredItem -= SetGrid;
		}
	}

	private void OnUpdateGold(IdleNumber gold)
	{
		_gold += gold;
		textGold.text = _gold.ToString();
	}
	private void OnUpdateExp(IdleNumber exp)
	{
		_exp += exp;
		textExp.text = _exp.ToString();
	}

	public void Open()
	{

		SoundManager.Instance.MuteAll();
		GameManager.it.isSleepMode = true;
		gameObject.SetActive(true);
		mainCamera.enabled = false;
		uiCamera.enabled = false;
		sliderLock.value = 0;

		textStageTitle.text = StageManager.it.CurrentStage.StageName;
		time = new DateTime(System.DateTime.Now.Ticks);
		textPlayTime.text = "000:00:00";

		textTimeNow.text = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
		stageAcquiredReward.Clear();
		SetGrid(null);
		_gold.Reset();
		_exp.Reset();

		textExp.text = "0";
		textGold.text = "0";
	}

	public void OnPointerUp()
	{
		if (sliderLock.value == 1)
		{
			SleepModeUnlock();
			//잠금해제
			return;
		}

		sliderLock.value = 0;
	}

	private void Update()
	{
		textBattery.text = $"{SystemInfo.batteryLevel * 100}%";
		textTimeNow.text = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
		TimeSpan ts = DateTime.Now - time;
		textPlayTime.text = $"{ts.Hours.ToString("00")}:{ts.Minutes.ToString("00")}:{ts.Seconds.ToString("00")}";
	}

	public void SetGrid(List<RuntimeData.RewardInfo> info)
	{
		if (info == null)
		{
			for (int i = 0; i < content.childCount; i++)
			{
				var child = content.GetChild(i);
				child.gameObject.SetActive(false);
			}
			return;
		}

		for (int i = 0; i < info.Count; i++)
		{
			long tid = info[i].Tid;
			if (stageAcquiredReward.ContainsKey(tid))
			{
				var reward = stageAcquiredReward[tid];

				reward.value += info[i].fixedCount;
				stageAcquiredReward[tid] = reward;
			}
			else
			{
				stageAcquiredReward.Add(tid, new AddItemInfo(info[i]));
			}
		}

		int count = stageAcquiredReward.Count;
		AddItemInfo[] array = new AddItemInfo[count];
		stageAcquiredReward.Values.CopyTo(array, 0);
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

	public void SleepModeUnlock()
	{
		SoundManager.Instance.UnmuteAll();
		gameObject.SetActive(false);
		mainCamera.enabled = true;
		uiCamera.enabled = true;
		GameManager.it.isSleepMode = false;
	}
}
