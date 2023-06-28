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

		}
		OnSlider((float)GameManager.UserDB.userInfoContainer.userInfo.CurrentExp / (float)GameManager.UserDB.userInfoContainer.userInfo.ExpForLevelUP);
	}
	private void OnEnable()
	{
		UserInfoContainer.OnExpEarned += OnSlider;
	}
	private void OnDisable()
	{
		UserInfoContainer.OnExpEarned -= OnSlider;
	}
	private void OnDestroy()
	{



	}

	public void OnSlider(float ratio)
	{
		expSlider.value = ratio;
		textLevel.text = $"LV. {GameManager.UserDB.userInfoContainer.userInfo.UserLevel}";
		IdleNumber need = (IdleNumber)GameManager.UserDB.userInfoContainer.userInfo.ExpForLevelUP;
		need.Turncate();
		textExp.text = $"<color=green>EXP {GameManager.UserDB.userInfoContainer.userInfo.CurrentExp.ToString()} / {need.ToString()} ({(ratio * 100f).ToString("F2")}%)";
	}
}
