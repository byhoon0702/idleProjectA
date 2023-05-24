using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;

public class UIManagement : MonoBehaviour
{
	public enum ViewType
	{
		Training,
		Skill,
		Veterancy,
		Advancement,
		Muscle,
		//Relic,
		//Costume,
	}

	[SerializeField] private UITraining trainingUI;

	//[SerializeField] private UIManagementSkill skillUI;
	[SerializeField] private UITraining muscleUI;
	//[SerializeField] private UIManagementRelic relicUI;
	[SerializeField] private UIManagementAdvancement advancementUI;
	[SerializeField] private UIManagementVeterancy veterancyUI;
	//[SerializeField] private UICostumeManagement costumeUI;

	[Header("Button")]
	[SerializeField] private Button trainingButton;
	[SerializeField] private Button skillButton;
	[SerializeField] private Button veterancyButton;
	[SerializeField] private Button youthButton;
	[SerializeField] private Button muscleButton;


	[SerializeField] private RectTransform uiRectTransform;


	private void Awake()
	{
		//closeButton.onClick.RemoveAllListeners();
		//closeButton.onClick.AddListener(Close);

		veterancyButton.onClick.RemoveAllListeners();
		veterancyButton.onClick.AddListener(() => ChangeView(ViewType.Veterancy));
		//skillButton.onClick.RemoveAllListeners();
		//skillButton.onClick.AddListener(() => ChangeView(ViewType.Skill));
		youthButton.onClick.RemoveAllListeners();
		youthButton.onClick.AddListener(() => ChangeView(ViewType.Advancement));
		muscleButton.onClick.RemoveAllListeners();
		muscleButton.onClick.AddListener(() => ChangeView(ViewType.Muscle));
		trainingButton.onClick.RemoveAllListeners();
		trainingButton.onClick.AddListener(() => ChangeView(ViewType.Training));
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
		ChangeView(ViewType.Training);
	}

	public void ChangeView(ViewType _viewType)
	{
		trainingUI.gameObject.SetActive(false);
		//skillUI.gameObject.SetActive(false);
		//muscleUI.gameObject.SetActive(false);
		advancementUI.gameObject.SetActive(false);
		veterancyUI.gameObject.SetActive(false);
		uiRectTransform.offsetMax = new Vector2(0, -130);
		RectTransform root = (transform as RectTransform);
		switch (_viewType)
		{
			case ViewType.Training:
				uiRectTransform.offsetMax = new Vector2(0, -root.sizeDelta.y / 2.5f);
				trainingUI.gameObject.SetActive(true);
				trainingUI.OnUpdate(false);
				break;
			case ViewType.Advancement:
				advancementUI.gameObject.SetActive(true);
				advancementUI.OnUpdate();
				break;
			case ViewType.Veterancy:
				veterancyUI.gameObject.SetActive(true);
				veterancyUI.OnUpdate();
				break;
			case ViewType.Muscle:
				break;

			//case ViewType.Skill:
			//	skillUI.gameObject.SetActive(true);
			//	skillUI.OnUpdate(0, false);
			//	break;
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
