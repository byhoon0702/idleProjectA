using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RuntimeData;
using System.Net.Http.Headers;

public class UICostumeInfo : MonoBehaviour
{
	[SerializeField] private UICostumeManagement parentUI;

	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI textPoint;
	[SerializeField] private TextMeshProUGUI textGetPlace;

	[SerializeField] private TextMeshProUGUI textTotalPoints;

	[SerializeField] private Button upgradeButton;
	[SerializeField] private TextMeshProUGUI upgradeButtonLabel;
	[SerializeField] private TextMeshProUGUI upgradeButtonCost;

	[SerializeField] private Button equipButton;
	[SerializeField] private TextMeshProUGUI equipButtonLabel;

	private RuntimeData.CostumeInfo costumeInfo;

	public void OnEnable()
	{
		equipButton.onClick.RemoveAllListeners();
		equipButton.onClick.AddListener(() =>
		{
			if (costumeInfo.unlock == false)
			{
				return;
			}
			GameManager.UserDB.costumeContainer.Equip(parentUI.selectedItemTid, parentUI.costumeType);
			if (UnitManager.it.Player != null)
			{
				UnitManager.it.Player.ChangeCostume();
			}
			parentUI.OnUpdate(false);

		});

		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(() =>
		{
			GameManager.UserDB.costumeContainer.Buy(costumeInfo.Tid);
			parentUI.OnUpdate(false);

		});
	}

	public void OnUpdate(RuntimeData.CostumeInfo info)
	{
		costumeInfo = info;
		itemName.text = costumeInfo.ItemName;
		textPoint.text = costumeInfo.rawData.point.ToString();
		textTotalPoints.text = GameManager.UserDB.costumeContainer.TotalCostumePoints.ToString();
		upgradeButtonCost.text = "무료";

		equipButton.interactable = costumeInfo.unlock;
		equipButton.gameObject.SetActive(costumeInfo.unlock);
		upgradeButton.gameObject.SetActive(costumeInfo.unlock == false);

	}
}
