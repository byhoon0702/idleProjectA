using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Rendering;
using System.Globalization;

public class UICostumeManagement : UIBase, ISelectListener
{
	[SerializeField] private UIPopupCostumePoint uiPopupCostumePoint;
	[SerializeField] private Button buttonCostumePoint;
	[SerializeField] private Button closeButton;

	[Header("메인 탭")]
	[SerializeField] private Button weaponTab;
	[SerializeField] private Button helmetTab;

	[SerializeField] private Toggle clothTab;
	public Toggle ClothTab => clothTab;
	[SerializeField] private Toggle awakeningTab;
	public Toggle AwakeningTab => awakeningTab;

	[SerializeField] private Transform characterStand;
	[SerializeField] private UICostumeInfo uiCostumeInfo;
	public UICostumeInfo UiCostumeInfo => uiCostumeInfo;

	[Header("리스트")]
	[SerializeField] private UICostumeSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	public long selectedItemTid { get; private set; }
	private RuntimeData.CostumeInfo selectedInfo;
	public CostumeType costumeType { get; private set; }


	private CostumeData itemData;

	private UnitAnimation unitCostume;
	private HyperUnitCostume hyperUnitCostume;

	public void OnClickCostumePoint()
	{
		uiPopupCostumePoint.Show(() =>
		{
			if (costumeType == CostumeType.CHARACTER)
			{
				if (unitCostume != null)
				{
					unitCostume.gameObject.SetActive(true);
				}
			}
			if (costumeType == CostumeType.HYPER)
			{
				if (hyperUnitCostume != null)
				{
					hyperUnitCostume.gameObject.SetActive(true);
				}
			}
		});
		if (unitCostume != null)
		{
			unitCostume.gameObject.SetActive(false);
		}
		if (hyperUnitCostume != null)
		{
			hyperUnitCostume.gameObject.SetActive(false);
		}
	}
	private void Awake()
	{
		buttonCostumePoint.onClick.RemoveAllListeners();
		buttonCostumePoint.onClick.AddListener(OnClickCostumePoint);

		clothTab.onValueChanged.RemoveAllListeners();
		clothTab.onValueChanged.AddListener((isOn) =>
		{
			if (costumeType == CostumeType.CHARACTER)
			{
				return;
			}
			if (isOn)
			{
				OnUpdate(CostumeType.CHARACTER, PlatformManager.UserDB.costumeContainer[CostumeType.CHARACTER].itemTid, false);
				hyperUnitCostume?.gameObject.SetActive(false);
				CreateUnitForUI(PlatformManager.UserDB.costumeContainer[CostumeType.CHARACTER].costume);
			}
		});

		awakeningTab.onValueChanged.RemoveAllListeners();
		awakeningTab.onValueChanged.AddListener((isOn) =>
		{
			if (costumeType == CostumeType.HYPER)
			{
				return;
			}
			if (isOn)
			{
				OnUpdate(CostumeType.HYPER, PlatformManager.UserDB.costumeContainer[CostumeType.HYPER].itemTid, false);
				unitCostume?.gameObject.SetActive(false);
				CreateHyperUnitForUI(PlatformManager.UserDB.costumeContainer[CostumeType.HYPER].costume);
			}
		});
	}

	public void CreateHyperUnitForUI(GameObject costume)
	{
		if (hyperUnitCostume != null)
		{
			Destroy(hyperUnitCostume.gameObject);
			hyperUnitCostume = null;
		}

		var obj = Instantiate(costume);

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		hyperUnitCostume = obj.GetComponent<HyperUnitCostume>();
		hyperUnitCostume.Init();
		hyperUnitCostume.ChangeCostume();

		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	public void CreateUnitForUI(GameObject _costume)
	{
		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}

		var obj = Instantiate(_costume);

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		unitCostume = obj.GetComponent<UnitAnimation>();
		//unitCostume = obj.GetComponent<NormalUnitCostume>();
		//unitCostume.Init();
		//unitCostume.ChangeCostume();

		//UnitFacial unitFacial = obj.GetComponent<UnitFacial>();
		//unitFacial.ChangeFacial(PlatformManager.UserDB.advancementContainer.Info.CostumeIndex);

		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected override void OnDisable()

	{
		base.OnDisable();

		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}
		if (hyperUnitCostume != null)
		{
			Destroy(hyperUnitCostume.gameObject);
			hyperUnitCostume = null;
		}
	}

	public UICostumeSlot Find(int index)
	{

		return itemRoot.GetChild(index).GetComponent<UICostumeSlot>();
	}

	public void OnUpdate(bool _refreshGrid)
	{
		//OnUpdate(costumeType, selectedItemTid, _refreshGrid);
		ChangeCurrentItem(selectedItemTid);
	}
	public void OnUpdate()
	{
		clothTab.SetIsOnWithoutNotify(true);
		OnUpdate(CostumeType.CHARACTER, 0, false);
		CreateUnitForUI(selectedInfo.itemObject.CostumeObject);
	}
	public void OnUpdate(CostumeType _itemType, long _selectedItemTid, bool _refreshGrid)
	{
		selectedItemTid = _selectedItemTid;
		costumeType = _itemType;

		if (selectedItemTid == 0)
		{
			// 선택된 아이템이 없는경우 첫번째 아이템을 강제선택 시킴
			selectedItemTid = DefaultSelectTid();
			selectedInfo = PlatformManager.UserDB.costumeContainer.GetList(costumeType).Find(x => x.Tid == selectedItemTid);
		}

		itemData = null;
		VResult vResult = InitData(selectedItemTid);
		if (vResult.Fail())
		{
			PopAlert.Create(vResult);
			gameObject.SetActive(false);
			return;
		}

		UpdateItems(_refreshGrid);

		var info = PlatformManager.UserDB.costumeContainer.FindCostumeItem(selectedItemTid, costumeType);
		uiCostumeInfo.OnUpdate(info);
	}

	private VResult InitData(long _itemTid)
	{
		VResult result = new VResult();

		itemData = DataManager.Get<CostumeDataSheet>().Get(_itemTid);
		if (itemData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return result.SetOk();
	}


	public void UpdateItems(bool _refresh)
	{
		var list = PlatformManager.UserDB.costumeContainer.GetList(costumeType);
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
			if (info.rawData.hideUI)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UICostumeSlot slot = child.GetComponent<UICostumeSlot>();
			slot.OnUpdate(this, info, () =>
			{
				selectedItemTid = info.Tid;
				selectedInfo = PlatformManager.UserDB.costumeContainer.GetList(costumeType).Find(x => x.Tid == selectedItemTid);

				if (costumeType == CostumeType.HYPER)
				{
					OnUpdate(costumeType, info.Tid, true);
					CreateHyperUnitForUI(selectedInfo.itemObject.CostumeObject);
				}
				else
				{
					OnUpdate(costumeType, info.Tid, true);
					CreateUnitForUI(selectedInfo.itemObject.CostumeObject);
				}
				//UpdateInfo();
			});
			//slot.ShowSlider(true);
		}
	}

	private long DefaultSelectTid()
	{
		return DataManager.Get<CostumeDataSheet>().GetByItemType(costumeType)[0].tid;
	}

	public void ChangeCurrentItem(long _itemTid)
	{
		OnUpdate(costumeType, _itemTid, true);
	}


	public void SetSelectedTid(long tid)
	{
		selectedItemTid = tid;
	}

	public void AddSelectListener(OnSelect callback)
	{

	}

	public void RemoveSelectListener(OnSelect callback)
	{

	}
}
