using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DungeonImageDictionary : SerializableDictionary<StageType, Sprite>
{ }


public class UIDungeonList : UIBase
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
	[SerializeField] private UIDungeonStagePopup uiDungeonStagePopup;
	[SerializeField] private UIDungeonPopup uiDungeonPopup;

	[SerializeField] private DungeonImageDictionary dungeonImages;

	private List<UIItemDungeonList> uiDungeonItems = new List<UIItemDungeonList>();
	private List<UIItemDungeonList> uiChallengeItems = new List<UIItemDungeonList>();
	private DungeonType showType;


	protected override void OnEnable()
	{
		AddCloseListener();
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Close();
	}
	protected override void OnDisable()
	{
		RemoveCloseListener();
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Close();
	}

	public Sprite GetDungeonImage(StageType _type)
	{
		if (dungeonImages.ContainsKey(_type) == false)
		{
			dungeonImages.Values.TryFirstOrDefault(out Sprite image);
			return image;
		}
		return dungeonImages[_type];
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

	public void ShowDungeonPopup(DungeonData data)
	{
		uiDungeonStagePopup.Close();
		uiDungeonPopup.Init(this, data);
	}
	public void ShowDungeonStagePopup(DungeonData data)
	{
		uiDungeonStagePopup.Init(this, data);
		uiDungeonPopup.Close();
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
			var dungeonInfoList = DataManager.Get<DungeonDataSheet>().GetInfosClone();

			for (int i = 0; i < dungeonInfoList.Count; i++)
			{

				var dungeonInfo = dungeonInfoList[i];

				if (dungeonInfo.type == DungeonType.Dungeon && dungeonInfo.stageType != StageType.Youth)
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

		var challengeInfoList = DataManager.Get<DungeonDataSheet>().GetInfosClone();
		// 챌린지
		if (uiChallengeItems.Count == 0)
		{

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


	public override void Close()
	{
		UIController.it.InactivateAllBottomToggle();

		gameObject.SetActive(false);
	}
}


