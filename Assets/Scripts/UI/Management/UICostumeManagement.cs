using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VIVID;
using UnityEngine.Rendering;
using System.Globalization;

public class UICostumeManagement : UIBase, ISelectListener
{
	//[SerializeField] private UIManagement parentUI;
	[SerializeField] private Button closeButton;

	[Header("메인 탭")]
	[SerializeField] private Button weaponTab;
	[SerializeField] private Button helmetTab;
	[SerializeField] private Button clothTab;
	[SerializeField] private Button awakeningTab;

	[SerializeField] private Transform characterStand;
	[SerializeField] private UICostumeInfo uiCostumeInfo;

	[Header("리스트")]
	[SerializeField] private UICostumeSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	public long selectedItemTid { get; private set; }
	private RuntimeData.CostumeInfo selectedInfo;
	public CostumeType costumeType { get; private set; }


	private CostumeData itemData;

	private NormalUnitCostume unitCostume;
	private HyperUnitCostume hyperUnitCostume;


	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		weaponTab.onClick.RemoveAllListeners();
		weaponTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.WEAPON)
			{
				return;
			}

			OnUpdate(CostumeType.WEAPON, GameManager.UserDB.costumeContainer[CostumeType.WEAPON].itemTid, false);
			hyperUnitCostume?.gameObject.SetActive(false);
			unitCostume?.gameObject.SetActive(true);
			unitCostume?.ChangeCostume();
		});
		helmetTab.onClick.RemoveAllListeners();
		helmetTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.HEAD)
			{
				return;
			}

			OnUpdate(CostumeType.HEAD, GameManager.UserDB.costumeContainer[CostumeType.HEAD].itemTid, false);
			hyperUnitCostume?.gameObject.SetActive(false);
			unitCostume?.gameObject.SetActive(true);
			unitCostume?.ChangeCostume();
		});
		clothTab.onClick.RemoveAllListeners();
		clothTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.BODY)
			{
				return;
			}

			OnUpdate(CostumeType.BODY, GameManager.UserDB.costumeContainer[CostumeType.BODY].itemTid, false);
			hyperUnitCostume?.gameObject.SetActive(false);
			unitCostume?.gameObject.SetActive(true);
			unitCostume?.ChangeCostume();
		});

		awakeningTab.onClick.RemoveAllListeners();
		awakeningTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.HYPER)
			{
				return;
			}

			OnUpdate(CostumeType.HYPER, GameManager.UserDB.costumeContainer[CostumeType.HYPER].itemTid, false);
			unitCostume?.gameObject.SetActive(false);
			CreateHyperUnitForUI(selectedInfo.itemObject.CostumeObject);
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

	public void CreateUnitForUI()
	{
		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}

		var obj = UnitModelPoolManager.it.Get("B/Player/", "player_01_old");

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		unitCostume = obj.GetComponent<NormalUnitCostume>();
		unitCostume.Init();
		unitCostume.ChangeCostume();

		UnitFacial unitFacial = obj.GetComponent<UnitFacial>();
		unitFacial.ChangeFacial(GameManager.UserDB.advancementContainer.Info.CostumeIndex);

		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		CreateUnitForUI();
		//CreateHyperUnitForUI();

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

	public void OnUpdate(bool _refreshGrid)
	{
		//OnUpdate(costumeType, selectedItemTid, _refreshGrid);
		ChangeCurrentItem(selectedItemTid);
	}
	public void OnUpdate(CostumeType _itemType, long _selectedItemTid, bool _refreshGrid)
	{
		selectedItemTid = _selectedItemTid;
		costumeType = _itemType;

		if (selectedItemTid == 0)
		{
			// 선택된 아이템이 없는경우 첫번째 아이템을 강제선택 시킴
			selectedItemTid = DefaultSelectTid();
			selectedInfo = GameManager.UserDB.costumeContainer.GetList(costumeType).Find(x => x.Tid == selectedItemTid);
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

		var info = GameManager.UserDB.costumeContainer.FindCostumeItem(selectedItemTid, costumeType);
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
		var list = GameManager.UserDB.costumeContainer.GetList(costumeType);
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

			child.gameObject.SetActive(true);
			UICostumeSlot slot = child.GetComponent<UICostumeSlot>();

			var info = list[i];
			slot.OnUpdate(this, info, () =>
			{
				selectedItemTid = info.Tid;
				selectedInfo = GameManager.UserDB.costumeContainer.GetList(costumeType).Find(x => x.Tid == selectedItemTid);

				if (costumeType == CostumeType.HYPER)
				{
					CreateHyperUnitForUI(selectedInfo.itemObject.CostumeObject);
				}
				else
				{
					ChangeCurrentItem(selectedItemTid);
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


		var info = GameManager.UserDB.costumeContainer.FindCostumeItem(selectedItemTid, costumeType);
		unitCostume.ChangeCostume(_itemTid, costumeType, info.unlock == false);
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
