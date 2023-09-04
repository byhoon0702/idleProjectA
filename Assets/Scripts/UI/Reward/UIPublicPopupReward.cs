using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class UIPublicPopupReward : MonoBehaviour
{
	[SerializeField] protected UITextMeshPro uiTextTitle;

	[SerializeField] protected GameObject rewardPrefab;

	[SerializeField] protected RectTransform content;

	protected List<AddItemInfo> rewardList;

	public virtual void Show(List<AddItemInfo> _rewardList)
	{
		gameObject.SetActive(true);

		rewardList = _rewardList;
		OnUpdate();
	}

	public abstract void OnUpdate();
}
