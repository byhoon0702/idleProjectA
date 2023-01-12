using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuTab : MonoBehaviour
{
	[SerializeField] private Button openButton;
	[SerializeField] private Button closeButton;

	[SerializeField] private GameObject closedMenu;
	[SerializeField] private GameObject openMenu;

	private void Start()
	{
		openButton.onClick.RemoveAllListeners();
		openButton.onClick.AddListener(Open);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}

	private void Open()
	{
		openMenu.SetActive(true);
		closedMenu.SetActive(false);
	}

	private void Close()
	{
		openMenu.SetActive(false);
		closedMenu.SetActive(true);
	}
}
