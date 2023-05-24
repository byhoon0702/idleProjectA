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
	[SerializeField] private TextMeshProUGUI dungeonName;

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
			var itemInfo = DataManager.GetFromAll<ItemData>(_data.dungeonItemTid);


		}
		else
		{
			consumeObj.SetActive(false);
		}

		dungeonName.text = _data.name;


	}

	public void OnRefresh()
	{

	}

	private void OnEnterButtonClick()
	{
		//var stage = StageManager.it.metaGameStage.GetStages(dungeonInfo.stageType);

		var stageList = GameManager.UserDB.stageContainer.GetStages(dungeonInfo.stageType, StageDifficulty.NONE);
		StageManager.it.PlayStage(stageList[0]);
		parent.Close();
	}
}
