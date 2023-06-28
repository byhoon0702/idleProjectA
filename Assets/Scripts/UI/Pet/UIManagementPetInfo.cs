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
	//[SerializeField] private TextMeshProUGUI ownedOptionText;

	[SerializeField] private UIItemOptionText[] itemOptions;

	[SerializeField] private GameObject toggleDisableEquip;
	[SerializeField] private GameObject toggleEquip;
	[SerializeField] private GameObject toggleUnEquip;

	[SerializeField] private Button buttonEquip;
	[SerializeField] private Button btnLevelUp;
	[SerializeField] private Button buttonEvolution;
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
		nameValueText.text = info.ItemName;

		UpdateItemLevelupInfo();

	}
	public void UpdateItemLevelupInfo()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < petInfo.equipAbilities.Count; i++)
		{
			string tail = petInfo.equipAbilities[i].tailChar;
			sb.Append($"{petInfo.equipAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{petInfo.equipAbilities[i].GetValue(petInfo.level).ToString()}{tail}</color>");
			sb.Append('\n');
		}
		equipAbliityText.text = $"{sb.ToString()}";

		sb.Clear();
		for (int i = 0; i < petInfo.ownedAbilities.Count; i++)
		{
			string tail = petInfo.ownedAbilities[i].tailChar;
			sb.Append($"{petInfo.ownedAbilities[i].type.ToUIString()}");
			sb.Append($" <color=yellow>{petInfo.ownedAbilities[i].GetValue(petInfo.level).ToString()}{tail}</color>");
			sb.Append('\n');
		}

		ownedAbilityText.text = $"{sb.ToString()}";
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
