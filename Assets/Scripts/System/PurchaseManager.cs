using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Unity.Services.Core;
using UnityEngine.Purchasing.Security;
using System.Threading.Tasks;

using UnityEngine.Analytics;

[System.Serializable]
public class PurchaseHistory
{
	public List<string> history;
	public PurchaseHistory()
	{
		history = new List<string>();
	}

}

public class PurchaseManager : MonoBehaviour, IDetailedStoreListener
{
	public static PurchaseManager Instance { get; private set; }

	public IStoreController StoreController;
	public IGooglePlayStoreExtensions GooglePlayStoreExtensions;


	public Action PurchaseCompleted;
	public Action PurchaseFailed;

	const string definitionIdConsumable = "consumable";
	const string definitionIdNonConsumable = "nonconsumable";
	const string definitionIdSubscription = "subscription";

	public PurchaseHistory history = new PurchaseHistory();

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

	public async Task LoadHistory()
	{
		string json = await PlatformManager.RemoteSave.LoadPurchaseHistory();
		var obj = JsonUtility.FromJson(json, typeof(PurchaseHistory));
		if (obj != null)
		{
			history = obj as PurchaseHistory;
		}
		else
		{
			history = new PurchaseHistory();
		}
	}

	public void Initialize()
	{

		ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
		builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);

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
	void OnDeferredPurchase(Product product)
	{
		Debug.Log($"{product.definition.id} is Deferred");
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		StoreController = controller;
		GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();



		//StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>().RestoreTransactions((isOk, context) =>
		//{
		//	Debug.Log(context);
		//});
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("Initiailize Fail");
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.Log("Initiailize Fail");
	}

	public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureDescription failureDescription)
	{

		Debug.Log($"Purchase Fail {failureDescription} {failureDescription.reason}");
		if (failureDescription.message.Contains("ItemAlreadyOwned"))
		{
			GooglePlayStoreExtensions.RestoreTransactions((isOk, context) =>
			{
				Debug.Log($"Restore : {isOk} {context} ");
				if (isOk)
				{
					PurchaseCompleted?.Invoke();
					PurchaseCompleted = null;
					StoreController.ConfirmPendingPurchase(product);
				}
			});
			return;
		}
		PurchaseFailed?.Invoke();
	}

	public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
	{
		PurchaseFailed?.Invoke();

		Debug.Log($"Purchase Fail {failureReason}");
	}

	bool isfail = false;
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

		Debug.Log($"Purchase Process {definitionId}");


#if !UNITY_EDITOR
		var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

		if (validator != null)
		{
			Debug.Log("====== validator 1");
			var result = validator.Validate(args.purchasedProduct.receipt);
			Debug.Log("====== validator 2");
			foreach (IPurchaseReceipt purchaseReceipt in result)
			{
				string data = $"{purchaseReceipt.productID} {purchaseReceipt.purchaseDate} {purchaseReceipt.transactionID}";
				Debug.Log(data);
				//history.history.Add(data);
				//Analytics.Transaction(purchaseReceipt.productID, args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.metadata.isoCurrencyCode, purchaseReceipt.purchaseDate.ToString(), purchaseReceipt.transactionID);
			}
			Debug.Log("====== validator 3");
			//PlatformManager.RemoteSave.SavePurchaseHistory(JsonUtility.ToJson(history));
		}


#endif
		PurchaseCompleted?.Invoke();
		PurchaseCompleted = null;

		PlatformManager.RemoteSave.CloudSave();
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

	public UnityEngine.Purchasing.Product GetProduct(List<PlatformProductID> productIds)
	{
		if (productIds == null || productIds.Count == 0)
		{
			return null;
		}

		PlatformProductID productId = null;
		UnityEngine.Purchasing.Product product = null;
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
		return StoreController != null && GooglePlayStoreExtensions != null;
	}

	private void OnDestroy()
	{
		//Instance = null;
	}


}
