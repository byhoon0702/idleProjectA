using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserExpBar : MonoBehaviour
{
	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;


	private void Update()
	{
		expSlider.value = UserInfo.expRatio;
		textExp.text = $"{UserInfo.currExp.ToString("N0")} / {UserInfo.nextExp.ToString("N0")} ({(UserInfo.expRatio * 100).ToString("F2")}%)";
	}
}
