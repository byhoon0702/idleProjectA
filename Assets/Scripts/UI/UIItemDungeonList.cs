using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UIItemDungeonList : MonoBehaviour
{
	[SerializeField] private Image mainImage;
	[SerializeField] private GameObject consumeObj;
	[SerializeField] private Image itemIconImage;
	[SerializeField] private TextMeshProUGUI itemCount;
	[SerializeField] private Button enterButton;
	[SerializeField] private UITextMeshPro uiTextdungeonName;
	[SerializeField] private UITextMeshPro uiTextDungeonDescription;

	private UIDungeonList parent;
	private DungeonData dungeonInfo;

	private void Awake()
	{
		enterButton.onClick.RemoveAllListeners();
		enterButton.onClick.AddListener(OnEnterButtonClick);
	}

	public void SetData(UIDungeonList _owner, DungeonData _data)
	{
		parent = _owner;
		dungeonInfo = _data;

		if (_data.dungeonItemTid != 0)
		{
			consumeObj.SetActive(true);
			GameManager.UserDB.inventory.GetScriptableObject(_data.dungeonItemTid, out CurrencyItemObject itemObject);
			if (itemObject != null)
			{
				var item = GameManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

				itemIconImage.sprite = itemObject.Icon;
				itemCount.text = $"{item.Value.ToString()}/{item.max.ToString()}";
			}
		}
		else
		{
			consumeObj.SetActive(false);
		}

		mainImage.sprite = parent.GetDungeonImage(dungeonInfo.stageType);
		uiTextdungeonName.SetKey(dungeonInfo.name);
		uiTextDungeonDescription.SetKey(dungeonInfo.description);
	}

	public void OnRefresh()
	{
		if (dungeonInfo.dungeonItemTid != 0)
		{
			consumeObj.SetActive(true);
			GameManager.UserDB.inventory.GetScriptableObject(dungeonInfo.dungeonItemTid, out CurrencyItemObject itemObject);
			if (itemObject != null)
			{
				var item = GameManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

				itemIconImage.sprite = itemObject.Icon;
				itemCount.text = $"{item.Value.ToString()}/{item.max.ToString()}";
			}
		}

		else
		{
			consumeObj.SetActive(false);
		}
		mainImage.sprite = parent.GetDungeonImage(dungeonInfo.stageType);
		uiTextdungeonName.SetKey(dungeonInfo.name);
		uiTextDungeonDescription.SetKey(dungeonInfo.description);
	}

	private void OnEnterButtonClick()
	{

		switch (dungeonInfo.stageType)
		{
			case StageType.Sesame:
			case StageType.NightmareTower:
			case StageType.Tomb:
			case StageType.Hatchery:
				parent.ShowDungeonStagePopup(dungeonInfo);
				break;
			default:
				parent.ShowDungeonPopup(dungeonInfo);
				break;
		}

		//var stage = StageManager.it.metaGameStage.GetStages(dungeonInfo.stageType);

		//var stageList = GameManager.UserDB.stageContainer.GetStages(dungeonInfo.stageType, StageDifficulty.NONE);
		//StageManager.it.PlayStage(stageList[0]);
		//parent.Close();
	}
}
