using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/AdsBuff/List")]
public class TutorialAdsBuffList : TutorialStep
{
	UIPopupAdBuff uiPopup;
	Transform rect;
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		uiPopup = FindObject<UIPopupAdBuff>();
		rect = uiPopup.UiItemAdBuff[0].ButtonWatchAd.transform;
		if (rect != null)
		{
			TutorialManager.instance.SetPosition(rect as RectTransform);
		}
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		if (rect != null && rect.gameObject.activeInHierarchy == false)
		{
			return Back();
		}
		return next == null ? this : next.Enter(_quest);
	}
}
