
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UIManagementMain : MonoBehaviour, IUIClosable
{

	[SerializeField] private UIManagementEquip uiequipChange;

	[Header("캐릭터")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI levelText;

	[SerializeField] private RectTransform characterStand;

	[Header("장비")]
	[SerializeField] private UIEquipSlot weaponSlot;
	[SerializeField] private UIEquipSlot armorSlot;
	[SerializeField] private UIEquipSlot ringSlot;
	[SerializeField] private UIEquipSlot neckSlot;

	[SerializeField] private Transform statsInfoGrid;
	[SerializeField] private UIStatsInfoCell cellPrefab;

	private UnitItemData itemData;
	private UnitData unitData;

	[SerializeField] private TextMeshProUGUI totalpowerLabel;
	[SerializeField] private TextMeshProUGUI totalpowerValue;

	private UnitAnimation uiUnit;



	public void OnUpdate()
	{
		VResult vResult = InitData();
		if (vResult.Fail())
		{
			PopAlert.Create(vResult);
			gameObject.SetActive(false);
			return;
		}

		gameObject.SetActive(true);
		UpdateUnitInfo();
		UpdateEquipSlot();

		UpdateStatsInfo();
	}

	private VResult InitData()
	{
		VResult result = new VResult();

		//itemData = DataManager.Get<UnitItemDataSheet>().Get(UserInfo.equip.EquipUnitItemTid);
		//if (itemData == null)
		//{
		//	return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. selectedUnitTid: {UserInfo.equip.EquipUnitItemTid}");
		//}

		//unitData = DataManager.Get<UnitDataSheet>().GetData(itemData.unitTid);
		//if (unitData == null)
		//{
		//	return result.SetFail(VResultCode.NO_META_DATA, $"UnitDataSheet. itemData.unitTid: {itemData.unitTid}");
		//}


		return result.SetOk();
	}

	public void UpdateUnitInfo()
	{

		CreateUnitForUI();
	}

	public void CreateUnitForUI()
	{
		if (UnitManager.it.Player == null)
		{
			return;
		}

		if (uiUnit != null)
		{
			Destroy(uiUnit.gameObject);
			uiUnit = null;
		}


		GameObject obj = Instantiate(UnitManager.it.Player.unitAnimation.gameObject);

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;


		uiUnit = obj.GetComponent<UnitAnimation>();
		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}
	public void UpdateStatsInfo()
	{
		//totalpowerValue.text = UserInfo.totalCombatPower.ToString();

		//int makeCount = GameManager.UserDB.abilityinfos.Count - statsInfoGrid.childCount;

		//for (int i = 0; i < makeCount; i++)
		//{
		//	GameObject go = Instantiate(cellPrefab.gameObject, statsInfoGrid);
		//}

		//int index = 0;
		//foreach (var info in GameManager.UserDB.abilityinfos)
		//{
		//	Transform child = statsInfoGrid.GetChild(index);
		//	UIStatsInfoCell cell = child.GetComponent<UIStatsInfoCell>();
		//	var ability = info.Value;

		//	string tail = "";
		//	if (ability.rawData.isPercentage)
		//	{
		//		tail = "%";
		//	}
		//	ability.UpdateValue();
		//	cell.OnUpdate(ability.rawData.description, $"{ability.GetValue().ToString("{0:0.##}")} {tail}");
		//	index++;
		//}
	}
	public void UpdateEquipSlot()
	{
		weaponSlot.OnUpdate(null, GameManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).item, () => { ShowEquipUi(EquipType.WEAPON); });
		armorSlot.OnUpdate(null, GameManager.UserDB.equipContainer.GetSlot(EquipType.ARMOR).item, () => { ShowEquipUi(EquipType.ARMOR); });
		ringSlot.OnUpdate(null, GameManager.UserDB.equipContainer.GetSlot(EquipType.RING).item, () => { ShowEquipUi(EquipType.RING); });
		neckSlot.OnUpdate(null, GameManager.UserDB.equipContainer.GetSlot(EquipType.NECKLACE).item, () => { ShowEquipUi(EquipType.NECKLACE); });
	}


	public void ShowEquipUi(EquipType _itemType)
	{
		Close();

		UIController.it.BottomMenu.EquipmentToggle.isOn = true;

		long tid = GameManager.UserDB.equipContainer.GetSlot(_itemType).itemTid;

		UIController.it.Equipment.OnUpdate((EquipTabType)_itemType, tid);

	}

	void OnEnable()
	{
		AddCloseListener();
	}
	void OnDisable()
	{
		RemoveCloseListener();
	}
	public void AddCloseListener()
	{
		GameUIManager.it.onClose += Close;
	}

	public void RemoveCloseListener()
	{
		GameUIManager.it.onClose -= Close;
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
