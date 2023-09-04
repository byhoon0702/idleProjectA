using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

public class LeaderBoard : MonoBehaviour
{
	private const string k_LeaderBoardId = "StageRating";
	public async void AddScore()
	{
		var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(k_LeaderBoardId, PlatformManager.UserDB.stageContainer.LastPlayedNormalStage().StageNumber);
		Debug.Log(JsonConvert.SerializeObject(scoreResponse));
	}

	public async Task<LeaderboardScoresPage> GetScores()
	{
		var scoreResponse = await LeaderboardsService.Instance.GetScoresAsync(k_LeaderBoardId, new GetScoresOptions() { Offset = 0, Limit = 10 });
		Debug.Log(JsonConvert.SerializeObject(scoreResponse));
		return scoreResponse;

	}
}
