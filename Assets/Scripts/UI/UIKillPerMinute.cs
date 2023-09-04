using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIKillPerMinute : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textCount;
	[SerializeField] private TextMeshProUGUI textKillCount;
	[SerializeField] private Slider slider;
	public void OnClickDetail()
	{

	}

	private void Update()
	{
		if (StageManager.it.killCountForOffline < 1000)
		{
			if (slider.gameObject.activeInHierarchy == false)
			{
				slider.gameObject.SetActive(true);
			}

			textKillCount.text = $"{StageManager.it.killCountForOffline}/1000";
			slider.value = StageManager.it.killCountForOffline / 1000f;
		}
		else
		{
			if (slider.gameObject.activeInHierarchy)
			{
				slider.gameObject.SetActive(false);
			}
		}
		int killcount = PlatformManager.UserDB.userInfoContainer.userInfo.KillPerMinutes;
		if (killcount < 30)
		{
			killcount = 30;
		}
		textCount.text = $"{killcount}";
	}

}
