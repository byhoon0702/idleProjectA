using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomMenu : MonoBehaviour
{
	[SerializeField] private Toggle toggleHero;
	[SerializeField] private Toggle toggleEquipment;
	[SerializeField] private Toggle toggleRelic;
	[SerializeField] private Toggle togglePet;

	[SerializeField] private Toggle toggleShop;
	[SerializeField] private Toggle toggleGacha;
	[SerializeField] private Toggle toggleDungeon;

	[SerializeField] private Toggle[] toggles;

	public Toggle ToggleHero => toggleHero;
	public Toggle ToggleEquipment => toggleEquipment;
	public Toggle ShopToggle => toggleShop;
	public Toggle DungeonToggle => toggleDungeon;
	public Toggle SkillToggle => togglePet;


	private void Awake()
	{
		SetToggle();
	}

	private void SetToggle()
	{
		toggleHero.onValueChanged.AddListener((_isOn) =>
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

		toggleEquipment.onValueChanged.AddListener((_isOn) =>
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

		toggleRelic.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleRelic();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});


		toggleDungeon.onValueChanged.AddListener((_isOn) =>
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


		togglePet.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.TogglePet();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});

		toggleGacha.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.ToggleGacha();
			}
			else
			{
				UIController.it.InactiveAllMainUI();
			}
		});
		toggleShop.onValueChanged.AddListener((_isOn) =>
		{
			if (_isOn == true)
			{
				GameUIManager.it.OnClose();
				UIController.it.InactiveAllMainUI();
				ToastUI.it.Enqueue("현재 개발중입니다.");
				//UIController.it.ToggleShop();
				toggleShop.isOn = false;
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
