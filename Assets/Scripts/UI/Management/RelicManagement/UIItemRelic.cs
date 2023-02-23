using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemRelic : MonoBehaviour
{
	[SerializeField] private Image[] icons;


	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textCurrentStat;
	[SerializeField] private TextMeshProUGUI textNextStat;

	[SerializeField] private RepeatButton upgradeButton;
	[SerializeField] private TextMeshProUGUI textPrice;

	[Header("State")]
	[SerializeField] private GameObject noPossessed;


	private UIRelicData uiData;



	private void Awake()
	{
		upgradeButton.repeatCallback += OnLevelupButtonClick;
	}

	public void OnUpdate(UIRelicData _uiData)
	{
		uiData = _uiData;

		Sprite iconSprite = Resources.Load<Sprite>($"Icon/{uiData.Icon}");
		foreach(var icon in icons)
		{
			icon.sprite = iconSprite;
		}

		textTitle.text = uiData.ItemName;
		textCurrentStat.text = uiData.CurrentStatText;
		textNextStat.text = uiData.NextStatText;
		textPrice.text = uiData.LevelupCostText;

		var item = uiData.GetItem();
		if (item == null || item.Count == 0)
		{
			noPossessed.SetActive(true);
		}
		else
		{
			noPossessed.SetActive(false);
		}

		UpdateButton();
	}

	public void OnRefresh()
	{
		UpdateButton();
	}

	public void UpdateButton()
	{
		upgradeButton.SetInteractable(uiData.Levelupable());
	}

	private void OnLevelupButtonClick()
	{
		uiData.LevelupItem(() =>
		{
			OnUpdate(uiData);
		});
	}
}
