using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGachaProbabilityPopup : MonoBehaviour, IUIClosable
{
	[SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private TextMeshProUGUI descText;
	[SerializeField] private Button plusButton;
	[SerializeField] private Button minusButton;
	[SerializeField] private Button closeButton;


	private int showLevel;
	private UIGachaData gachaEntryData;


	private void Awake()
	{
		plusButton.onClick.RemoveAllListeners();
		plusButton.onClick.AddListener(OnPlusButtonClick);

		minusButton.onClick.RemoveAllListeners();
		minusButton.onClick.AddListener(OnMinusButtonClick);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(() => 
		{
			if(Closable())
			{
				Close();
			}
		});
	}

	public void OnUpdate(int _level, UIGachaData _uiData)
	{
		showLevel = _level;
		showLevel = Mathf.Clamp(showLevel, 1, 10);
		gachaEntryData = _uiData;


		levelText.text = $"Level {showLevel}";

		string descString = "";
		foreach (Grade grade in Enum.GetValues(typeof(Grade)))
		{
			double probability =_uiData.GetProbability(grade, showLevel);
			descString += $"[{grade}] {(probability * 100).ToString("F4")}%\n";
		}

		descText.text = descString;
	}

	private void OnPlusButtonClick()
	{
		OnUpdate(showLevel + 1, gachaEntryData);
	}

	private void OnMinusButtonClick()
	{
		OnUpdate(showLevel - 1, gachaEntryData);
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
