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
	[SerializeField] private TextMeshProUGUI ownedBuffLabelText;
	[SerializeField] private TextMeshProUGUI ownedBuffValueText;
	[SerializeField] private TextMeshProUGUI ownedOptionText;

	[SerializeField] private UIItemOptionText[] itemOptions;

	[SerializeField] private GameObject toggleEquip;
	[SerializeField] private TextMeshProUGUI equipText;
	[SerializeField] private GameObject toggleUnEquip;
	[SerializeField] private TextMeshProUGUI unequipText;

	[SerializeField] private Button btnLevelUp;
	[SerializeField] private TextMeshProUGUI levelUpText;
	[SerializeField] private Button btnChangeOption;
	[SerializeField] private TextMeshProUGUI changeOptionText;

	private UIManagementPet parent;
	private bool isEquipped;
	public void OnEquip()
	{
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
		petSlot.OnUpdate(parent, info);
		petSlot.ShowSlider(true);
		toggleEquip.SetActive(petSlot.isEquipped == false);
		toggleUnEquip.SetActive(petSlot.isEquipped);
		nameValueText.text = info.ItemName;

		if (info.OwnedAbilities.Length == 0)
		{
			ownedBuffValueText.text = "";
		}
		else
		{
			for (int i = 0; i < info.OwnedAbilities.Length; i++)
			{
				var ability = info.OwnedAbilities[i];
				string tail = "";
				if (ability.rawData.isPercentage)
				{
					tail = "%";
				}
				string desc = $"{ability.rawData.description} +{ability.GetValue(info.level).ToString("{0:0.##}")} {tail}";
				ownedBuffValueText.text = desc;
			}
		}


		for (int i = 0; i < itemOptions.Length; i++)
		{
			if (i >= info.options.Count - 1)
			{
				itemOptions[i].gameObject.SetActive(false);
				continue;
			}
			itemOptions[i].gameObject.SetActive(true);
			itemOptions[i].OnUpdate(info.options[i].grade, info.options[i].ability.GetValue(1).ToString());
		}
	}
}
