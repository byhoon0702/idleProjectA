using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemVeterancy : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;


	private UIManagementVeterancy parent;
	private RuntimeData.VeterancyInfo info;



	private void Awake()
	{
		upgradeButton.repeatCallback = () => AbilityLevelUp();

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
			info.OnClickLevelup += UpdateLevelInfo;
		}
	}
	private void OnDisable()
	{
		if (info != null)
		{
			info.OnClickLevelup -= UpdateLevelInfo;
		}
	}

	public void OnUpdate(UIManagementVeterancy _parent, RuntimeData.VeterancyInfo _uiData)
	{
		parent = _parent;
		info = _uiData;

		icon.sprite = info.icon;
		info.OnClickLevelup -= UpdateLevelInfo;
		info.OnClickLevelup += UpdateLevelInfo;
		UpdateLevelInfo();
	}

	public void OnRefresh()
	{
		UpdateLevelInfo();
	}

	public void UpdateLevelInfo()
	{
		textTitle.text = info.Name;

		string tail = "";
		if (info != null && info.modeType != StatModeType.Flat)
		{
			tail = "%";
		}

		textCurrentStat.text = $"{info.currentValue.ToString("{0:0.##}")}{tail}";
		//textNextStat.text = $"{uiData.nextValue.ToString("{0:0.##}")}{tail}";
		textPrice.text = info.cost.ToString();

		//tmpu_level.text = $"LV. {uiData.level}";
	}

	private void AbilityLevelUp()
	{
		info.ClickLevelup();
		//uiData.LevelupItem(() =>
		//{
		//	Inventory.it.abilityCalculator.GetCalculator(ItemType.Property).Calculate(Inventory.it.Items);
		//	Inventory.it.abilityCalculator.RefreshAbilityTotal();

		//	owner.UpdateItem(true);
		//	owner.UpdateMoney();
		//});
		GameManager.UserDB.UpdateUserStats();

		if (UnitManager.it.Player != null)
		{
			UnitManager.it.Player.PlayLevelupEffect(info.type);
		}

	}
}
