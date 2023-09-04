using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

using System.Globalization;
public class RemoteConfigManager : MonoBehaviour
{
	public static RemoteConfigManager Instance { get; private set; }
	public const int MaxMailCount = 20;
	public string Season;
	List<string> m_OrderedMessageIds;

	private int _userLevel;
	private int _stageLevel;
	public string Version { get; private set; }
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}
	}
	private void Start()
	{
		RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
	}
	public void UpdateUserLevel(int userLevel)
	{
		_userLevel = userLevel;
	}

	public void UpdateStageLevel(int stageLevel)
	{
		_stageLevel = stageLevel;
	}

	public async void CloudSave(bool showToast = false)
	{
		try
		{
			await CloudSaveAsync();
			if (showToast)
			{
				ToastUI.Instance.Enqueue("클라우드 저장 완료");
			}
		}
		catch (System.Exception ex)
		{
			Debug.LogError(ex);
		}
	}


	const string k_CloudSave = "CloudSave";
	public async Task CloudSaveAsync()
	{
		string json = PlatformManager.UserDB.Save();
		if (json.IsNullOrEmpty())
		{
			return;
		}
		var data = new Dictionary<string, object> { { k_CloudSave, json } };

		await CloudSaveService.Instance.Data.ForceSaveAsync(data);

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
			UserDBSave saveData = new UserDBSave();//Newtonsoft.Json.JsonConvert.DeserializeObject<UserDBSave>(data[k_CloudSave]);
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
	public async Task FetchConfigs()
	{
		//RemoteConfigService.Instance.FetchCompleted -= RemoteConfigLoaded;

		await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes() { userLevel = _userLevel, stageLevel = _stageLevel }, new appAttributes());

		if (this == null)
		{
			return;
		}

		CacheConfigValues();

	}
	public async void FetchUpdatedInboxData()
	{
		await Task.WhenAll(
		FetchConfigs(),
		InGameMailBox.CloudSaveManager.Instance.FetchPlayerInbox()
		);

	}

	void CacheConfigValues()
	{
		var json = RemoteConfigService.Instance.appConfig.GetJson("MESSAGES_ALL", "");
		if (json.IsNullOrEmpty())
		{
			Debug.LogError("Remote config key \"MESSAGES_ALL\" cannot be found.");
			return;
		}

		var messageIds = JsonUtility.FromJson<MessageIds>(json);
		m_OrderedMessageIds = messageIds.messageList;
	}

	private void RemoteConfigLoaded(ConfigResponse configResponse)
	{
		switch (configResponse.requestOrigin)
		{
			case ConfigOrigin.Default:
				break;
			case ConfigOrigin.Cached:
				break;
			case ConfigOrigin.Remote:
				Season = RemoteConfigService.Instance.appConfig.GetString("Season");
				Version = RemoteConfigService.Instance.appConfig.GetString("Version");
				break;
		}
	}

	public bool NeedUpdate()
	{
		if (Version.Equals(Application.version) == false)
		{
			PopAlert.Create("알림", $"앱 업데이트가 필요합니다.\n최신버젼{Version}\n현재버젼{Application.version}", GoToStore);
			return true;
		}
		return false;
	}

	public void GoToStore()
	{
#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.nclo.aos.projectb");
#endif
	}

	public List<InboxMessage> GetNextMessages(int numberOfMessages, string lastMessageId = "")
	{
		if (m_OrderedMessageIds is null)
		{
			return null;
		}

		if (string.IsNullOrEmpty(lastMessageId))
		{
			return GetNextMessagesFromStartLocation(0, numberOfMessages);
		}

		for (var i = 0; i < m_OrderedMessageIds.Count; i++)
		{
			if (string.Equals(m_OrderedMessageIds[i], lastMessageId) && i + 1 < m_OrderedMessageIds.Count)
			{
				return GetNextMessagesFromStartLocation(i + 1, numberOfMessages);
			}
		}

		return null;
	}

	List<InboxMessage> GetNextMessagesFromStartLocation(int startLocation, int numberOfMessages)
	{
		var newMessages = new List<InboxMessage>();

		for (var i = startLocation; i < m_OrderedMessageIds.Count; i++)
		{
			if (numberOfMessages > 0)
			{
				var message = FetchMessage(m_OrderedMessageIds[i]);

				// Some message values will be blank if the player does not fall into a targeted audience.
				// We want to filter those messages out when downloading a specific number of messages.
				if (MessageIsValid(message))
				{
					newMessages.Add(message);
					numberOfMessages--;
				}
			}

			if (numberOfMessages == 0)
			{
				break;
			}
		}

		return newMessages;
	}

	InboxMessage FetchMessage(string messageId)
	{
		var json = RemoteConfigService.Instance.appConfig.GetJson(messageId, "");

		if (string.IsNullOrEmpty(json))
		{
			Debug.LogError($"Remote config key {messageId} cannot be found.");
			return new InboxMessage();
		}

		var message = JsonUtility.FromJson<MessageInfo>(json);

		return message == null
			? new InboxMessage()
			: new InboxMessage(messageId, message);
	}

	bool MessageIsValid(InboxMessage inboxMessage)
	{
		var message = inboxMessage.messageInfo;

		if (string.IsNullOrEmpty(inboxMessage.messageId) || message == null ||
			string.IsNullOrEmpty(message.title) || string.IsNullOrEmpty(message.content) ||
			string.IsNullOrEmpty(message.expirationPeriod) || !System.TimeSpan.TryParse(message.expirationPeriod,
				new CultureInfo("en-US"), out var timespan))
		{
			return false;
		}

		return true;
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}


	[System.Serializable]
	public struct MessageIds
	{
		public List<string> messageList;
	}
	public struct userAttributes
	{
		public int userLevel;
		public int stageLevel;
	}

	public struct appAttributes
	{ }


}
