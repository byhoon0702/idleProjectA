using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGachaButton : MonoBehaviour
{
	[Header("11회버튼")]
	[SerializeField] private Button gacha11Button;
	[SerializeField] private Image gacha11ConsumeIcon;
	[SerializeField] private TextMeshProUGUI gacha11CostText;
	[SerializeField] private TextMeshProUGUI gacha11TitleText;

	[Header("35회버튼")]
	[SerializeField] private Button gacha35Button;
	[SerializeField] private Image gacha35ConsumeIcon;
	[SerializeField] private TextMeshProUGUI gacha35CostText;
	[SerializeField] private TextMeshProUGUI gacha35TitleText;

	[Header("ad버튼")]
	[SerializeField] private Button gachaAdButton;
	[SerializeField] private TextMeshProUGUI gachaAdCostText;
	[SerializeField] private TextMeshProUGUI gachaAdTitleText;

	private UIGachaData uiData;

	private void Awake()
	{
		gacha11Button.onClick.RemoveAllListeners();
		gacha11Button.onClick.AddListener(OnGacha11ButtonClick);

		gacha35Button.onClick.RemoveAllListeners();
		gacha35Button.onClick.AddListener(OnGacha35ButtonClick);

		gachaAdButton.onClick.RemoveAllListeners();
		gachaAdButton.onClick.AddListener(OnGachaAdButtonClick);
	}

	public void OnUpdate(UIGachaData _uiData)
	{
		uiData = _uiData;

		// 11연
		var gacha11ConsumeTid = uiData.ConsumeItemTid(GachaButtonType.Gacha11);
		var gacha11ConsumeCount = uiData.ConsumeItemCount(GachaButtonType.Gacha11);

		gacha11Button.gameObject.SetActive(uiData.IsActiveSummonButton(GachaButtonType.Gacha11));
		gacha11Button.interactable = Inventory.it.CheckMoney(gacha11ConsumeTid, gacha11ConsumeCount).Ok();
		gacha11ConsumeIcon.sprite = Resources.Load<Sprite>($"Icon/{gacha11ConsumeTid}");
		gacha11CostText.text = $"{gacha11ConsumeCount.ToString()}";
		gacha11TitleText.text = $"{uiData.SummonItemCount(GachaButtonType.Gacha11)}회 소환";

		// 35연
		var gacha35ConsumeTid = uiData.ConsumeItemTid(GachaButtonType.Gacha35);
		var gacha35ConsumeCount = uiData.ConsumeItemCount(GachaButtonType.Gacha35);

		gacha35Button.gameObject.SetActive(uiData.IsActiveSummonButton(GachaButtonType.Gacha35));
		gacha35Button.interactable = Inventory.it.CheckMoney(gacha35ConsumeTid, gacha35ConsumeCount).Ok();
		gacha35ConsumeIcon.sprite = Resources.Load<Sprite>($"Icon/{gacha35ConsumeTid}");
		gacha35CostText.text = $"{gacha35ConsumeCount.ToString()}";
		gacha35TitleText.text = $"{uiData.SummonItemCount(GachaButtonType.Gacha35)}회 소환";

		// 광고소환
		string itemHashTag = "";
		switch (uiData.GachaType)
		{
			case GachaType.Equip:
				itemHashTag = "gacha_ad_equip";
				break;
			case GachaType.Skill:
				itemHashTag = "gacha_ad_skill";
				break;
			case GachaType.Pet:
				itemHashTag = "gacha_ad_pet";
				break;
		}

		gachaAdButton.gameObject.SetActive(uiData.IsActiveSummonButton(GachaButtonType.Ads));
		gachaAdButton.interactable = Inventory.it.CheckMoney(itemHashTag, new IdleNumber(1)).Ok();
		gachaAdCostText.text = $"{Inventory.it.ItemCount(itemHashTag).ToString()} / {uiData.AdSummonMaxCount}";
		gachaAdTitleText.text = $"{uiData.SummonItemCount(GachaButtonType.Ads)}회 소환";
	}

	private void OnGacha11ButtonClick()
	{
		uiData.SummonGacha(GachaButtonType.Gacha11, (newItems) => 
		{
			UIController.it.ShowGachaRewardPopup(uiData, newItems);
		});
	}

	private void OnGacha35ButtonClick()
	{
		uiData.SummonGacha(GachaButtonType.Gacha35, (newItems) =>
		{
			UIController.it.ShowGachaRewardPopup(uiData, newItems);
		});
	}

	private void OnGachaAdButtonClick()
	{
		uiData.SummonGacha(GachaButtonType.Ads, (newItems) =>
		{
			UIController.it.ShowGachaRewardPopup(uiData, newItems);
		});
	}
}
