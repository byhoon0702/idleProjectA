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
	public Toggle TogglePet => togglePet;
	public Toggle ToggleGacha => toggleGacha;

	private void Awake()
	{
		SetToggle();
	}
	private bool IsNormalStage()
	{
		bool normal = StageManager.it.CheckNormalStage();

		if (!normal)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_use_only_normal_stage"]);
		}
		return normal;
	}

	private void SetToggle()
	{
		toggleHero.onValueChanged.AddListener((_isOn) =>
		{

			if (_isOn == true)
			{
				if (!IsNormalStage())
				{
					toggleHero.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ToggleManagement(() => { toggleHero.isOn = false; });
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
				if (!IsNormalStage())
				{
					toggleEquipment.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ToggleEquipment(onClose: () => { toggleEquipment.isOn = false; });
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
				if (!IsNormalStage())
				{
					toggleRelic.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ToggleRelic(() => { toggleRelic.isOn = false; });
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
				if (!IsNormalStage())
				{
					toggleDungeon.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ShowDungeonList(() => { toggleDungeon.isOn = false; });
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
				if (!IsNormalStage())
				{
					togglePet.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.TogglePet(() => { togglePet.isOn = false; });
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
				if (!IsNormalStage())
				{
					toggleGacha.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ToggleGacha(() => { toggleGacha.isOn = false; });
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
				if (!IsNormalStage())
				{
					toggleShop.isOn = false;
					return;
				}
				GameUIManager.it.Close();
				UIController.it.ToggleShop(() => { toggleShop.isOn = false; });
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
