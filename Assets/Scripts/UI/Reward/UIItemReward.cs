using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIItemReward : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Image bg;
	[SerializeField] private Image icon;
	[SerializeField] private Image grade;
	[SerializeField] private TextMeshProUGUI textCount;

	[SerializeField] private TextMeshProUGUI textChance;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject objStarGroup;
	[SerializeField] private Toggle[] stars;
	//[SerializeField] private Button _button;
	bool isPressed, isHeldButton;
	float touchTime = 0;
	float longTouchBeginTime = 0.4f;
	private System.Action _onClick;
	private AddItemInfo _info;
	public void Set(AddItemInfo info, System.Action onClick = null)
	{
		_info = info;
		info.value.Turncate();
		if (info.value < 1)
		{
			gameObject.SetActive(false);
			return;
		}
		_onClick = onClick;
		grade.enabled = false;


		textCount.text = info.value.ToString();

		bg.sprite = GameUIManager.it.spriteGradeList[(int)info.grade];

		switch (info.category)
		{
			case RewardCategory.Equip:
				grade.enabled = true;
				grade.sprite = GameUIManager.it.spriteGradeFrameList[(int)info.grade];

				break;
		}
		objStarGroup.SetActive(info.starGrade > 0);
		if (info.starGrade > 0)
		{
			for (int i = 0; i < stars.Length; i++)
			{
				if (i < info.starGrade)
				{
					stars[i].isOn = true;
				}
				else
				{
					stars[i].isOn = false;
				}
			}
		}
		icon.sprite = info.iconImage;
		textChance.text = $"{info.chance}%";
		ShowChance(false);

		//	_button.interactable = _onClick != null;
	}

	public void ShowChance(bool isTrue)
	{
		textChance.gameObject.SetActive(isTrue);
	}

	public void SetCountText(string text)
	{
		textCount.text = text;
	}
	public void ShowEffect()
	{
		gameObject.SetActive(true);
		animator.speed = 1;
		animator?.Play("show", 0, 0);
	}

	public void Skip()
	{
		gameObject.SetActive(true);
		animator.speed = 0;
		animator?.Play("show", 0, 1);

	}
	public void OnClick()
	{
		_onClick?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		touchTime = 0;
		isHeldButton = false;
		isPressed = false;
		TooltipManager.Instance.Off();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPressed = true;
	}
	private void Update()
	{
		if (isPressed)
		{
			touchTime += Time.deltaTime;
			if (touchTime > longTouchBeginTime)
			{
				if (isHeldButton == false)
				{
					//ShowTooltip
					TooltipManager.Instance.Tooltip(_info.tid, _info.category).SetPosition(transform as RectTransform);
					isHeldButton = true;
				}
			}
		}
	}


}
