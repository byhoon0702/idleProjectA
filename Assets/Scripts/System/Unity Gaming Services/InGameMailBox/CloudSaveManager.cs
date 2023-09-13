using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

namespace InGameMailBox
{
	public class CloudSaveManager : MonoBehaviour
	{
		public static CloudSaveManager Instance { get; private set; }

		public List<InboxMessage> InboxMessages { get; private set; } = new List<InboxMessage>();

		string m_LastMessageDownloadedId;
		const string k_InboxStateKey = "MESSAGES_INBOX_STATE";
		const string k_LastMessageDownloadedKey = "MESSAGES_LAST_MESSAGE_DOWNLOADED_ID";


		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		public async Task FetchPlayerInbox()
		{
			try
			{
				var cloudSaveData = await CloudSaveService.Instance.Data.LoadAllAsync();
				if (this == null)
				{
					return;
				}
				if (cloudSaveData.ContainsKey(k_InboxStateKey))
				{
					var inbox = JsonUtility.FromJson<InboxState>(cloudSaveData[k_InboxStateKey]);
					InboxMessages = inbox.messages;
				}
				m_LastMessageDownloadedId = cloudSaveData.ContainsKey(k_LastMessageDownloadedKey) ?
					cloudSaveData[k_LastMessageDownloadedKey] : "";


			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex);
			}
		}

		public int DeleteExpiredMessages()
		{
			int messagesDeletedCount = 0;
			System.DateTime currentDateTime = System.DateTime.Now;
			for (int i = InboxMessages.Count - 1; i >= 0; i--)
			{
				if (System.DateTime.TryParse(InboxMessages[i].metadata.expirationDate, out System.DateTime expirationDateTime))
				{
					if (IsMessageExpired(expirationDateTime, currentDateTime))
					{
						InboxMessages.RemoveAt(i);
						messagesDeletedCount++;
					}
				}
			}
			return messagesDeletedCount;
		}

		bool IsMessageExpired(System.DateTime expirationDateTime, System.DateTime currentDateTime)
		{
			// Could much more simply compare if (expirationDateTime <= currentDateTime), however we want the
			// messages to expire at the top of the minute, instead of at the correct second. i.e. if expiration
			// time is 2:43:35, and current time is 2:43:00 we want the message to be treated as expired.

			if (expirationDateTime.Date < currentDateTime.Date)
			{
				return true;
			}

			if (expirationDateTime.Date == currentDateTime.Date)
			{
				if (expirationDateTime.Hour < currentDateTime.Hour)
				{
					return true;
				}

				if (expirationDateTime.Hour == currentDateTime.Hour)
				{
					if (expirationDateTime.Minute <= currentDateTime.Minute)
					{
						return true;
					}
				}
			}

			return false;
		}

		public void CheckForNewMessages()
		{
			var newMessages = RemoteConfigManager.Instance.GetNextMessages(RemoteConfigManager.MaxMailCount, m_LastMessageDownloadedId);
			if (newMessages == null || newMessages.Count == 0)
			{
				return;
			}

			foreach (var inboxMessage in newMessages)
			{
				var expirationPeriod = System.TimeSpan.Parse(inboxMessage.messageInfo.expirationPeriod);
				bool hasUnclaimedAttachment = (inboxMessage.messageInfo.attachment != null && inboxMessage.messageInfo.attachment.Length > 0);
				inboxMessage.metadata = new MessageMetadata(expirationPeriod, hasUnclaimedAttachment);
				InboxMessages.Add(inboxMessage);
			}
			m_LastMessageDownloadedId = InboxMessages[InboxMessages.Count - 1].messageId;

		}

		public async Task SavePlayerInboxInCloudSave()
		{
			try
			{
				var inboxState = new InboxState { messages = InboxMessages };
				var inboxStateJson = JsonUtility.ToJson(inboxState);
				var dataToSave = new Dictionary<string, object> { { k_LastMessageDownloadedKey, m_LastMessageDownloadedId }, { k_InboxStateKey, inboxStateJson } };
				await CloudSaveService.Instance.Data.ForceSaveAsync(dataToSave);

			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex);
			}
		}
		public void MarkMessageAsRead(string messageId)
		{
			foreach (var message in InboxMessages)
			{
				if (string.Equals(messageId, message.messageId))
				{
					message.metadata.isRead = true;
					break;
				}
			}
		}
		public void DeleteMessage(string messageId)
		{
			int oldMessageCount = InboxMessages.Count;
			foreach (var message in InboxMessages)
			{
				if (string.Equals(messageId, message.messageId))
				{
					InboxMessages.Remove(message);
					break;
				}
			}

			if (oldMessageCount == RemoteConfigManager.MaxMailCount)
			{
				CheckForNewMessages();
			}
		}
		public int DeleteAllReadAndClaimedMessages()
		{
			int messagesDeletedCount = 0;
			int oldMessageCount = InboxMessages.Count;

			for (int i = InboxMessages.Count - 1; i >= 0; i--)
			{
				var message = InboxMessages[i];
				if (message.metadata.isRead && !message.metadata.hasUnclaimedAttachment)
				{
					InboxMessages.RemoveAt(i);
					messagesDeletedCount++;
				}
			}

			if (oldMessageCount == RemoteConfigManager.MaxMailCount)
			{
				CheckForNewMessages();
			}

			return messagesDeletedCount;
		}
		public int DeleteAllClaimedMessages()
		{
			int messagesDeletedCount = 0;
			int oldMessageCount = InboxMessages.Count;

			for (int i = InboxMessages.Count - 1; i >= 0; i--)
			{
				var message = InboxMessages[i];
				if (!message.metadata.hasUnclaimedAttachment)
				{
					InboxMessages.RemoveAt(i);
					messagesDeletedCount++;
				}
			}

			if (oldMessageCount == RemoteConfigManager.MaxMailCount)
			{
				CheckForNewMessages();
			}

			return messagesDeletedCount;
		}

		public async Task ResetMessageInboxData()
		{
			try
			{
				m_LastMessageDownloadedId = "";
				InboxMessages.Clear();

				await Task.WhenAll(
					CloudSaveService.Instance.Data.ForceDeleteAsync(k_LastMessageDownloadedKey),
					CloudSaveService.Instance.Data.ForceDeleteAsync(k_InboxStateKey)
				);
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
			}
		}


		void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		[System.Serializable]
		struct InboxState
		{
			public List<InboxMessage> messages;
		}
	}
}
