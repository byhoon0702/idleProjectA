using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIProfile : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textUserName;
	[SerializeField] private TextMeshProUGUI textUserLevel;

	[SerializeField] private TextMeshProUGUI combatPower;



	private void Update()
	{
		textUserName.text = $"{UserInfo.UserName}";
		textUserLevel.text = $"LV.{UserInfo.UserLv}";
		combatPower.text = $"{UserInfo.totalCombatPower.ToString()}";
	}
}
