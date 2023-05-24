using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIItemYouth : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI abilityText;
	[SerializeField] private TextMeshProUGUI textLevel;

	[SerializeField] private RepeatButton buttonUpgrade;
	[SerializeField] private TextMeshProUGUI textButtonUpgrade;


	private UIManagementYouth parent;

	private RuntimeData.YouthInfo info;

	private void Awake()
	{
		buttonUpgrade.repeatCallback = OnClickUpgrade;
		buttonUpgrade.onbuttonUp = (isRepeat) =>
		{
			if (isRepeat)
			{

			}
			else
			{

				OnClickUpgrade();
			}
		};

	}

	private void OnEnable()
	{


	}
	private void OnDisable()
	{

		info?.RemoveEvent(UpdateInfo);
	}
	public void OnUpdate(UIManagementYouth _owner, RuntimeData.YouthInfo _info)
	{
		parent = _owner;
		info = _info;
		info.SetDirty();
		info.AddEvent(UpdateInfo);
		UpdateInfo();
	}

	public void UpdateInfo()
	{
		textLevel.text = $"LV. {info.Level}";
		abilityText.text = $"{info.type.ToUIString()} +{info.currentValue.ToString()}%";
	}


	public void OnClickUpgrade()
	{
		if (info.IsMax())
		{
			return;
		}
		info.ClickLevelup();

		GameManager.UserDB.UpdateUserStats();

		//if (UnitManager.it.Player != null)
		//{
		//	UnitManager.it.Player.PlayLevelupEffect(info.type);
		//}
	}
}
