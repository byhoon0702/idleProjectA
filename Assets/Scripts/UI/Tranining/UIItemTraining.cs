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
	[SerializeField] private TextMeshProUGUI textButtonLabel;

	[SerializeField] private RepeatButton upgradeButton;
	public RepeatButton UpgradeButton => upgradeButton;
	[SerializeField] private Button buttonMax;
	[SerializeField] private TextMeshProUGUI textPrice;

	[SerializeField] private GameObject objLock;
	[SerializeField] private TextMeshProUGUI textLockMessage;

	[SerializeField] private Image iconImage;

	private UITraining parent;
	private RuntimeData.TrainingInfo trainingInfo;
	public RuntimeData.TrainingInfo TrainingInfo => trainingInfo;
	private IdleNumber _cost;

	private RuntimeData.CurrencyInfo currency;
	public Animator animator;
	private int levelUpCount = 1;
	private void Awake()
	{
		objBgHighlight.SetActive(false);
		upgradeButton.repeatCallback = AbilityLevelUpRepeat;
		upgradeButton.onbuttonUp = AbilityLevelUp;
	}


	private void OnEnable()
	{
		if (trainingInfo != null)
		{
			trainingInfo.OnClickLevelup += UpdateLevelInfo;
		}
		EventCallbacks.onCurrencyChanged += CurrencyChanged;
	}
	private void OnDisable()
	{
		if (trainingInfo != null)
		{
			trainingInfo.OnClickLevelup -= UpdateLevelInfo;
		}
		EventCallbacks.onCurrencyChanged -= CurrencyChanged;
	}

	public void CurrencyChanged(CurrencyType type)
	{
		if (type != CurrencyType.GOLD)
		{
			return;
		}
		UpdateLevelInfo();
		UpdateButton();
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

		var baseValue = PlatformManager.UserDB.GetBaseValue(trainingInfo.type);
		if (trainingInfo.rawData.buff.isPercentage)
		{
			textCurrentStat.text = $"{(baseValue + trainingInfo.currentValue).ToFloatingString()}{trainingInfo.tailChar}";
		}
		else
		{
			textCurrentStat.text = $"{(baseValue + trainingInfo.currentValue).ToString()}";
		}

		currency = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD);
		_cost = (IdleNumber)0;

		switch (parent.levelupCount)
		{

			case LevelUpCount.MAX:
				{
					_cost = trainingInfo.GetCost(trainingInfo.Level);
					int count = 0;
					IdleNumber totalCost = (IdleNumber)0;
					int level = trainingInfo.Level + count;
					while (totalCost <= currency.Value && level < trainingInfo.rawData.maxLevel)
					{
						var currentCost = trainingInfo.GetCost(level);
						totalCost += currentCost;

						if (totalCost > currency.Value)
						{
							break;
						}
						count++;
						_cost = totalCost;
						level = trainingInfo.Level + count;
					}
					levelUpCount = Mathf.Max(1, count);
				}
				break;
			default:
				{
					if (parent.levelupCount == LevelUpCount.ONE)
					{
						levelUpCount = 1;
					}
					if (parent.levelupCount == LevelUpCount.TEN)
					{
						levelUpCount = 10;
					}
					if (parent.levelupCount == LevelUpCount.HUNDRED)
					{
						levelUpCount = 100;
					}


					for (int i = 0; i < levelUpCount; i++)
					{
						_cost += trainingInfo.GetCost(trainingInfo.Level + i);
					}

				}
				break;
		}
		textButtonLabel.text = $"{PlatformManager.Language["str_ui_reinforce"]} +{levelUpCount}";
		textPrice.text = _cost.ToString();

		tmpu_level.text = $"LV. {trainingInfo.Level}";
	}

	private void UpdateButton()
	{
		if (trainingInfo.isMaxLevel)
		{
			buttonMax.gameObject.SetActive(true);
			upgradeButton.gameObject.SetActive(false);
			return;
		}
		buttonMax.gameObject.SetActive(false);
		upgradeButton.gameObject.SetActive(true);
		bool check = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD).Check(_cost);

		if (check)
		{
			textPrice.color = Color.white;
		}
		else
		{
			textPrice.color = Color.red;
		}

	}
	private void AbilityLevelUp(bool isRepeat)
	{
		if (isRepeat)
		{
			animator.SetBool("Pressed", false);
			return;
		}
		if (currency.Check(_cost) == false)
		{
			ToastUI.Instance.Enqueue("골드가 부족합니다.");

			return;
		}
		if (trainingInfo.isMaxLevel)
		{
			ToastUI.Instance.Enqueue("최대 레벨입니다.");

			return;
		}

		animator.SetTrigger("Selected");

		trainingInfo.ClickLevelup(levelUpCount);

		currency.Pay(_cost);
		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.PlayLevelupEffect(trainingInfo.type);
		}

		UpdateButton();
		parent.Refresh();
	}
	private bool AbilityLevelUpRepeat()
	{
		if (trainingInfo.isMaxLevel)
		{
			ToastUI.Instance.Enqueue("최대 레벨입니다.");
			animator.SetBool("Pressed", false);
			return false;
		}
		if (currency.Check(_cost) == false)
		{
			ToastUI.Instance.Enqueue("골드가 부족합니다.");
			animator.SetBool("Pressed", false);
			return false;
		}

		animator.SetBool("Pressed", true);

		trainingInfo.ClickLevelup(levelUpCount);
		currency.Pay(_cost);
		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.PlayLevelupEffect(trainingInfo.type);
		}

		UpdateButton();
		parent.Refresh();
		return true;
	}
}
