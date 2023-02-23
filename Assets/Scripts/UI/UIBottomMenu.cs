using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomMenu : MonoBehaviour
{
	[SerializeField] private Button managementButton;
	[SerializeField] private Button petButton;
	[SerializeField] private Button gachaButton;


	private void Awake()
	{
		managementButton.onClick.RemoveAllListeners();
		managementButton.onClick.AddListener(OnManagementButtonClick);
		petButton.onClick.RemoveAllListeners();
		petButton.onClick.AddListener(OnPetButtonClick);
		gachaButton.onClick.RemoveAllListeners();
		gachaButton.onClick.AddListener(OnGachaButtonClick);
	}

	private void OnManagementButtonClick()
	{
		UIController.it.ToggleManagement();
	}

	private void OnPetButtonClick()
	{
		UIController.it.TogglePet();
	}
	private void OnGachaButtonClick()
	{
		UIController.it.ToggleGacha();
	}
}
