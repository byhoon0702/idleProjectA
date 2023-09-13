using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TooltipManager : MonoBehaviour
{
	public static TooltipManager Instance;
	public RectTransform bound;
	public RectTransform tooltip;

	public TextMeshProUGUI textTitle;
	public TextMeshProUGUI textContext;
	public TextMeshProUGUI textPlace;

	public Camera uiCamera;

	private void Awake()
	{
		Instance = this;
		bound.gameObject.SetActive(false);
		tooltip.gameObject.SetActive(false);
	}
	private void OnDestroy()
	{
		Instance = null;
	}

	public TooltipManager Tooltip(long tid, RewardCategory category)
	{
		string name = "";
		string description = "";
		string getPlace = "";
		switch (category)
		{
			case RewardCategory.Event_Currency:
			case RewardCategory.Currency:
				{
					var item = PlatformManager.UserDB.inventory.FindCurrency(tid);

					name = item.rawData.name;
					description = item.rawData.tooltip;
					getPlace = item.rawData.getPlace;

				}
				break;
			case RewardCategory.RewardBox:
				{
					var item = PlatformManager.UserDB.inventory.GetRewardBox(tid);
					name = item.rawData.name;
					description = item.rawData.tooltip;
					getPlace = item.rawData.getPlace;

				}
				break;
			default:
				return this;
		}

		tooltip.gameObject.SetActive(true);
		textTitle.text = PlatformManager.Language[name];
		textContext.text = PlatformManager.Language[description];
		textPlace.text = PlatformManager.Language[getPlace];
		return this;
	}
	public void SetPosition(RectTransform rect)
	{
		float halfWidth = rect.rect.width / 2f;
		float halfHeight = rect.rect.height / 2f;

		Vector3 position = new Vector3(rect.position.x, rect.position.y, 0);

		Vector3 screenPos = uiCamera.WorldToScreenPoint(position);
		Vector2 viewport = uiCamera.ScreenToViewportPoint(screenPos);


		tooltip.anchorMin = new Vector2(viewport.x, viewport.y);
		tooltip.anchorMax = new Vector2(viewport.x, viewport.y);

		if (viewport.x >= bound.anchorMin.x && viewport.x <= bound.anchorMax.x && viewport.y <= bound.anchorMax.y && viewport.y >= bound.anchorMin.y)
		{
			Vector2 anchorPos = Vector2.zero;
			anchorPos.y += rect.rect.height;
			tooltip.anchoredPosition = anchorPos;
		}
		else
		{
			Vector2 anchorPos = Vector2.zero;
			anchorPos.y += rect.rect.height;

			if (viewport.x < bound.anchorMin.x)
			{
				viewport.x = bound.anchorMin.x;
			}
			if (viewport.x > bound.anchorMax.x)
			{
				viewport.x = bound.anchorMax.x;
			}

			if (viewport.y < bound.anchorMin.y)
			{
				viewport.y = bound.anchorMin.y;
			}
			if (viewport.y > bound.anchorMax.y)
			{
				viewport.y = bound.anchorMax.y;
				anchorPos.y = 0 - rect.rect.height;
			}

			tooltip.anchorMin = new Vector2(viewport.x, viewport.y);
			tooltip.anchorMax = new Vector2(viewport.x, viewport.y);
			tooltip.anchoredPosition = anchorPos;
		}
	}

	public void Off()
	{
		tooltip.gameObject.SetActive(false);
	}

}
