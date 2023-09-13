using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemCollection : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private UIItemCollectionCell[] items;
	[SerializeField] private UIItemCollectionReward[] rewards;

	RuntimeData.CollectionInfo info;

	UIPopupCollection parent;

	public void OnUpdateParent()
	{
		parent.Refresh();
	}

	public CollectionCellData GetData(long tid, RewardCategory category)
	{
		CollectionCellData data = new CollectionCellData();
		switch (category)
		{
			case RewardCategory.Equip:
				{
					var item = PlatformManager.UserDB.equipContainer.FindEquipItem(tid);
					if (item == null)
					{
						Debug.LogError($"{tid} is Missing");
						return data;
					}
					data.grade = item.grade;
					data.icon = item.IconImage;
					data.level = item.Level;
				}
				break;
			case RewardCategory.Skill:
				{
					var item = PlatformManager.UserDB.skillContainer.FindSKill(tid);
					if (item == null)
					{
						Debug.LogError($"{tid} is Missing");
						return data;
					}
					data.grade = item.grade;
					data.icon = item.IconImage;
					data.level = item.Level;
				}
				break;
			case RewardCategory.Pet:
				{
					var item = PlatformManager.UserDB.petContainer.FindPetItem(tid);
					if (item == null)
					{
						Debug.LogError($"{tid} is Missing");
						return data;
					}
					data.grade = item.grade;
					data.icon = item.IconImage;
					data.level = item.Level;
				}
				break;
			default:
				return data;

		}
		return data;
	}


	public void OnUpdate(UIPopupCollection _parent, RuntimeData.CollectionInfo _info)
	{
		info = _info;
		parent = _parent;
		textTitle.text = PlatformManager.Language[info.rawData.name];
		List<CollectionCellData> datas = new List<CollectionCellData>();
		for (int i = 0; i < items.Length; i++)
		{
			if (i < info.rawData.itemList.Count)
			{
				var item = info.rawData.itemList[i];
				var data = GetData(item.tid, item.category);
				datas.Add(data);
				items[i].OnUpdate(data);
				items[i].gameObject.SetActive(true);
			}
			else
			{
				items[i].gameObject.SetActive(false);
			}
		}

		for (int i = 0; i < rewards.Length; i++)
		{
			if (i < info.rewardList.Count)
			{
				var item = info.rewardList[i];
				rewards[i].OnUpdate(this, info, item, datas);
			}
		}
	}
}
