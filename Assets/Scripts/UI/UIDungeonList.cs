using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonList : MonoBehaviour, IUIClosable
{
	[Space(10)]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button dungeonButton;
	[SerializeField] private Button challengeButton;

	[SerializeField] private UIItemDungeonList itemPrefab;

	[Space(10)]
	[SerializeField] private GameObject dungeonObj;
	[SerializeField] private Transform dungeonItemRoot;

	[Space(10)]
	[SerializeField] private GameObject challengeObj;
	[SerializeField] private Transform challengeItemRoot;

	private List<UIItemDungeonList> uiDungeonItems = new List<UIItemDungeonList>();
	private List<UIItemDungeonList> uiChallengeItems = new List<UIItemDungeonList>();
	private DungeonType showType;


	void OnEnable()
	{
		AddCloseListener();
	}
	void OnDisable()
	{
		RemoveCloseListener();
	}
	public void AddCloseListener()
	{
		GameUIManager.it.onClose += Close;
	}

	public void RemoveCloseListener()
	{
		GameUIManager.it.onClose -= Close;
	}
	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		dungeonButton.onClick.RemoveAllListeners();
		dungeonButton.onClick.AddListener(OnDungeonButtonClick);

		challengeButton.onClick.RemoveAllListeners();
		challengeButton.onClick.AddListener(OnChallengeButtonClick);


	}

	public void OnUpdate()
	{
		if (showType == DungeonType.Challenge)
		{
			ShowChallengeList();
		}
		else
		{
			ShowDungeonList();
		}
	}

	private void ShowDungeonList()
	{
		showType = DungeonType.Dungeon;

		dungeonObj.SetActive(true);
		challengeObj.SetActive(false);

		if (uiDungeonItems.Count == 0)
		{
			var dungeonInfoList = DataManager.Get<DungeonDataSheet>().infos;

			for (int i = 0; i < dungeonInfoList.Count; i++)
			{
				var dungeonInfo = dungeonInfoList[i];
				if (dungeonInfo.type == DungeonType.Dungeon)
				{
					UIItemDungeonList item = Instantiate(itemPrefab, dungeonItemRoot);
					item.SetData(this, dungeonInfo);
					uiDungeonItems.Add(item);
				}
			}
		}

		foreach (var dungeon in uiDungeonItems)
		{
			dungeon.OnRefresh();
		}
	}

	private void ShowChallengeList()
	{
		showType = DungeonType.Challenge;

		dungeonObj.SetActive(false);
		challengeObj.SetActive(true);

		// 챌린지
		if (uiChallengeItems.Count == 0)
		{
			var challengeInfoList = DataManager.Get<DungeonDataSheet>().infos;

			for (int i = 0; i < challengeInfoList.Count; i++)
			{
				var challengeInfo = challengeInfoList[i];
				if (challengeInfo.type == DungeonType.Challenge)
				{
					UIItemDungeonList item = Instantiate(itemPrefab, challengeItemRoot);
					item.SetData(this, challengeInfo);
					uiChallengeItems.Add(item);
				}
			}
		}

		foreach (var challenge in uiChallengeItems)
		{
			challenge.OnRefresh();
		}
	}

	private void OnDungeonButtonClick()
	{
		if (showType == DungeonType.Dungeon)
		{
			return;
		}

		ShowDungeonList();
	}

	private void OnChallengeButtonClick()
	{
		if (showType == DungeonType.Challenge)
		{
			return;
		}

		ShowChallengeList();
	}


	public void Close()
	{
		UIController.it.InactivateAllBottomToggle();

		gameObject.SetActive(false);
	}

	public bool Closable()
	{
		return true;
	}
}


