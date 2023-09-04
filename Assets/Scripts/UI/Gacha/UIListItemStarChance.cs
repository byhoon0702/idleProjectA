using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIListItemStarChance : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textChance;
	[SerializeField] private GameObject[] stars;

	public void SetData(int star, int chance)
	{

		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].SetActive(i < star);
		}
		textChance.text = $"{chance.ToString("00.00")}%";
	}
}
