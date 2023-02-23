using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopMenu : MonoBehaviour
{
	[SerializeField] private Button openButton;
	[SerializeField] private Button closeButton;
	[SerializeField] private Button saveButton;
	[SerializeField] private GameObject menuPanel;

	private void OnEnable()
	{
		openButton.onClick.RemoveAllListeners();
		openButton.onClick.AddListener(OpenMenu);

		saveButton.onClick.RemoveAllListeners();
		saveButton.onClick.AddListener(OnSaveButtonClick);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(CloseMenu);
	}

	private void OpenMenu()
	{
		menuPanel.SetActive(true);
		openButton.gameObject.SetActive(false);
	}

	private void CloseMenu()
	{
		menuPanel.SetActive(false);
		openButton.gameObject.SetActive(true);
	}

	private void OnSaveButtonClick()
	{
		UserInfo.SaveUserData();
	}
}
