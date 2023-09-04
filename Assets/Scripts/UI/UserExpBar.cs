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
		var userInfo = PlatformManager.UserDB.userInfoContainer.userInfo;
		OnSlider((float)(userInfo.CurrentExp / userInfo.ExpForLevelUP), userInfo);
	}
	private void OnEnable()
	{
		PlatformManager.UserDB.userInfoContainer.OnExpEarned += OnSlider;
	}
	private void OnDisable()
	{
		PlatformManager.UserDB.userInfoContainer.OnExpEarned -= OnSlider;
	}
	private void OnDestroy()
	{



	}

	public void OnSlider(float ratio, UserInfo userInfo)
	{
		if (userInfo != null)
		{
			expSlider.value = ratio;
			textLevel.text = $"LV. {userInfo.UserLevel}";
			IdleNumber need = (IdleNumber)userInfo.ExpForLevelUP;
			need.Turncate();
			textExp.text = $"<color=green>EXP {userInfo.CurrentExp.ToString()} / {need.ToString()} ({(ratio * 100f).ToString("F2")}%)";
		}
	}
}
