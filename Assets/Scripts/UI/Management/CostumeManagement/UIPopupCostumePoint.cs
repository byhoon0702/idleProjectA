using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UIPopupCostumePoint : UIBase
{
	[SerializeField] private UITextMeshPro uiTextTitle;

	[SerializeField] private Transform contentStats;
	[SerializeField] private GameObject itemPrefabStats;

	[SerializeField] private Transform contentPoints;
	[SerializeField] private GameObject itemPrefabPoints;

	List<RuntimeData.CostumePointInfo> costumePoints = new List<RuntimeData.CostumePointInfo>();
	Dictionary<StatsType, RuntimeData.AbilityInfo> costumeAbilities = new Dictionary<StatsType, RuntimeData.AbilityInfo>();

	System.Action _onClose;
	public void Show(System.Action onClose = null)
	{
		_onClose = onClose;
		gameObject.SetActive(true);
		OnUpdate();
	}

	public void OnUpdate()
	{
		costumePoints = PlatformManager.UserDB.costumeContainer.CostumePointList;
		costumeAbilities = PlatformManager.UserDB.costumeContainer.costumeAbilities;
		SetGrid();
	}
	private void SetGrid()
	{

		var list = costumeAbilities.Values.ToList();
		contentStats.CreateListCell(list.Count, itemPrefabStats, null);

		for (int i = 0; i < contentStats.childCount; i++)
		{
			var child = contentStats.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < list.Count)
			{
				UIItemStats uiItemStat = child.GetComponent<UIItemStats>();
				uiItemStat.OnUpdate(list[i].type.ToUIString(), $"+{list[i].Value.ToString()}%");
				child.gameObject.SetActive(true);
			}
		}

		contentPoints.CreateListCell(costumePoints.Count, itemPrefabPoints, null);
		for (int i = 0; i < contentPoints.childCount; i++)
		{
			var child = contentPoints.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < costumePoints.Count)
			{
				UIItemCostumePoint uiItemCostumePoint = child.GetComponent<UIItemCostumePoint>();
				uiItemCostumePoint.OnUpdate(this, costumePoints[i]);
				child.gameObject.SetActive(true);
			}
		}

	}
	protected override void OnClose()
	{
		base.OnClose();
		_onClose?.Invoke();
	}

}
