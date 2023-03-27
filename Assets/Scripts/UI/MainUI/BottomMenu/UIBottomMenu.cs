using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomMenu : MonoBehaviour
{
	[SerializeField] private Toggle managementToggle;
	[SerializeField] private Toggle equipmentToggle;
	[SerializeField] private Toggle skillToggle;
	[SerializeField] private Toggle petToggle;
	[SerializeField] private Toggle dungeonListToggle;
	[SerializeField] private Toggle gachaToggle;

	[SerializeField] private Toggle[] toggles;

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
				UIController.it.ToggleEquipment();
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
				UIController.it.ToggleSkill();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});


		petToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				UIController.it.TogglePet();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});

		dungeonListToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				UIController.it.ToggleDungeonList();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});


		gachaToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				UIController.it.ToggleGacha();
			}
			else
			{
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
