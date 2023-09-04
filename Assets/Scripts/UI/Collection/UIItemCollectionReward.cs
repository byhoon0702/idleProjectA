using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIItemCollectionReward : MonoBehaviour
{
	[SerializeField] private Button buttonGet;
	[SerializeField] private Button buttonComplete;
	[SerializeField] private Button buttonCant;
	[SerializeField] private UITextMeshPro textButton;
	[SerializeField] private TextMeshProUGUI textRewardTitle;
	[SerializeField] private TextMeshProUGUI textRewardValue;

	RuntimeData.CollectionInfo collectionInfo;
	RuntimeData.CollectionReward collectionReward;
	UIItemCollection parent;
	bool conditionFulfill = true;
	bool isGet = false;
	private void Awake()
	{
		buttonGet.SetButtonEvent(OnClickGetReward);
		//buttonComplete.SetButtonEvent(OnClickGetReward);
		//buttonCant.SetButtonEvent(OnClickGetReward);
	}

	public void OnClickGetReward()
	{
		if (isGet)
		{
			return;
		}

		collectionInfo.GetReward(collectionReward);
		parent.OnUpdateParent();
	}


	public void OnUpdate(UIItemCollection _parent, RuntimeData.CollectionInfo _info, RuntimeData.CollectionReward _reward, List<CollectionCellData> _cellData)
	{
		parent = _parent;
		collectionInfo = _info;
		collectionReward = _reward;
		conditionFulfill = true;
		for (int i = 0; i < _cellData.Count; i++)
		{
			if (_cellData[i].level < collectionReward.level)
			{
				conditionFulfill = false;
				break;
			}
		}

		if (_cellData.Count == 0)
		{
			conditionFulfill = false;
		}

		if (conditionFulfill)
		{
			textRewardTitle.text = $"<color=green>레벨 {collectionReward.level} 달성시</color>";
		}
		else
		{
			textRewardTitle.text = $"<color=red>레벨 {collectionReward.level} 달성시</color>";
		}

		isGet = _reward.IsGet;


		textRewardValue.text = $"{collectionReward.reward.type.ToUIString()} +{collectionReward.reward.Value.ToString()}%";
		buttonGet.gameObject.SetActive(conditionFulfill && isGet == false);
		buttonCant.gameObject.SetActive(!conditionFulfill);
		buttonComplete.gameObject.SetActive(isGet);
	}
}
