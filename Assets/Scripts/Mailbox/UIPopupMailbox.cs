using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using InGameMailBox;
using TMPro;

public class UIPopupMailbox : UIBase
{
	[SerializeField] private TextMeshProUGUI textNoMail;
	[SerializeField] private UIPopupMailboxDetail uiPopupDetail;
	[SerializeField] private Button _buttonReceiveAll;

	[SerializeField] private TextMeshProUGUI messageCountText;

	[SerializeField] private Transform _content;
	[SerializeField] private GameObject _prefab;


	private bool isInitialized = false;
	private string selectMessageId;

	public async void Reset()
	{
		selectMessageId = "";
		await CloudSaveManager.Instance.ResetMessageInboxData();
		if (this == null) return;
		await FetchUpdatedInboxData();
		if (this == null) return;

		RefreshView();
	}
	private void Awake()
	{
		_buttonReceiveAll.SetButtonEvent(OnClaimAllMail);
	}
	async Task FetchUpdatedInboxData()
	{
		PlatformManager.Instance.ShowLoadingRotate(true);

		await Task.WhenAll(
			RemoteConfigManager.Instance.FetchConfigs(),
			CloudSaveManager.Instance.FetchPlayerInbox()
			);


		if (this == null)
		{
			PlatformManager.Instance.ShowLoadingRotate(false);
			return;
		}

		CloudSaveManager.Instance.DeleteExpiredMessages();
		CloudSaveManager.Instance.CheckForNewMessages();
		await CloudSaveManager.Instance.SavePlayerInboxInCloudSave();
		PlatformManager.Instance.ShowLoadingRotate(false);
	}

	public async void Open()
	{
		if (Activate() == false)
		{
			return;
		}
		gameObject.SetActive(true);
		PlatformManager.Instance.ShowLoadingRotate(true);

		await FetchUpdatedInboxData();

		CloudSaveManager.Instance.CheckForNewMessages();
		InitializeView();
		PlatformManager.Instance.ShowLoadingRotate(false);

	}

	private void InitializeView()
	{
		if (isInitialized == false)
		{
			_content.CreateListCell(RemoteConfigManager.MaxMailCount, _prefab);
			for (int i = 0; i < _content.childCount; i++)
			{
				var child = _content.GetChild(i);
				child.gameObject.SetActive(false);
				UIItemMail itemMail = child.GetComponent<UIItemMail>();
				itemMail.MessageSelect += OnMessageSelect;
			}


			isInitialized = true;
		}
		RefreshView();
	}
	public void OnMessageSelect(UIItemMail message)
	{
		SelectMessage(message.Message.messageId);
		uiPopupDetail.Open(this, message);
		RefreshView();

	}

	public void RefreshView()
	{
		int count = CloudSaveManager.Instance.InboxMessages.Count;
		textNoMail.gameObject.SetActive(count == 0);
		messageCountText.gameObject.SetActive(count > 0);
		//if (count > 0)
		//{
		for (int i = 0; i < _content.childCount; i++)
		{
			var child = _content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < CloudSaveManager.Instance.InboxMessages.Count)
			{
				var message = CloudSaveManager.Instance.InboxMessages[i];
				SetListItemView(child, message);
			}
		}
		messageCountText.text = $"{CloudSaveManager.Instance.InboxMessages.Count}/{RemoteConfigManager.MaxMailCount}";
		//}
		_buttonReceiveAll.gameObject.SetActive(count > 0);
	}

	private void SetListItemView(Transform view, InboxMessage message)
	{
		UIItemMail itemMail = view.GetComponent<UIItemMail>();
		itemMail.SetData(this, message, string.Equals(message.messageId, selectMessageId));
		itemMail.gameObject.SetActive(true);
	}
	void UpdateInboxCounts()
	{
		if (messageCountText != null)
		{
			messageCountText.text = $"{CloudSaveManager.Instance.InboxMessages.Count} / {RemoteConfigManager.MaxMailCount}";
		}

		if (CloudSaveManager.Instance.InboxMessages.Count == RemoteConfigManager.MaxMailCount)
		{
			//messageCountText.color = inboxFullTextColor;
			//messageFullAlert.gameObject.SetActive(!m_IsFetchingNewMessages);
		}
		else
		{
			//messageCountText.color = inboxDefaultTextColor;
			//messageFullAlert.gameObject.SetActive(false);
		}
	}
	async void Update()
	{
		try
		{
			if (CloudSaveManager.Instance.DeleteExpiredMessages() > 0)
			{
				PlatformManager.Instance.ShowLoadingRotate(true);
				CloudSaveManager.Instance.CheckForNewMessages();
				await CloudSaveManager.Instance.SavePlayerInboxInCloudSave();
				PlatformManager.Instance.ShowLoadingRotate(false);
				if (this == null)
				{
					return;
				}
			}

		}
		catch (System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	public async void SelectMessage(string messageId)
	{
		PlatformManager.Instance.ShowLoadingRotate(true);
		try
		{
			selectMessageId = messageId;
			CloudSaveManager.Instance.MarkMessageAsRead(selectMessageId);
			await CloudSaveManager.Instance.SavePlayerInboxInCloudSave();
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		PlatformManager.Instance.ShowLoadingRotate(false);
	}

	public async void OnDeleteOpenMessageButtonPressed()
	{
		PlatformManager.Instance.ShowLoadingRotate(true);
		try
		{
			CloudSaveManager.Instance.DeleteMessage(selectMessageId);
			await CloudSaveManager.Instance.SavePlayerInboxInCloudSave();
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		PlatformManager.Instance.ShowLoadingRotate(false);
	}

	public async Task<bool> OnClaimMailAttachment()
	{

		try
		{
			await InGameMailBox.CloudCodeManager.Instance.CallClaimMessageAttachmentEndpoint(selectMessageId);
			if (this == null) return false;
			await UpdateSceneAfterClaim();
			OnDeleteAllReadAndClaimedMail();

			return true;
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			return false;
		}

	}
	async Task UpdateSceneAfterClaim()
	{
		PlatformManager.Instance.ShowLoadingRotate(true);
		await CloudSaveManager.Instance.FetchPlayerInbox();
		if (this == null)
			return;
		RefreshView();
		PlatformManager.Instance.ShowLoadingRotate(false);
	}


	public async void OnClaimAllMail()
	{
		PlatformManager.Instance.ShowLoadingRotate(true);
		try
		{

			await InGameMailBox.CloudCodeManager.Instance.CallClaimAllMessageAttachmentsEndpoint();
			if (this == null) return;

			Dictionary<long, AddItemInfo> rewardList = new Dictionary<long, AddItemInfo>();

			foreach (var message in CloudSaveManager.Instance.InboxMessages)
			{

				for (int i = 0; i < message.messageInfo.attachment.Length; i++)
				{
					var attachment = message.messageInfo.attachment[i];
					if (rewardList.ContainsKey(attachment.tid))
					{
						rewardList[attachment.tid].AddValue(attachment.amount);
					}
					else
					{
						rewardList.Add(attachment.tid, new AddItemInfo(attachment.tid, (IdleNumber)attachment.amount, RewardCategory.Currency));
					}
				}
			}


			await UpdateSceneAfterClaim();
			OnDeleteAllReadAndClaimedMail();
			PlatformManager.UserDB.AddRewards(new List<AddItemInfo>(rewardList.Values), true);
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		PlatformManager.Instance.ShowLoadingRotate(false);
	}

	public async void OnDeleteAllReadAndClaimedMail()
	{
		try
		{
			if (CloudSaveManager.Instance.DeleteAllReadAndClaimedMessages() > 0)
			{
				RefreshView();
				await CloudSaveManager.Instance.SavePlayerInboxInCloudSave();
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}

	}


	public async void OnResetInbox()
	{

	}


}
