using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPopupRanking : UIBase
{

	public TextMeshProUGUI textNoRanking;
	public Transform content;
	public GameObject prefab;


	public void Open()
	{
		if (Activate())
		{
			SetData();
		}

	}


	public async void SetData()
	{
		var page = await PlatformManager.Instance.LeaderBoard.GetScores();

		int count = page.Results.Count;

		if (count == 0)
		{
			content.gameObject.SetActive(false);
			textNoRanking.gameObject.SetActive(true);
			return;
		}
		else
		{
			content.gameObject.SetActive(true);
			textNoRanking.gameObject.SetActive(false);
		}

		content.CreateListCell(page.Results.Count, prefab);


		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);

			child.gameObject.SetActive(false);
			if (i < page.Results.Count)
			{
				UIListItemRanking listItem = child.GetComponent<UIListItemRanking>();
				listItem.SetData(page.Results[i]);
				listItem.gameObject.SetActive(true);
			}
		}

	}

}
