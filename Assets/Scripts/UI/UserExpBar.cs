using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserExpBar : MonoBehaviour
{
	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;
	[SerializeField] private TextMeshProUGUI textLevel;

	private void Start()
	{
		if (GameManager.UserDB != null)
		{
			GameManager.UserDB.userInfoContainer.onExpEarned += OnSlider;
		}
		OnSlider((float)GameManager.UserDB.userInfoContainer.userInfo.CurrentExp / (float)GameManager.UserDB.userInfoContainer.userInfo.ExpForLevelUP);

	}
	private void OnDestroy()
	{
		if (GameManager.UserDB != null)
		{
			GameManager.UserDB.userInfoContainer.onExpEarned -= OnSlider;
		}
	}

	public void OnSlider(float ratio)
	{
		expSlider.value = ratio;
		textLevel.text = $"LV. {GameManager.UserDB.userInfoContainer.userInfo.UserLevel}";
		textExp.text = $"<color=green>EXP {GameManager.UserDB.userInfoContainer.userInfo.CurrentExp} / {GameManager.UserDB.userInfoContainer.userInfo.ExpForLevelUP} ({(ratio * 100f).ToString("F2")}%)";
	}
}
