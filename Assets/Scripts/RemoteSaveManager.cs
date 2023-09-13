using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.CloudSave;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class RemoteSaveManager : MonoBehaviour
{
	const string k_PurchaseHistory = "Purchase";
	const string k_CloudSave = "CloudSave";

	public async void SavePurchaseHistory(string json)
	{
		await _SavePurchaseHistory(json);
	}
	private async Task _SavePurchaseHistory(string json)
	{
		var data = new Dictionary<string, object> { { k_PurchaseHistory, json } };

		await CloudSaveService.Instance.Data.ForceSaveAsync(data);
	}

	public async Task<string> LoadPurchaseHistory()
	{
		var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { k_PurchaseHistory });
		if (data.ContainsKey(k_PurchaseHistory))
		{
			return data[k_PurchaseHistory];
		}
		else
		{
			return "";
		}
	}

	public async Task CloudSaveAsync(bool showToast = false)
	{
		string json = PlatformManager.UserDB.Save();
		if (json.IsNullOrEmpty())
		{
			return;
		}

		if (PlatformManager.Instance.overrideJson)
		{
			return;
		}

		var data = new Dictionary<string, object> { { k_CloudSave, json } };
		await CloudSaveService.Instance.Data.ForceSaveAsync(data);
		if (showToast)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_cloud_save_success"]);
		}
	}

	public async void CloudSave(bool showToast = false)
	{
		try
		{
			await PlatformManager.RemoteSave.CloudSaveAsync(showToast);

		}
		catch (System.Exception ex)
		{
			Debug.LogError(ex);
		}
	}
	public async Task<string> LoadDataFromCloud()
	{
		var result = await CloudData();
		return result;
	}

	public async Task<string> CloudData()
	{
		var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { k_CloudSave });
		if (data.ContainsKey(k_CloudSave))
		{
			return data[k_CloudSave];
		}
		else
		{
			return "";
		}
	}


	public async Task<UserDB> CloudLoad()
	{
		var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { k_CloudSave });
		if (data.ContainsKey(k_CloudSave))
		{
			UserDBSave saveData = new UserDBSave();
			UserDB temp = new UserDB();
			temp.InitializeContainer();
			saveData.LoadData(temp, data[k_CloudSave]);
			return temp;
		}
		else
		{
			return null;
		}

	}

	public void OpenSavedData()
	{
		if (((PlayGamesPlatform)Social.Active).IsAuthenticated() == false)
		{
			return;
		}

		string fileName = "CloudSave.bin";
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
	}

	public void LoadGameData(ISavedGameMetadata game)
	{
		((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, OnSaveGameDataReadComplete);
	}

	public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			string json = PlatformManager.UserDB.Save();
			byte[] bytes = Encoding.UTF8.GetBytes(json);
			SaveGame(game, bytes, new TimeSpan(PlatformManager.UserDB.userInfoContainer.userInfo.PlayTicks));
		}
		else
		{

		}
	}

	public void OnSaveGameDataReadComplete(SavedGameRequestStatus status, byte[] bytes)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			string json = Encoding.UTF8.GetString(bytes);
			UserDBSave save = new UserDBSave();
			save.LoadData(null, json);
			Newtonsoft.Json.JsonConvert.DeserializeObject(json);
		}
		else
		{

		}
	}


	public void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
	{
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
		builder = builder.WithUpdatedPlayedTime(totalPlaytime).WithUpdatedDescription("Saved At " + TimeManager.Instance.UtcNow);

		SavedGameMetadataUpdate updatedMetaData = builder.Build();
		savedGameClient.CommitUpdate(game, updatedMetaData, savedData, OnSavedGameWrittenComplete);
	}
	public void OnSavedGameWrittenComplete(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{

		}
		else
		{

		}
	}
}
