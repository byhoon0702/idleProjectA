using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITraining : MonoBehaviour
{
	[SerializeField] private UIItemTraining itemPrefab;
	[SerializeField] private Transform itemRoot;

	private List<UIItemTraining> items = new List<UIItemTraining>();

	private void OnEnable()
	{
		EventCallbacks.onItemChanged += OnItemChanged;

		SceneCamera.it.ChangeViewPort(true);
	}

	private void OnDisable()
	{
		EventCallbacks.onItemChanged -= OnItemChanged;
		if (SceneCamera.it != null)
		{
			SceneCamera.it.ChangeViewPort(false);
		}
	}

	private void OnItemChanged(List<long> _changedItems)
	{

	}

	public void OnUpdate(bool _refreshGrid)
	{
		UpdateItem();
	}

	public void UpdateItem()
	{
		var list = GameManager.UserDB.training.trainingInfos;
		int countForMake = list.Count - itemRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
			}
		}

		list.Sort((a, b) => { return b.isOpen.CompareTo(a.isOpen); });

		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIItemTraining slot = child.GetComponent<UIItemTraining>();

			var info = list[i];
			slot.OnUpdate(this, info);
		}
	}
}
