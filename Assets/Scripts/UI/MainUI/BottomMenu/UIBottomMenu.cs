using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomMenu : MonoBehaviour
{
	[SerializeField] private Toggle managementToggle;
	[SerializeField] private Toggle equipmentToggle;
	[SerializeField] private Toggle juvenescenceToggle;
	[SerializeField] private Toggle skillToggle;
	[SerializeField] private Toggle dungeonToggle;


	[SerializeField] private Toggle[] toggles;

	public Toggle ManagementToggle => managementToggle;
	public Toggle EquipmentToggle => equipmentToggle;
	public Toggle ShopToggle => juvenescenceToggle;
	public Toggle DungeonToggle => dungeonToggle;
	public Toggle SkillToggle => skillToggle;


	private void Awake()
	{
		SetToggle();
	}

	private void SetToggle()
	{
		managementToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleManagement();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});

		equipmentToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleEquipment();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});

		juvenescenceToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleJuvenescence();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});


		dungeonToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ShowDungeonList();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});


		skillToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleSkill();
			}
			else
			{
				GameUIManager.it.OnClose();
				UIController.it.InactiveAllMainUI();
			}
		});
	}

	public void InactivateAllToggle()
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			toggles[i].isOn = false;
		}
	}
}
