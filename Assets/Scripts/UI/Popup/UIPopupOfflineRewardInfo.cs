using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIPopupOfflineRewardInfo : UIBase
{
	[SerializeField] private TextMeshProUGUI textKillCount;

	protected override void OnEnable()
	{
		base.OnEnable();
		textKillCount.text = "0";

	}
	private void Update()
	{
		int killcount = PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes;
		if (killcount < 30)
		{
			killcount = 30;
		}

		textKillCount.text = $"{killcount}";

	}

}
