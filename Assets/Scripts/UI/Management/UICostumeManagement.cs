using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VIVID;
using UnityEngine.Rendering;

public class UICostumeManagement : MonoBehaviour, IUIClosable
{
	[SerializeField] private UIManagement parentUI;
	[SerializeField] private Button closeButton;


	[Header("메인 탭")]
	[SerializeField] private Button weaponTab;
	[SerializeField] private Button helmetTab;
	[SerializeField] private Button clothTab;

	[SerializeField] private Transform characterStand;
	[SerializeField] private UICostumeInfo uiCostumeInfo;

	[Header("리스트")]
	[SerializeField] private UIItemCostumeChange itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	public long selectedItemTid { get; private set; }
	public CostumeType costumeType { get; private set; }


	private CostumeData itemData;

	private UnitCostume unitCostume;


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
			unitCostume.ChangeCostume();
			OnUpdate(CostumeType.WEAPON, VGameManager.it.userDB.costumeContainer[CostumeType.WEAPON].itemTid, false);
		});
		helmetTab.onClick.RemoveAllListeners();
		helmetTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.HEAD)
			{
				return;
			}
			unitCostume.ChangeCostume();
			OnUpdate(CostumeType.HEAD, VGameManager.it.userDB.costumeContainer[CostumeType.HEAD].itemTid, false);
		});
		clothTab.onClick.RemoveAllListeners();
		clothTab.onClick.AddListener(() =>
		{
			if (costumeType == CostumeType.BODY)
			{
				return;
			}
			unitCostume.ChangeCostume();
			OnUpdate(CostumeType.BODY, VGameManager.it.userDB.costumeContainer[CostumeType.BODY].itemTid, false);
		});

	}

	public void CreateUnitForUI()
	{
		if (UnitManager.it.Player == null)
		{
			return;
		}

		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}


		GameObject obj = Instantiate(UnitManager.it.Player.unitAnimation.gameObject);

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;


		unitCostume = obj.GetComponent<UnitCostume>();
		unitCostume.Init();
		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	private void OnEnable()
	{
		CreateUnitForUI();
	}
	private void OnDisable()
	{
		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}

	}
	public void OnUpdate(CostumeType _itemType, long _selectedItemTid, bool _refreshGrid)
	{
		selectedItemTid = _selectedItemTid;
		costumeType = _itemType;

		if (selectedItemTid == 0)
		{
			// 선택된 아이템이 없는경우 첫번째 아이템을 강제선택 시킴
			selectedItemTid = DefaultSelectTid();
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


		var info = VGameManager.it.userDB.inventory.FindCostumeItem(selectedItemTid, costumeType);
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

		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemCostumeChange>())
			{
				Destroy(v.gameObject);
			}

			var list = DataManager.Get<CostumeDataSheet>().GetByItemType(costumeType);
			for (int i = 0; i < list.Count; i++)
			{
				//var item = Instantiate(itemPrefab, itemRoot);
				//item.OnUpdate(this, list[i]);
			}

			foreach (var v in VGameManager.it.userDB.inventory[costumeType])
			{
				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, v);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemCostumeChange>())
		{
			v.SetSelect(v.UIData.tid == selectedItemTid);
			v.SetEquipped(v.UIData.tid == VGameManager.it.userDB.costumeContainer[costumeType].itemTid);
		}
	}

	private long DefaultSelectTid()
	{
		return DataManager.Get<CostumeDataSheet>().GetByItemType(costumeType)[0].tid;
	}

	public void ChangeCurrentItem(long _itemTid)
	{
		OnUpdate(costumeType, _itemTid, true);


		var info = VGameManager.it.userDB.inventory.FindCostumeItem(selectedItemTid, costumeType);
		unitCostume.ChangeCostume(_itemTid, costumeType, info.count == 0);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		parentUI.ChangeView(UIManagement.ViewType.Main);
	}
}
