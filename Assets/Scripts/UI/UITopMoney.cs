using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITopMoney : MonoBehaviour
{
	[Header("gold")]
	[SerializeField] private TextMeshProUGUI textGoldCount;
	[SerializeField] private Button buttonGoldPlus;

	private IdleNumber moneyCount = new IdleNumber(0, 0);

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		SetData();
	}

	public void SetData()
	{
		var currentCount = Inventory.it.GoldItem.count;
		if (currentCount != moneyCount)
		{
			moneyCount = currentCount;
			textGoldCount.text = moneyCount.ToString();
		}
	}

	private void SetButtons()
	{
		buttonGoldPlus.onClick.RemoveAllListeners();
		buttonGoldPlus.onClick.AddListener(OnClickGoldPlusButton);
	}

	private void OnClickGoldPlusButton()
	{

	}

	private void OnClickLevelUpStonePlusButton()
	{

	}

	private void OnClickDiaPlusButton()
	{

	}
}
