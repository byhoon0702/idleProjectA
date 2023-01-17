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
		textUserName.text = $"{UserInfo.userName}";
		textUserLevel.text = $"LV.{UserInfo.userLv}";
		combatPower.text = $"{UserInfo.totalCombatPower.ToString()}";
	}
}
