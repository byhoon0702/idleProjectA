using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using Unity.Physics;

public class UIManagement : UIBase
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
	public UIContentToggle TrainingButton => trainingButton;
	[SerializeField] private UIContentToggle skillButton;
	public UIContentToggle SkillButton => skillButton;
	[SerializeField] private UIContentToggle veterancyButton;
	public UIContentToggle VeterancyButton => veterancyButton;
	[SerializeField] private UIContentToggle advanceButton;
	public UIContentToggle AdvanceButton => advanceButton;
	[SerializeField] private UIContentToggle awakeningButton;
	public UIContentToggle AwakeningButton => awakeningButton;
	[SerializeField] private UIContentToggle costumeButton;
	public UIContentToggle CostumeButton => costumeButton;


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


	public void OnUpdate(ViewType view)
	{
		trainingButton.SetIsOnWithoutNotify(true);
		currentTab = trainingButton;
		ChangeView(view);
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
				costumeUI.OnUpdate();
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

	protected override void OnClose()
	{
		UIController.it.InactivateAllBottomToggle();
		base.OnClose();
	}
}
