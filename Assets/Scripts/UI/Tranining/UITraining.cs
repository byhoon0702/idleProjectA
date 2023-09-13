using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum LevelUpCount
{
	ONE,
	TEN,
	HUNDRED,
	MAX,

}

public class UITraining : UIBase
{
	[SerializeField] private UIItemTraining itemPrefab;
	[SerializeField] private Transform itemRoot;

	[SerializeField] Toggle[] toggles;

	public LevelUpCount levelupCount { get; private set; } = LevelUpCount.ONE;

	private List<UIItemTraining> items = new List<UIItemTraining>();

	protected override void OnEnable()
	{
		base.OnEnable();
		EventCallbacks.onItemChanged += OnItemChanged;

		switch (levelupCount)
		{
			case LevelUpCount.ONE:
				toggles[0].isOn = true;
				break;
			case LevelUpCount.TEN:
				toggles[1].isOn = true;
				break;
			case LevelUpCount.HUNDRED:
				toggles[2].isOn = true;
				break;
			case LevelUpCount.MAX:
				toggles[3].isOn = true;
				break;

		}

	}

	protected override void OnDisable()
	{
		base.OnDisable();
		EventCallbacks.onItemChanged -= OnItemChanged;
	}

	private void OnItemChanged(List<long> _changedItems)
	{

	}
	private void Awake()
	{
		levelupCount = LevelUpCount.ONE;
	}
	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItem();
	}

	public UIItemTraining Find(long tid)
	{
		for (int i = 0; i < itemRoot.childCount; i++)
		{
			var itemTraining = itemRoot.GetChild(i).GetComponent<UIItemTraining>();
			if (itemTraining.TrainingInfo.Tid == tid)
			{
				return itemTraining;
			}
		}
		return null;
	}

	public void UpdateItem()
	{
		var list = PlatformManager.UserDB.training.trainingInfos;
		int countForMake = list.Count - itemRoot.childCount;

		itemRoot.CreateListCell(countForMake, itemPrefab.gameObject);

		list.Sort((a, b) => { return b.isOpen.CompareTo(a.isOpen); });

		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIItemTraining slot = child.GetComponent<UIItemTraining>();

			var info = list[i];
			slot.OnUpdate(this, info);
		}
	}

	public void Refresh()
	{
		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (child.gameObject.activeInHierarchy == false)
			{
				continue;
			}

			UIItemTraining slot = child.GetComponent<UIItemTraining>();
			slot.OnRefresh();
		}
	}

	public void OnLevelUpCountOne(bool isOn)
	{
		if (isOn)
		{
			levelupCount = LevelUpCount.ONE;
			Refresh();
		}
	}
	public void OnLevelUpCountTen(bool isOn)
	{
		if (isOn)
		{
			levelupCount = LevelUpCount.TEN;
			Refresh();
		}
	}
	public void OnLevelUpCountHundred(bool isOn)
	{
		if (isOn)
		{
			levelupCount = LevelUpCount.HUNDRED;
			Refresh();
		}
	}
	public void OnLevelUpCountMaxLevel(bool isOn)
	{
		if (isOn)
		{
			levelupCount = LevelUpCount.MAX;
			Refresh();
		}
	}

}
