using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UITopMenu : MonoBehaviour
{

	[SerializeField] private Toggle toggleOnOff;
	[SerializeField] private Button saveButton;
	[SerializeField] private Button stageMoveButton;
	[SerializeField] private GameObject menuPanel;
	[SerializeField]
	private UICostumeManagement uiCostume;

	public void OnValueChange(bool toggle)
	{
		menuPanel.SetActive(toggle);
	}

	public void OnOpenCostume()
	{
		GameUIManager.it.OnClose();
		toggleOnOff.isOn = false;
		uiCostume.gameObject.SetActive(true);
		uiCostume.OnUpdate(CostumeType.WEAPON, 0, false);
	}

	private void OnSaveButtonClick()
	{

	}

	private void OnStageMoveButtonClick()
	{
		UIController.it.ShowMap();
		toggleOnOff.isOn = false;
	}
}
