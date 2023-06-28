using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using Unity.Physics;

public class UIManagement : MonoBehaviour
{
	public enum ViewType
	{
		Training,
		Skill,
		Veterancy,
		Advancement,
		Awakening,
		Costume,
	}

	[SerializeField] private UITraining trainingUI;

	[SerializeField] private UIManagementSkill skillUI;

	[SerializeField] private UIManagementAdvancement advancementUI;
	[SerializeField] private UIManagementVeterancy veterancyUI;
	[SerializeField] private UICostumeManagement costumeUI;
	[SerializeField] private UIManagementAwakening awakeningUI;

	[Header("Button")]
	[SerializeField] private UIContentToggle trainingButton;
	[SerializeField] private UIContentToggle skillButton;
	[SerializeField] private UIContentToggle veterancyButton;
	[SerializeField] private UIContentToggle advanceButton;
	[SerializeField] private UIContentToggle awakeningButton;
	[SerializeField] private UIContentToggle costumeButton;


	[SerializeField] private RectTransform uiRectTransform;

	private Toggle currentTab;
	private void Awake()
	{
		veterancyButton.onValueChanged.RemoveAllListeners();
		veterancyButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (veterancyButton.IsAvailable() == false)
				{
					veterancyButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = veterancyButton;
				ChangeView(ViewType.Veterancy);
			}
		});
		skillButton.onValueChanged.RemoveAllListeners();
		skillButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (skillButton.IsAvailable() == false)
				{
					skillButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = skillButton;
				ChangeView(ViewType.Skill);
			}
		});
		advanceButton.onValueChanged.RemoveAllListeners();
		advanceButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (advanceButton.IsAvailable() == false)
				{
					advanceButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = advanceButton;
				ChangeView(ViewType.Advancement);
			}
		});
		awakeningButton.onValueChanged.RemoveAllListeners();
		awakeningButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (awakeningButton.IsAvailable() == false)
				{
					awakeningButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = awakeningButton;
				ChangeView(ViewType.Awakening);
			}
		});
		trainingButton.onValueChanged.RemoveAllListeners();
		trainingButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (trainingButton.IsAvailable() == false)
				{
					trainingButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = trainingButton;
				ChangeView(ViewType.Training);
			}
		});
		costumeButton.onValueChanged.RemoveAllListeners();
		costumeButton.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				if (costumeButton.IsAvailable() == false)
				{
					costumeButton.SetIsOnWithoutNotify(false);
					currentTab?.SetIsOnWithoutNotify(true);
					return;
				}
				currentTab = costumeButton;
				ChangeView(ViewType.Costume);
			}
		});
	}


	private void OnEnable()
	{
		UIController.it.SetCoinEffectActivate(false);
	}

	private void OnDisable()
	{
		UIController.it.SetCoinEffectActivate(true);
	}

	public void OnUpdate()
	{
		trainingButton.SetIsOnWithoutNotify(true);
		currentTab = trainingButton;
		ChangeView(ViewType.Training);
	}

	public void ChangeView(ViewType _viewType)
	{
		RectTransform root = (transform as RectTransform);
		trainingUI.gameObject.SetActive(false);
		skillUI.gameObject.SetActive(false);
		costumeUI.gameObject.SetActive(false);
		advancementUI.gameObject.SetActive(false);
		veterancyUI.gameObject.SetActive(false);
		awakeningUI.gameObject.SetActive(false);
		//uiRectTransform.offsetMax = new Vector2(0, -(130 + root.offsetMin.y));

		//root.offsetMax
		switch (_viewType)
		{
			case ViewType.Training:
				//uiRectTransform.offsetMax = new Vector2(0, -root.rect.height * 0.6f);
				trainingUI.gameObject.SetActive(true);
				trainingUI.OnUpdate(false);
				break;
			case ViewType.Advancement:
				//uiRectTransform.offsetMax = new Vector2(0, -root.rect.height * 0.5f);
				advancementUI.gameObject.SetActive(true);
				advancementUI.OnUpdate();
				break;
			case ViewType.Veterancy:
				veterancyUI.gameObject.SetActive(true);
				veterancyUI.OnUpdate();
				break;
			case ViewType.Awakening:
				awakeningUI.gameObject.SetActive(true);
				awakeningUI.OnUpdate();
				break;
			case ViewType.Costume:
				costumeUI.gameObject.SetActive(true);
				costumeUI.OnUpdate(CostumeType.WEAPON, 0, false);
				break;

			case ViewType.Skill:
				skillUI.gameObject.SetActive(true);
				skillUI.OnUpdate(0);
				break;
			default:
				trainingUI.gameObject.SetActive(true);
				trainingUI.OnUpdate(false);
				break;
		}
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		UIController.it.InactivateAllBottomToggle();
		gameObject.SetActive(false);
	}
}
