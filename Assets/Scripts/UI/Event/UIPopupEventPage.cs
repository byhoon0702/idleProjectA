using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPopupEventPage : UIBase
{
	[SerializeField] private Image imageBanner;
	[SerializeField] private TextMeshProUGUI textContext;

	[SerializeField] private UIPageEventShop uIPageEventShop;
	[SerializeField] private UIPageEventPackage uIPageEventPackage;
	[SerializeField] private UIPageEventDungeon uIPageEventDungeon;

	[SerializeField] private Toggle toggleDungeon;
	[SerializeField] private Toggle toggleShop;
	[SerializeField] private Toggle togglePackage;

	public void OnToggleDungeon(bool isOn)
	{
		uIPageEventDungeon.gameObject.SetActive(isOn);

		if (isOn)
		{
			uIPageEventDungeon.OnUpdate(this);
		}
	}
	public void OnToggleNormalShop(bool isOn)
	{
		uIPageEventShop.gameObject.SetActive(isOn);
		if (isOn)
		{
			uIPageEventShop.OnUpdate(ShopType.NORMAL);
		}

	}
	public void OnTogglePackageShop(bool isOn)
	{
		uIPageEventPackage.gameObject.SetActive(isOn);
		if (isOn)
		{
			uIPageEventPackage.OnUpdate(ShopType.PACKAGE);
		}
	}



	private void Awake()
	{

	}

	protected override void OnActivate()
	{

		var info = PlatformManager.UserDB.eventContainer.GetCurrentEvent();
		toggleDungeon.SetIsOnWithoutNotify(true);
		uIPageEventDungeon.gameObject.SetActive(true);
		uIPageEventDungeon.OnUpdate(this);

		imageBanner.sprite = null;

		System.DateTime startTime;
		System.DateTime.TryParse(info.rawData.startDate, out startTime);

		System.DateTime endTime;
		System.DateTime.TryParse(info.rawData.endDate, out endTime);

		string format = PlatformManager.Language["str_ui_event_period"];
		textContext.text = string.Format(format, startTime.Month, startTime.Day, endTime.Month, endTime.Day);

	}
}
