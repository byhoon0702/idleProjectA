using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

using System.Globalization;


public enum NoticeButtonType
{
	OK = 0,
	GOTOCOMMUNITY = 1,
	QUIT = 2,

}

[System.Serializable]
public class Notice
{
	public int id;
	public string context;
	public NoticeButtonType buttonType;
}

public class RemoteConfigManager : MonoBehaviour
{
	public static RemoteConfigManager Instance { get; private set; }
	public const int MaxMailCount = 20;
	public string Season { get; private set; }
	List<string> m_OrderedMessageIds;

	private int _userLevel;
	private int _stageLevel;
	public string Version { get; private set; }
	public string TestVersion { get; private set; }
	public bool Maintenance { get; private set; }

	public Notice Notice { get; private set; }

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


	public async Task FetchConfigs()
	{
		//RemoteConfigService.Instance.FetchCompleted -= RemoteConfigLoaded;
		RemoteConfigService.Instance.SetCustomUserID(PlatformManager.UserDB.userInfoContainer.userInfo.UUID);
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
		Notice = null;
		switch (configResponse.requestOrigin)
		{
			case ConfigOrigin.Default:
				break;
			case ConfigOrigin.Cached:
				break;
			case ConfigOrigin.Remote:
				Season = RemoteConfigService.Instance.appConfig.GetString("Season");
				Version = RemoteConfigService.Instance.appConfig.GetString("Version");
				TestVersion = RemoteConfigService.Instance.appConfig.GetString("TestVersion");
				Maintenance = RemoteConfigService.Instance.appConfig.GetBool("Maintenance");
				string json = RemoteConfigService.Instance.appConfig.GetJson("Notice");
				if (json.IsNullOrEmpty() == false)
				{
					Notice = JsonUtility.FromJson<Notice>(json);
				}
				break;
		}
	}

	public bool IsNoticeExist(System.Action onClose = null)
	{
		if (Notice == null)
		{
			return false;
		}

		if (PlayerPrefs.GetInt("Notice_Id") == Notice.id)
		{
			return false;
		}

		System.Action action = null;
		string str_ok = "";
		switch (Notice.buttonType)
		{
			case NoticeButtonType.OK:
				str_ok = "str_ui_ok";
				action = onClose;
				break;
			case NoticeButtonType.GOTOCOMMUNITY:
				str_ok = "str_ui_go_to_community";
				action = () => { GoToCommunity(); onClose.Invoke(); };
				break;
			case NoticeButtonType.QUIT:
				str_ok = "str_ui_quit_game";
#if !UNITY_EDITOR
				action = Application.Quit;
#else
				action = () => { UnityEditor.EditorApplication.isPlaying = false; };
#endif

				break;
		}

		PopAlert.CreateNotice("공지", Notice.context, str_ok, action);
		PlayerPrefs.SetInt("Notice_Id", Notice.id);
		return true;
	}
	public bool NeedUpdate()
	{
		if (Version.Equals(Application.version) == false && TestVersion.Equals(Application.version) == false)
		{
			PopAlert.Create("알림", $"앱 업데이트가 필요합니다.\n최신버젼{Version}\n현재버젼{Application.version}", "str_ui_go_to_store", "닫기", GoToStore);
			return true;
		}
		return false;
	}

	public void GoToCommunity()
	{
		Application.OpenURL("https://game.naver.com/lounge/Rejuvenation_Hero_Idle_RPG/");
	}
	public void GoToStore()
	{
#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.nclo.aos.projectb");
#endif
		Application.Quit();
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
		public string uuid;
	}

	public struct appAttributes
	{ }


}
