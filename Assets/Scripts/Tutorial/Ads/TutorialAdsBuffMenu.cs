using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/AdsBuff/Menu")]
public class TutorialAdsBuffMenu : TutorialStep
{
	UIPopupAdBuff uiAdbuff;
	UIContentButton button;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiAdbuff = FindObject<UIPopupAdBuff>();
		UIContentButton[] buttons = GameObject.FindObjectsOfType<UIContentButton>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].ContentType == ContentType.ADS_BUFF)
			{
				button = buttons[i];
				break;
			}
		}

		TutorialManager.instance.SetPosition(button.transform);
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (uiAdbuff != null && uiAdbuff.gameObject.activeInHierarchy)
		{
			return next == null ? this : next.Enter(_quest);
		}
		return this;
	}
}
