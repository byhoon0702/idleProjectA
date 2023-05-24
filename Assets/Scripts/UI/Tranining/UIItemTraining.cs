using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class UIItemTraining : MonoBehaviour
{
	[SerializeField] private GameObject objBgHighlight;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;
	[SerializeField] private TextMeshProUGUI tmpu_level;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	[SerializeField] private GameObject objLock;
	[SerializeField] private TextMeshProUGUI textLockMessage;

	[SerializeField] private Image iconImage;

	private UITraining parent;
	private RuntimeData.TrainingInfo trainingInfo;

	public Animator animator;
	private void Awake()
	{
		objBgHighlight.SetActive(false);
		upgradeButton.repeatCallback = () =>
		{
			animator.SetBool("Pressed", true);
			AbilityLevelUp();
		};
		upgradeButton.onbuttonUp = (isRepeat) =>
		{
			if (isRepeat)
			{
				animator.SetBool("Pressed", false);
			}
			else
			{
				animator.SetTrigger("Selected");
				AbilityLevelUp();
			}
		};
	}


	private void OnEnable()
	{
		if (trainingInfo != null)
		{
			trainingInfo.OnClickLevelup += UpdateLevelInfo;
		}
	}
	private void OnDisable()
	{
		if (trainingInfo != null)
		{
			trainingInfo.OnClickLevelup -= UpdateLevelInfo;
		}
	}

	public void OnUpdate(UITraining _parent, RuntimeData.TrainingInfo _uiData)
	{
		parent = _parent;
		trainingInfo = _uiData;

		trainingInfo.OnClickLevelup -= UpdateLevelInfo;
		trainingInfo.OnClickLevelup += UpdateLevelInfo;


		textLockMessage.text = $"{trainingInfo.PreconditionType.ToUIString()}{trainingInfo.PreconditionLevel}";
		UpdateIcon();
		UpdateLevelInfo();
		UpdateButton();
	}

	private void UpdateIcon()
	{
		iconImage.sprite = trainingInfo.icon;
	}

	public void OnRefresh()
	{
		UpdateLevelInfo();
		UpdateButton();
	}

	private void UpdateLevelInfo()
	{
		bool conditionPass = trainingInfo.isOpen;

		objLock.SetActive(conditionPass == false);

		textTitle.text = trainingInfo.type.ToUIString();

		trainingInfo.SetLevel(trainingInfo.Level);

		var baseValue = GameManager.UserDB.GetBaseValue(trainingInfo.type);
		textCurrentStat.text = $"{(baseValue + trainingInfo.currentValue).ToString("{0:0.##}")}{trainingInfo.itemObject.tailChar}";
		textPrice.text = trainingInfo.cost.ToString();

		tmpu_level.text = $"LV. {trainingInfo.Level}";
	}

	private void UpdateButton()
	{
		//bool levelupable = uiData.Levelupable();
		//upgradeButton.SetInteractable(levelupable);
	}

	private void AbilityLevelUp()
	{
		//if (GameManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD).Pay(trainingInfo.cost) == false)
		//{
		//	return;
		//}

		trainingInfo.ClickLevelup();

		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.PlayLevelupEffect(trainingInfo.type);
		}


	}
}
