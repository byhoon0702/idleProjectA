using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuffItem : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textExp;

	[SerializeField] private Button levelUpButton;

	[SerializeField] private Image iconImage;

	private UIBuffData uiData;

	private void Awake()
	{
		levelUpButton.onClick.RemoveAllListeners();
		levelUpButton.onClick.AddListener(BuffLevelUp);
	}

	public void OnUpdate(UIBuffData _uiData)
	{
		uiData = _uiData;

		UpdateIcon();
		UpdateBuffInfo();
		UpdateButton();
	}

	private void UpdateIcon()
	{
		iconImage.sprite = Resources.Load<Sprite>($"Icon/{uiData.itemData.Icon}");
	}

	private void UpdateBuffInfo()
	{
		textTitle.text = uiData.TitleText;
		textExp.text = $"{uiData.currentExp}/{uiData.maxExp}";
	}

	private void UpdateButton()
	{
		levelUpButton.interactable = uiData.Levelupable();
	}

	private void BuffLevelUp()
	{
		uiData.BuffExpUp(() =>
		{
			UpdateBuffInfo();
		});
	}
}
