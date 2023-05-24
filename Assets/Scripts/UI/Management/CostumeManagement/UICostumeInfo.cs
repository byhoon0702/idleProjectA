using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RuntimeData;
using System.Net.Http.Headers;

public class UICostumeInfo : MonoBehaviour
{
	[SerializeField] private UICostumeManagement parentUI;

	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI upgradeItemOwnedCount;
	[SerializeField] private EffectInfoCell[] cells;

	[SerializeField] private Button upgradeButton;
	[SerializeField] private TextMeshProUGUI upgradeButtonLabel;
	[SerializeField] private TextMeshProUGUI upgradeButtonCost;

	[SerializeField] private Button equipButton;
	[SerializeField] private TextMeshProUGUI equipButtonLabel;

	private RuntimeData.CostumeInfo costumeInfo;

	public void OnEnable()
	{
		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(() =>
		{
			if (costumeInfo.unlock == false)
			{
				return;
			}
			GameManager.UserDB.costumeContainer.Equip(parentUI.selectedItemTid, parentUI.costumeType);
			if (UnitManager.it.Player != null)
			{
				UnitManager.it.Player.ChangeCostume();
			}
			parentUI.OnUpdate(false);

		});

		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(() =>
		{
			costumeInfo.unlock = true;
			parentUI.OnUpdate(false);

		});
	}

	public void OnUpdate(RuntimeData.CostumeInfo info)
	{
		costumeInfo = info;
		itemName.text = costumeInfo.ItemName;

		for (int i = 0; i < cells.Length; i++)
		{
			if (i >= costumeInfo.ownedAbilities.Count - 1)
			{
				cells[i].gameObject.SetActive(false);
				continue;
			}

			var ability = costumeInfo.ownedAbilities[i];
			cells[i].gameObject.SetActive(true);

			string value = ability.GetValue(costumeInfo.level).ToString();
			if (ability.modeType != StatModeType.Flat)
			{
				value = $"+ {value} %";
			}
			cells[i].OnUpdate(ability.Description(), value);
		}

		upgradeButtonCost.text = "무료";

		equipButton.interactable = costumeInfo.unlock;
		equipButton.gameObject.SetActive(costumeInfo.unlock);
		upgradeButton.gameObject.SetActive(costumeInfo.unlock == false);

	}
}
