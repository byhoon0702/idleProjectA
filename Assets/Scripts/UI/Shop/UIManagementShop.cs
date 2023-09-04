using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopType
{
	NORMAL,
	PACKAGE,
	SALE,
	DIA,
	ADS,
	BATTLEPASS,

}


public class UIManagementShop : UIBase
{
	[SerializeField] private UIShopPackage uiShopPackage;
	[SerializeField] private UIShopNormal uiShopNormal;
	[SerializeField] private UIShopDia uiShopDia;

	[SerializeField] private Toggle togglePackage;
	[SerializeField] private Toggle toggleSale;
	[SerializeField] private Toggle toggleDia;
	[SerializeField] private Toggle toggleNormal;
	[SerializeField] private Toggle toggleAds;



	private ShopType _currentType;
	private void Awake()
	{
		togglePackage.onValueChanged.RemoveAllListeners();
		togglePackage.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				ShowShopPage(ShopType.PACKAGE);
			}
		});

		toggleSale.onValueChanged.RemoveAllListeners();
		toggleSale.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				ShowShopPage(ShopType.SALE);
			}
		});

		toggleDia.onValueChanged.RemoveAllListeners();
		toggleDia.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				ShowShopPage(ShopType.DIA);
			}
		});

		toggleNormal.onValueChanged.RemoveAllListeners();
		toggleNormal.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				ShowShopPage(ShopType.NORMAL);
			}
		});

		toggleAds.onValueChanged.RemoveAllListeners();
		toggleAds.onValueChanged.AddListener((isTrue) =>
		{
			if (isTrue)
			{
				ShowShopPage(ShopType.ADS);
			}
		});
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ShopContainer.AddEvent(Refresh);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		ShopContainer.RemoveEvent(Refresh);
	}

	private void Refresh()
	{
		OnUpdate(_currentType);
	}

	public void ShowShopPage(ShopType type)
	{
		_currentType = type;
		uiShopPackage.gameObject.SetActive(false);
		uiShopDia.gameObject.SetActive(false);
		uiShopNormal.gameObject.SetActive(false);
		switch (type)
		{
			case ShopType.PACKAGE:
			case ShopType.SALE:
				uiShopPackage.gameObject.SetActive(true);
				uiShopPackage.OnUpdate(type);
				break;

			case ShopType.DIA:
				uiShopDia.gameObject.SetActive(true);
				uiShopDia.OnUpdate(type);
				break;

			case ShopType.NORMAL:
			case ShopType.ADS:
				uiShopNormal.gameObject.SetActive(true);
				uiShopNormal.OnUpdate(type);
				break;
		}
	}

	public void OnUpdate(ShopType type)
	{
		switch (type)
		{
			case ShopType.PACKAGE:
				togglePackage.SetIsOnWithoutNotify(true);
				break;
			case ShopType.SALE:
				toggleSale.SetIsOnWithoutNotify(true);
				break;
			case ShopType.DIA:
				toggleDia.SetIsOnWithoutNotify(true);
				break;
			case ShopType.NORMAL:
				toggleNormal.SetIsOnWithoutNotify(true);
				break;
			case ShopType.ADS:
				toggleAds.SetIsOnWithoutNotify(true);
				break;
		}

		ShowShopPage(type);
	}
}
