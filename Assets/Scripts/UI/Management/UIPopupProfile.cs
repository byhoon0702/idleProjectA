
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UIPopupProfile : UIBase
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

	[Header("펫")]
	[SerializeField] private UIPetSlot[] petSlots;

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
		CreateUnitForUI();
		UpdateEquipSlot();
		UpdateEquipPet();
		UpdateStatsInfo();
	}

	private VResult InitData()
	{
		VResult result = new VResult();
		return result.SetOk();
	}

	public void UpdateUnitInfo()
	{
		nameText.text = PlatformManager.UserDB.userInfoContainer.userInfo.UserName;
		levelText.text = $"Lv. {PlatformManager.UserDB.userInfoContainer.userInfo.UserLevel}";

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

		var obj = Instantiate(PlatformManager.UserDB.costumeContainer[CostumeType.CHARACTER].costume);

		obj.transform.SetParent(characterStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		uiUnit = obj.GetComponent<UnitAnimation>();
		//var unitCostume = obj.GetComponent<NormalUnitCostume>();
		//unitCostume.Init();
		//unitCostume.ChangeCostume();
		//unitCostume.EquipeWeapon(PlatformManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).item);

		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 5;
		uiUnit.PlayAnimation("idle");
	}
	public void UpdateStatsInfo()
	{
		totalpowerValue.text = PlatformManager.UserDB.UserStats.GetTotalPower().ToString();

		int makeCount = PlatformManager.UserDB.statusDataList.Count - statsInfoGrid.childCount;

		for (int i = 0; i < makeCount; i++)
		{
			GameObject go = Instantiate(cellPrefab.gameObject, statsInfoGrid);
		}

		int index = 0;
		for (int i = 0; i < statsInfoGrid.childCount; i++)
		{
			statsInfoGrid.GetChild(i).gameObject.SetActive(false);
		}
		foreach (var info in PlatformManager.UserDB.statusDataList)
		{
			if (info.type == StatsType.Buff_Gain_Item)
			{
				continue;
			}
			Transform child = statsInfoGrid.GetChild(index);
			child.gameObject.SetActive(true);
			UIStatsInfoCell cell = child.GetComponent<UIStatsInfoCell>();

			var data = PlatformManager.UserDB.UserStats.stats.Find(x => x.type == info.type);
			if (info.isPercentage)
			{
				cell.OnUpdate(info.type.ToUIString(), $"{data.Value.ToFloatingString()}%");
			}
			else
			{
				cell.OnUpdate(info.type.ToUIString(), $"{data.Value.ToString()}");
			}

			index++;
		}
	}
	public void UpdateEquipSlot()
	{
		weaponSlot.OnUpdate(null, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).item, () => { ShowEquipUi(EquipType.WEAPON); });
		weaponSlot.ShowSlider(false);
		weaponSlot.ShowEquipMark(false);
		armorSlot.OnUpdate(null, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.ARMOR).item, () => { ShowEquipUi(EquipType.ARMOR); });
		armorSlot.ShowSlider(false);
		armorSlot.ShowEquipMark(false);
		ringSlot.OnUpdate(null, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.RING).item, () => { ShowEquipUi(EquipType.RING); });
		ringSlot.ShowSlider(false);
		ringSlot.ShowEquipMark(false);
		neckSlot.OnUpdate(null, PlatformManager.UserDB.equipContainer.GetSlot(EquipType.NECKLACE).item, () => { ShowEquipUi(EquipType.NECKLACE); });
		neckSlot.ShowSlider(false);
		neckSlot.ShowEquipMark(false);
	}

	public void UpdateEquipPet()
	{
		for (int i = 0; i < petSlots.Length; i++)
		{

			petSlots[i].OnUpdate(null, PlatformManager.UserDB.petContainer.PetSlots[i].item);
			petSlots[i].ShowEquipMark(false);
			petSlots[i].ShowSlider(false);

		}
	}


	public void ShowEquipUi(EquipType _itemType)
	{
		Close();

		UIController.it.BottomMenu.ToggleEquipment.isOn = true;

		long tid = PlatformManager.UserDB.equipContainer.GetSlot(_itemType).itemTid;

		UIController.it.Equipment.OnUpdate((EquipTabType)_itemType, tid);

	}
}
