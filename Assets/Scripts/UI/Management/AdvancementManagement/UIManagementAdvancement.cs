using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class UIManagementAdvancement : MonoBehaviour
{
	[SerializeField] private Transform pivot;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform itemRoot;

	[SerializeField] private Button buttonAdvancement;
	[SerializeField] private Button buttonCostumeChange;


	private UnitAdvancementInfo currentInfo;
	private UIManagement parent;
	private UnitCostume unitCostume;

	//private void OnEnable()
	//{
	//	//EventCallbacks.onItemChanged += OnItemChanged;

	//	SceneCamera.it.ChangeViewPort(true);
	//}

	//private void OnDisable()
	//{
	//	//EventCallbacks.onItemChanged -= OnItemChanged;
	//	if (SceneCamera.it != null)
	//	{
	//		SceneCamera.it.ChangeViewPort(false);
	//	}
	//}

	public void Init(UIManagement _parent)
	{
		parent = _parent;
	}

	public void OnUpdate()
	{
		currentInfo = GameManager.UserDB.advancementContainer.Info.advancementInfo;
		UpdateInfo();

	}

	public void CreateUnitForUI(string resource)
	{
		if (resource.IsNullOrEmpty())
		{
			return;
		}

		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}
		var obj = UnitModelPoolManager.it.Get("B/Player", resource);

		obj.transform.SetParent(pivot);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;


		unitCostume = obj.GetComponent<UnitCostume>();
		unitCostume.Init();
		unitCostume.ChangeCostume();
		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	public void Refresh()
	{
		for (int i = 0; i < itemRoot.childCount; i++)
		{
			var child = itemRoot.GetChild(i);
			if (child.gameObject.activeInHierarchy == false)
			{
				continue;
			}

			UIItemAdvancement slot = child.GetComponent<UIItemAdvancement>();

			slot.UpdateInfo();
		}
	}

	public void UpdateInfo()
	{
		var list = GameManager.UserDB.advancementContainer.Info.rawData.upgradeInfoList;

		int countForMake = list.Count - itemRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, itemRoot);
			}
		}


		for (int i = 0; i < itemRoot.childCount; i++)
		{

			var child = itemRoot.GetChild(i);
			if (i > list.Count - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			var info = list[i];
			if (info.level == 0)
			{
				continue;
			}
			child.gameObject.SetActive(true);
			UIItemAdvancement slot = child.GetComponent<UIItemAdvancement>();

			slot.OnUpdate(this, info);
		}
	}
}
