using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagementPetInfo : UIManagementBaseInfo<RuntimeData.PetInfo>
{
	[SerializeField] private UIPetSlot petSlot;
	[SerializeField] private TextMeshProUGUI nameLabelText;
	[SerializeField] private TextMeshProUGUI nameValueText;
	[SerializeField] private TextMeshProUGUI ownedBuffLabelText;
	[SerializeField] private TextMeshProUGUI ownedBuffValueText;
	[SerializeField] private TextMeshProUGUI ownedOptionText;

	[SerializeField] private UIItemOptionText[] itemOptions;

	[SerializeField] private GameObject toggleDisableEquip;
	[SerializeField] private TextMeshProUGUI disableEquipText;
	[SerializeField] private GameObject toggleEquip;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private GameObject toggleUnEquip;
	[SerializeField] private TextMeshProUGUI unequipText;

	[SerializeField] private Button buttonEquip;
	[SerializeField] private Button btnLevelUp;
	[SerializeField] private Button buttonEvolution;
	[SerializeField] private TextMeshProUGUI levelUpText;


	private UIManagementEquip parent;
	private bool isEquipped;
	private RuntimeData.PetInfo petInfo;
	private void Awake()
	{

		buttonEquip.onClick.RemoveAllListeners();
		buttonEquip.onClick.AddListener(OnEquip);
		btnLevelUp.onClick.RemoveAllListeners();
		btnLevelUp.onClick.AddListener(OnClickShowLevelUp);

		buttonEvolution.onClick.RemoveAllListeners();
		buttonEvolution.onClick.AddListener(OnClickShowUpgrade);

	}
	public void OnEquip()
	{
		if (petSlot.isEquipped)
		{
			parent.UnEquipPet(petInfo);
		}
		else
		{
			parent.EquipPet(petInfo);
		}
	}

	public override void OnUpdate(UIManagementEquip _parent, RuntimeData.PetInfo info)
	{
		parent = _parent;
		petInfo = info;
		petSlot.OnUpdate(parent, info);
		toggleEquip.SetActive(petSlot.isEquipped == false && info.unlock);
		toggleUnEquip.SetActive(petSlot.isEquipped && info.unlock);
		toggleDisableEquip.SetActive(info.unlock == false);
		nameValueText.text = info.ItemName;

		if (info.ownedAbilities.Count == 0)
		{
			ownedBuffValueText.text = "";
		}
		else
		{
			for (int i = 0; i < info.ownedAbilities.Count; i++)
			{
				var ability = info.ownedAbilities[i];
				string desc = $"{ability.Description()} +{ability.GetValue(info.level).ToString("{0:0.##}")} {ability.tailChar}";

				ownedBuffValueText.text = desc;
			}
		}

	}
	public void OnClickShowLevelUp()
	{
		parent.UiPopupPetLevelup.OnUpdate(parent, petInfo);

	}

	public void OnClickShowUpgrade()
	{
		parent.UiPopupPetEvolution.OnUpdate(parent, petInfo);
	}
	private void OnUpgradeAllButtonClick()
	{

	}

}
