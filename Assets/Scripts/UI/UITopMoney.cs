using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITopMoney : MonoBehaviour
{
	[Header("gold")]
	[SerializeField] private TextMeshProUGUI textGoldCount;
	[SerializeField] private TextMeshProUGUI textDiaCount;

	private IdleNumber prevGoldCount = new IdleNumber(0);
	private IdleNumber prevDiaCount = new IdleNumber(0);

	private bool isInitialized = false;

	public void Init()
	{
		SetData();
		isInitialized = true;
	}

	private void Update()
	{
		if (isInitialized == false)
		{
			return;
		}
		SetData();
	}

	public void SetData()
	{
		var currentGoldCount = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.GOLD);
		textGoldCount.text = currentGoldCount.Value.ToString();

		var currentDiaCount = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.DIA);
		textDiaCount.text = currentDiaCount.Value.ToString();

	}
}
