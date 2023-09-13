using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListCellCurrency : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textName;
	[SerializeField] private TextMeshProUGUI textAmount;
	[SerializeField] private Image imageIcon;


	public void OnUpdate(RuntimeData.CurrencyInfo info)
	{
		textName.text = PlatformManager.Language[info.rawData.name];
		textAmount.text = info.Value.ToString();
		imageIcon.sprite = info.IconImage;
	}
}
