using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIItemVeterancy : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textLevel;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private GameObject objMax;

	[SerializeField] private RepeatButton upgradeButton;
	public RepeatButton UpgradeButton => upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;


	private UIManagementVeterancy parent;
	private RuntimeData.VeterancyInfo info;
	public RuntimeData.VeterancyInfo VeterancyInfo => info;

	private void Awake()
	{
		upgradeButton.repeatCallback = AbilityLevelUp;

		upgradeButton.onbuttonUp = (isRepeat) =>
		{
			if (isRepeat)
			{
				//animator.SetBool("Pressed", false);
			}
			else
			{
				//animator.SetTrigger("Selected");
				AbilityLevelUp();
			}
		};
	}
	private void OnEnable()
	{
		if (info != null)
		{
			info.OnClickLevelup += OnRefresh;
		}
	}
	private void OnDisable()
	{
		if (info != null)
		{
			info.OnClickLevelup -= OnRefresh;
		}
	}

	public void OnUpdate(UIManagementVeterancy _parent, RuntimeData.VeterancyInfo _uiData)
	{
		parent = _parent;
		info = _uiData;

		icon.sprite = info.icon;
		info.OnClickLevelup -= OnRefresh;
		info.OnClickLevelup += OnRefresh;
		UpdateLevelInfo();
	}

	public void OnRefresh()
	{
		UpdateLevelInfo();
		parent.UpdateMoney();
	}

	public void UpdateLevelInfo()
	{
		textTitle.text = PlatformManager.Language[info.Name];

		string tail = "";


		textLevel.text = $"LV.{info.Level}";
		if (info.rawData.buff.isPercentage)
		{
			textCurrentStat.text = $"{info.currentValue.ToFloatingString()}%";

		}
		else
		{
			textCurrentStat.text = $"{info.currentValue.ToString()}";
		}



		textPrice.text = info.cost.ToString();

		bool check = PlatformManager.UserDB.veterancyContainer.Check(info.cost);
		if (check == false)
		{
			textPrice.color = Color.red;
		}
		else
		{
			textPrice.color = Color.white;
		}
		bool isMax = info.IsMax();
		objMax.SetActive(isMax);
		upgradeButton.gameObject.SetActive(!isMax);

	}

	private bool AbilityLevelUp()
	{
		if (PlatformManager.UserDB.veterancyContainer.Check(info.cost) == false)
		{
			ToastUI.Instance.Enqueue("노련함 포인트가 부족합니다.");
			return false;
		}

		info.ClickLevelup();

		PlatformManager.UserDB.UpdateUserStats();

		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.PlayLevelupEffect(info.type);
		}

		parent.OnUpdate();
		return true;
	}
}
