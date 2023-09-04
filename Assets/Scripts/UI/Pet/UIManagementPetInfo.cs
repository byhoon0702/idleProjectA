using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManagementPetInfo : MonoBehaviour
{
	[SerializeField] private UIPetSlot petSlot;
	[SerializeField] private TextMeshProUGUI nameLabelText;
	[SerializeField] private TextMeshProUGUI nameValueText;
	//[SerializeField] private TextMeshProUGUI ownedBuffLabelText;
	[SerializeField] private TextMeshProUGUI ownedAbilityText;
	[SerializeField] private TextMeshProUGUI equipAbliityText;
	[SerializeField] private TextMeshProUGUI textPetSkill;
	//[SerializeField] private TextMeshProUGUI ownedOptionText;

	[SerializeField] private UIItemOptionText[] itemOptions;

	[SerializeField] private GameObject toggleDisableEquip;
	[SerializeField] private GameObject toggleEquip;
	[SerializeField] private GameObject toggleUnEquip;

	[SerializeField] private Button buttonEquip;
	public Button ButtonEquip => buttonEquip;
	[SerializeField] private Button btnLevelUp;
	public Button BtnLevelUp => btnLevelUp;
	[SerializeField] private Button buttonEvolution;
	public Button ButtonEvolution => buttonEvolution;
	[SerializeField] private TextMeshProUGUI levelUpText;


	private UIManagementPet parent;
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
		if (petInfo.unlock == false)
		{
			return;
		}

		if (petSlot.isEquipped)
		{
			parent.UnEquipPet();
		}
		else
		{
			parent.EquipPet();

		}
	}

	public void OnUpdate(UIManagementPet _parent, RuntimeData.PetInfo info)
	{
		parent = _parent;
		petInfo = info;
		petSlot.OnUpdate(parent, info);
		toggleEquip.SetActive(petSlot.isEquipped == false && info.unlock);
		toggleUnEquip.SetActive(petSlot.isEquipped && info.unlock);
		toggleDisableEquip.SetActive(info.unlock == false);



		var data = PlatformManager.CommonData.PetEvolutionLevelDataList[info.evolutionLevel];
		bool canEvolution = info.Count >= data.consumeCount;

		var currencyItem = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.PET_UPGRADE_ITEM);
		IdleNumber value = info.LevelUpNeedCount();
		bool canLevelUp = currencyItem.Value >= value;

		btnLevelUp.interactable = info.unlock && canLevelUp;

		buttonEvolution.interactable = info.unlock && canEvolution;

		nameValueText.text = PlatformManager.Language[info.ItemName];

		UpdateItemLevelupInfo();

	}
	public void UpdateItemLevelupInfo()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < petInfo.equipAbilities.Count; i++)
		{
			string tail = petInfo.equipAbilities[i].tailChar;
			sb.Append($"{petInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{petInfo.equipAbilities[i].GetValue(petInfo.Level).ToString()}{tail}</color>");
			sb.Append('\n');
		}
		equipAbliityText.text = $"{sb.ToString()}";

		sb.Clear();
		for (int i = 0; i < petInfo.ownedAbilities.Count; i++)
		{
			string tail = petInfo.ownedAbilities[i].tailChar;
			sb.Append($"{petInfo.ownedAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{petInfo.ownedAbilities[i].GetValue(petInfo.Level).ToString()}{tail}</color>");
			sb.Append('\n');
		}

		ownedAbilityText.text = $"{sb.ToString()}";

		if (petInfo.PetSkill != null)
		{
			textPetSkill.text = petInfo.PetSkill.Description;
		}
		else
		{
			textPetSkill.text = "";
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
