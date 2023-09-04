using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using System.Threading.Tasks;

public class EconomyManager : MonoBehaviour
{
	public static EconomyManager Instance { get; private set; }

	public Dictionary<string, List<ItemAndAmountSpec>> virtualPurchaseTransactions { get; private set; } =
		  new Dictionary<string, List<ItemAndAmountSpec>>();


	List<VirtualPurchaseDefinition> virtualPurchaseDefinitions;
	List<InventoryItemDefinition> inventoryItemDefinitions;
	List<CurrencyDefinition> currencyDefinitions;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public async Task RefreshEconomyConfiguration()
	{
		await EconomyService.Instance.Configuration.SyncConfigurationAsync();

		currencyDefinitions = EconomyService.Instance.Configuration.GetCurrencies();
		inventoryItemDefinitions = EconomyService.Instance.Configuration.GetInventoryItems();
		virtualPurchaseDefinitions = EconomyService.Instance.Configuration.GetVirtualPurchases();


		InitializeEconomyLookUp();
		InitializeVirtualPurchaseLookup();
	}

	private void InitializeEconomyLookUp()
	{
		foreach (var inventoryItemDefinition in inventoryItemDefinitions)
		{
			if (inventoryItemDefinition.CustomDataDeserializable.GetAs<Dictionary<string, string>>() is { } customData &&
				customData.TryGetValue("amount", out string value))
			{

			}

		}
		foreach (var currencyDefinition in currencyDefinitions)
		{
			if (currencyDefinition.CustomDataDeserializable.GetAs<Dictionary<string, string>>() is { } customData &&
				customData.TryGetValue("amount", out string value))
			{

			}
		}
	}

	private void InitializeVirtualPurchaseLookup()
	{

	}

}
