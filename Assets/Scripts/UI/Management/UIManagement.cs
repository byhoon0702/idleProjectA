using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour, IUIClosable
{
	public enum ViewType
	{
		Main,
		Skill,
		Relic,
		Drive,
		Property,
	}


	[SerializeField] private UIManagementMain mainUI;
	[SerializeField] private UIManagementSkill skillUI;
	[SerializeField] private UIManagementRelic relicUI;
	[SerializeField] private UIManagementDrive driveUi;
	[SerializeField] private UIManagementProperty propertyUI;

	[Header("Button")]
	[SerializeField] private Button closeButton;

	[SerializeField] private Button mainButton;
	[SerializeField] private Button skillButton;
	[SerializeField] private Button relicButton;
	[SerializeField] private Button driveButton;
	[SerializeField] private Button propertyButton;



	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);

		mainButton.onClick.RemoveAllListeners();
		mainButton.onClick.AddListener(() => ChangeView(ViewType.Main));
		skillButton.onClick.RemoveAllListeners();
		skillButton.onClick.AddListener(() => ChangeView(ViewType.Skill));
		relicButton.onClick.RemoveAllListeners();
		relicButton.onClick.AddListener(() => ChangeView(ViewType.Relic));
		driveButton.onClick.RemoveAllListeners();
		driveButton.onClick.AddListener(() => ChangeView(ViewType.Drive));
		propertyButton.onClick.RemoveAllListeners();
		propertyButton.onClick.AddListener(() => ChangeView(ViewType.Property));
	}


	private void OnEnable()
	{
		UIController.it.SetCoinEffectActivate(false);
	}

	private void OnDisable()
	{
		UIController.it.SetCoinEffectActivate(true);
		UserInfo.SaveUserData();
	}

	public void OnUpdate()
	{
		ChangeView(ViewType.Main);
	}

	public void ChangeView(ViewType _viewType)
	{
		mainUI.gameObject.SetActive(false);
		skillUI.gameObject.SetActive(false);
		relicUI.gameObject.SetActive(false);
		driveUi.gameObject.SetActive(false);
		propertyUI.gameObject.SetActive(false);

		switch (_viewType)
		{
			case ViewType.Main:
				mainUI.gameObject.SetActive(true);
				mainUI.OnUpdate();
				break;
			case ViewType.Skill:
				skillUI.gameObject.SetActive(true);
				skillUI.OnUpdate(false);
				break;

			case ViewType.Relic:
				relicUI.gameObject.SetActive(true);
				relicUI.OnUpdate(false);
				break;

			case ViewType.Drive:
				driveUi.gameObject.SetActive(true);
				driveUi.OnUpdate();
				break;

			case ViewType.Property:
				propertyUI.gameObject.SetActive(true);
				propertyUI.OnUpdate(false);
				break;
		}
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
