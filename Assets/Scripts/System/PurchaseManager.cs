using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using UnityEngine.Purchasing.Security;
public class PurchaseManager : MonoBehaviour, IDetailedStoreListener
{
	public static PurchaseManager Instance { get; private set; }

	public IStoreController StoreController;
	public IExtensionProvider StoreExtensionProvider;

	public Action PurchaseCompleted;
	public Action PurchaseFailed;

	const string definitionIdConsumable = "consumable";
	const string definitionIdNonConsumable = "nonconsumable";
	const string definitionIdSubscription = "subscription";

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}


		DontDestroyOnLoad(this);
	}

	public void Initialize()
	{

		ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

		var shopList = DataManager.Get<ShopDataSheet>().GetInfosClone();
		for (int i = 0; i < shopList.Count; i++)
		{
			var shopProduct = shopList[i];
			var productId = GetProductID(shopProduct.productIDs);
			if (productId == null)
			{
				continue;
			}

			ProductType type = ProductType.Consumable;

			if (shopProduct.shopType == ShopType.BATTLEPASS)
			{
				type = ProductType.NonConsumable;
			}

			builder.AddProduct(productId.id, type);
		}

		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		StoreController = controller;
		StoreExtensionProvider = extensions;

	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("Initiailize Fail");
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.Log("Initiailize Fail");
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
	{
		Debug.Log("Purchase Fail");
		PurchaseFailed?.Invoke();
	}
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		PurchaseFailed?.Invoke();

		Debug.Log("Purchase Fail");
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		string definitionId = args.purchasedProduct.definition.id;
		if (string.Equals(definitionId, definitionIdConsumable, StringComparison.Ordinal))

		{

		}
		else if (string.Equals(definitionId, definitionIdNonConsumable, StringComparison.Ordinal))
		{

		}
		else if (string.Equals(definitionId, definitionIdSubscription, StringComparison.Ordinal))
		{

		}

		//var validator = new CrossPlatformValidator(GooglePlayTangle.Data());


		PurchaseCompleted?.Invoke();
		PurchaseCompleted = null;

		//RemoteConfigManager.Instance.CloudSave();
		return PurchaseProcessingResult.Complete;
	}

	public PlatformProductID GetProductID(List<PlatformProductID> productIds)
	{

		PlatformProductID productId = null;

#if UNITY_ANDROID
		productId = productIds.Find(x => x.platform == StoreType.GOOGLE);
#elif UNITY_IOS
		productId = productIds.Find(x => x.platform == StoreType.APPLE);
#endif

		return productId;
	}

	public Product GetProduct(List<PlatformProductID> productIds)
	{

		PlatformProductID productId = null;
		Product product = null;
#if UNITY_ANDROID
		productId = productIds.Find(x => x.platform == StoreType.GOOGLE);
#elif UNITY_IOS
		productId = productIds.Find(x => x.platform == StoreType.APPLE);
#endif
		product = StoreController.products.WithID(productId.id);
		return product;
	}

	public void BuyProduct(string productId)
	{
		if (IsInitialized() == false)
		{
			PurchaseFailed?.Invoke();
			return;
		}
		var product = StoreController.products.WithID(productId);

		if (product == null)
		{
			PurchaseFailed?.Invoke();
			return;
		}

		if (product.availableToPurchase == false)
		{
			PurchaseFailed?.Invoke();
			return;

		}

		StoreController.InitiatePurchase(product);
	}

	public bool IsInitialized()
	{
		return StoreController != null && StoreExtensionProvider != null;
	}

	private void OnDestroy()
	{
		Instance = null;
	}


}
