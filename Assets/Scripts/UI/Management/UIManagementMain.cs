using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagementMain : MonoBehaviour
{
	[SerializeField] private UIUnitChange uiUnitChange;
	[SerializeField] private UIEquipChange uiequipChange;


	[Header("캐릭터")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI gradeText;
	[SerializeField] private TextMeshProUGUI toOwnText;
	[SerializeField] private TextMeshProUGUI levelText;

	[Header("장비")]
	[SerializeField] private EquipSlot weaponSlot;
	[SerializeField] private EquipSlot armorSlot;
	[SerializeField] private EquipSlot accSlot;

	[Header("스킬")]
	[SerializeField] private UImanagementUnitSkillInfo unitSkill;
	[SerializeField] private UImanagementUnitSkillInfo finalSkill;

	[Header("ETC")]
	[SerializeField] private Button editCharButton;


	private ItemData itemData;
	private UnitData unitData;
	private SkillData unitSkillData;
	private SkillData finalSkillData;



	private void Awake()
	{
		editCharButton.onClick.RemoveAllListeners();
		editCharButton.onClick.AddListener(OnEditUnit);
	}

	public void OnUpdate()
	{
		VResult vResult = InitData();
		if(vResult.Fail())
		{
			PopAlert.it.Create(vResult);
			gameObject.SetActive(false);
			return;
		}

		UpdateUnitInfo();
		UpdateEquipSlot();
		UpdateSkill();
	}

	private VResult InitData()
	{
		VResult result = new VResult();

		itemData = DataManager.Get<ItemDataSheet>().Get(UserInfo.EquipUnitItemTid);
		if(itemData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. selectedUnitTid: {UserInfo.EquipUnitItemTid}");
		}

		unitData = DataManager.Get<UnitDataSheet>().GetData(itemData.unitTid);
		if(unitData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"UnitDataSheet. itemData.unitTid: {itemData.unitTid}");
		}

		unitSkillData = DataManager.Get<SkillDataSheet>().Get(unitData.skillTid);
		if(unitSkillData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet. skillTid: {unitData.skillTid}");
		}

		finalSkillData = DataManager.Get<SkillDataSheet>().Get(unitData.finalSkillTid);
		if (finalSkillData == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet. finalSkillTid: {unitData.finalSkillTid}");
		}

		return result.SetOk();
	}

	public void UpdateUnitInfo()
	{
		var item = Inventory.it.FindItemByTid(itemData.tid);

		nameText.text = item.ItemName;
		gradeText.text = itemData.itemGrade.ToString();
		toOwnText.text = itemData.ToOwnAbilityInfo.ToString();
		levelText.text = $"Lv. {item.Level}";

		icon.sprite = Resources.Load<Sprite>($"Icon/{itemData.Icon}");
	}

	public void UpdateEquipSlot()
	{
		weaponSlot.OnUpdate(this, ItemType.Weapon, UserInfo.EquipWeaponTid);
		armorSlot.OnUpdate(this, ItemType.Armor, UserInfo.EquipArmorTid);
		accSlot.OnUpdate(this, ItemType.Accessory, UserInfo.EquipAccessoryTid);
	}

	public void UpdateSkill()
	{
		unitSkill.OnUpdate(unitSkillData);
		finalSkill.OnUpdate(finalSkillData);
	}

	private void OnEditUnit()
	{
		var itemData = DataManager.Get<ItemDataSheet>().Get(UserInfo.EquipUnitItemTid);
		uiUnitChange.gameObject.SetActive(true);
		uiUnitChange.OnUpdate(itemData.tid, false);
	}

	public void ShowEquipUi(ItemType _itemType)
	{
		switch (_itemType)
		{
			case ItemType.Weapon:
				uiequipChange.gameObject.SetActive(true);
				uiequipChange.OnUpdate(ItemType.Weapon, UserInfo.EquipWeaponTid, false);
				break;
			case ItemType.Armor:
				uiequipChange.gameObject.SetActive(true);
				uiequipChange.OnUpdate(ItemType.Armor, UserInfo.EquipArmorTid, false);
				break;
			case ItemType.Accessory:
				uiequipChange.gameObject.SetActive(true);
				uiequipChange.OnUpdate(ItemType.Accessory, UserInfo.EquipAccessoryTid, false);
				break;
		}
	}
}
