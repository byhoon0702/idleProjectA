using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageBattleDungeon : UIPageBattle
{

	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	private List<BattleData> dungeonDatas = new List<BattleData>();

	public UIItemDungeonList Find(long tid)
	{
		for (int i = 0; i < content.childCount; i++)
		{
			var item = content.GetChild(i).GetComponent<UIItemDungeonList>();
			if (item.BattleData.tid == tid)
			{
				return item;
			}
		}
		return null;
	}

	public override void OnUpdate(UIManagementBattle _parent)
	{
		parent = _parent;
		dungeonDatas.Clear();
		var dungeonInfoList = DataManager.Get<BattleDataSheet>().GetInfosClone();

		for (int i = 0; i < dungeonInfoList.Count; i++)
		{
			var dungeonData = dungeonInfoList[i];

			if (dungeonData.stageType == StageType.Dungeon)
			{
				dungeonDatas.Add(dungeonData);
			}
		}
		SetGrid();
	}

	private void SetGrid()
	{
		content.CreateListCell(dungeonDatas.Count, itemPrefab);

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < dungeonDatas.Count)
			{

				UIItemDungeonList item = child.GetComponent<UIItemDungeonList>();
				item.SetData(this, dungeonDatas[i]);
				child.gameObject.SetActive(true);
			}

		}
	}

	public void ShowDungeonPopup(BattleData dungeonInfo)
	{
		parent.ShowDungeonPopup(dungeonInfo);
	}

	public void ShowDungeonStagePopup(BattleData dungeonInfo)
	{
		parent.ShowDungeonStagePopup(dungeonInfo);
	}
}
