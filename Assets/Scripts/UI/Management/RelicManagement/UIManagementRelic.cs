using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class UIManagementRelic : UIBase
{

	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform contentRoot;

	public void OnUpdate()
	{
		SetGrid();
	}
	public UIItemRelic Find(long tid)
	{
		for (int i = 0; i < contentRoot.childCount; i++)
		{
			var tr = contentRoot.GetChild(i).GetComponent<UIItemRelic>();
			if (tr.RelicInfo.Tid == tid)
			{
				return tr;
			}

		}
		return null;
	}

	private void SetGrid()
	{
		var list = PlatformManager.UserDB.relicContainer.RelicInfos;
		int makeCount = list.Count - contentRoot.childCount;
		if (makeCount > 0)
		{
			for (int i = 0; i < makeCount; i++)
			{
				GameObject go = Instantiate(itemPrefab, contentRoot);
				go.SetActive(false);
			}
		}

		for (int i = 0; i < contentRoot.childCount; i++)
		{
			var child = contentRoot.GetChild(i);
			if (i < list.Count)
			{
				UIItemRelic relic = child.GetComponent<UIItemRelic>();
				relic.SetData(list[i]);
				relic.gameObject.SetActive(true);
			}
			else
			{
				child.gameObject.SetActive(false);
			}
		}


	}

}
