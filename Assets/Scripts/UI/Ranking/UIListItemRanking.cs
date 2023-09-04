using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards.Models;
using TMPro;
public class UIListItemRanking : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI textLabel;
	public void SetData(LeaderboardEntry entity)
	{
		textLabel.text = $"{entity.Rank + 1} {entity.PlayerName} {entity.Score}";
	}
}
