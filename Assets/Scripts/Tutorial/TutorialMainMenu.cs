using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/MainMenu")]
public class TutorialMainMenu : TutorialStep
{
	public ContentType type;
	private IUIContent _uicontent;
	private UIBase _uibase;
	private RectTransform rect;
	public override ITutorial Enter(RuntimeData.QuestInfo quest)
	{
		_quest = quest;
		var toggles = GameObject.FindObjectsOfType<UIContentToggle>(true);
		var buttons = GameObject.FindObjectsOfType<UIContentButton>(true);

		foreach (var toggle in toggles)
		{
			var uicontent = toggle as IUIContent;
			if (uicontent.ContentType == type)
			{
				rect = toggle.transform as RectTransform;
				_uicontent = uicontent;
				break;
			}
		}
		if (_uicontent == null)
		{
			foreach (var button in buttons)
			{
				var uicontent = button as IUIContent;
				if (uicontent.ContentType == type)
				{
					rect = button.transform as RectTransform;
					_uicontent = uicontent;
					break;
				}
			}
		}

		var uibases = GameObject.FindObjectsOfType<UIBase>(true);
		foreach (var uibase in uibases)
		{
			if (uibase.ContentType == type)
			{
				_uibase = uibase;
				break;
			}
		}
		TutorialManager.instance.SetPosition(rect);
		return this;

	}
	public override ITutorial Back()
	{
		if (prev == null)
		{
			return this;
		}
		return prev.Enter(_quest);
	}
	public override ITutorial OnUpdate(float time)
	{
		if (_uibase != null && _uibase.gameObject.activeInHierarchy)
		{
			return next == null ? this : next.Enter(_quest);
		}
		TutorialManager.instance.SetPosition(rect);
		return this;
	}
}
