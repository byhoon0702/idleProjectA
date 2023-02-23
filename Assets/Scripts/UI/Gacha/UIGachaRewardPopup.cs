using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGachaRewardPopup : MonoBehaviour, IUIClosable
{
	[SerializeField] private RectTransform itemRoot;
	[SerializeField] private Button closeButton;
	[SerializeField] UIGachaButton gachaButton;

	private UIGachaData uiData;


	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(() => 
		{
			if(Closable())
			{
				Close();
			}
		});
	}

	public void OnUpdate(UIGachaData _uiData, List<GachaResult> _items)
	{
		uiData = _uiData;
		var uiItemList = itemRoot.GetComponentsInChildren<UIItemGachaItemResult>(true);
		for (int i = 0 ; i < uiItemList.Length ; i++)
		{
			if (_items.Count <= i)
			{
				uiItemList[i].gameObject.SetActive(false);
			}
			else
			{
				uiItemList[i].gameObject.SetActive(true);
				uiItemList[i].OnUpdate(_items[i]);
			}
		}

		gachaButton.OnUpdate(uiData);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
