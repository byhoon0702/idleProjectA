using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffLevelupPage : MonoBehaviour
{
	[SerializeField] private Button closeButton;
	[SerializeField] private Transform listParentTransform;
	[SerializeField] private UIBuffItem itemPrefab;

	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(OnClose);
	}

	public void OnUpdate()
	{
		var userBuffDataList = DataManager.Get<BuffItemDataSheet>().GetInfosClone();

		if (listParentTransform.childCount == 0)
		{
			foreach (var item in userBuffDataList)
			{
				UIBuffData uiBuffData = new UIBuffData();
				uiBuffData.Setup(item);

				UIBuffItem uiBuffItem = Instantiate(itemPrefab, listParentTransform);
				uiBuffItem.OnUpdate(uiBuffData);
			}
		}
	}

	private void OnClose()
	{
		gameObject.SetActive(false);
	}
}
