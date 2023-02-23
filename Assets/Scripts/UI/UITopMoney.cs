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

	private IdleNumber prevGoldCount = new IdleNumber(0, 0);
	private IdleNumber prevDiaCount = new IdleNumber(0, 0);

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
		var currentGoldCount = Inventory.it.ItemCount(Inventory.it.GoldTid);
		if (currentGoldCount != prevGoldCount)
		{
			prevGoldCount = currentGoldCount;
			textGoldCount.text = prevGoldCount.ToString();
		}

		var currentDiaCount = Inventory.it.ItemCount(Inventory.it.DiaTid);
		if(currentDiaCount != prevDiaCount)
		{
			prevDiaCount = currentDiaCount;
			textDiaCount.text = prevDiaCount.ToString();
		}
	}
}
