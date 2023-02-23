using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemGachaItemResult : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI gradeText;


	public void OnUpdate(GachaResult _gachaResult)
	{
		var item = Inventory.it.FindItemByTid(_gachaResult.itemTid);

		icon.sprite = Resources.Load<Sprite>($"Icon/{item.Icon}");
		gradeText.text = $"{item.Grade}";
	}
}
