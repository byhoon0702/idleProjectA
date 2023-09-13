using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupQuestList : UIBase
{
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Toggle toggleDaily, toggleRepeat;
	[SerializeField] private Transform content;

	private QuestType currentType;

	protected override void OnEnable()
	{
		AddCloseListener();
		QuestContainer.OnUpdate += OnUpdateUI;
	}

	protected override void OnDisable()
	{
		RemoveCloseListener();
		QuestContainer.OnUpdate -= OnUpdateUI;
	}


	void OnUpdateUI()
	{
		OnUpdateList(currentType);
	}
	void Awake()
	{
		toggleDaily.onValueChanged.AddListener(ToggleDaily);
		toggleRepeat.onValueChanged.AddListener(ToggleRepeat);
	}

	public void ToggleDaily(bool isTrue)
	{
		if (isTrue)
		{
			OnUpdateList(QuestType.DAILY);
		}
	}
	public void ToggleRepeat(bool isTrue)
	{
		if (isTrue)
		{
			OnUpdateList(QuestType.REPEAT);
		}
	}

	public void OnUpdateList(QuestType type)
	{
		currentType = type;
		List<RuntimeData.QuestInfo> infos = new List<RuntimeData.QuestInfo>();
		switch (type)

		{
			case QuestType.DAILY:
				infos = PlatformManager.UserDB.questContainer.DailyQuestList;
				break;
			case QuestType.REPEAT:
				infos = PlatformManager.UserDB.questContainer.RepeatQuestList;
				break;
		}

		int differ = infos.Count - content.childCount;
		for (int i = 0; i < differ; i++)
		{
			GameObject go = Instantiate(itemPrefab, content);
		}

		for (int i = 0; i < content.childCount; i++)
		{

			Transform child = content.GetChild(i);

			if (i < infos.Count)
			{
				child.gameObject.SetActive(true);
				UIItemQuest itemQuest = child.GetComponent<UIItemQuest>();
				itemQuest.SetData(infos[i], () => { OnUpdateList(currentType); });
			}
			else
			{
				child.gameObject.SetActive(false);
			}
		}
	}
	protected override void OnActivate()
	{
		toggleDaily.SetIsOnWithoutNotify(true);
		ToggleDaily(true);
	}
}
