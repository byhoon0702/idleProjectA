using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
public class UIItemMail : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _textTitle;
	[SerializeField] private TextMeshProUGUI _textDate;

	[SerializeField] private Button _buttonShow;
	[SerializeField] private Transform _content;
	[SerializeField] private GameObject _prefab;

	public event Action<UIItemMail> MessageSelect;

	public InboxMessage Message { get; private set; }
	private UIPopupMailbox _parent;
	private List<RuntimeData.RewardInfo> rewardList = new List<RuntimeData.RewardInfo>();
	private void Awake()
	{
		_buttonShow.SetButtonEvent(ShowMail);
	}

	private void ShowMail()
	{
		MessageSelect?.Invoke(this);
	}

	public void SetData(UIPopupMailbox parent, InboxMessage message, bool isCurrentlySelect)
	{
		_parent = parent;
		Message = message;
		_textTitle.text = Message.messageInfo.title;


		if (DateTime.TryParse(Message.metadata.expirationDate, out var expiration))
		{
			TimeSpan ts = expiration - TimeManager.Instance.UtcNow;

			if (ts.TotalDays > 1)
			{
				_textDate.text = $"{ts.TotalDays}일 남음";
			}
			else
			{
				_textDate.text = $"{ts.Hours:00}시간 {ts.Minutes:00}분 남음";
			}


		}
		else
		{
			_textDate.text = "";
		}

		int count = Message.messageInfo.attachment != null ? Message.messageInfo.attachment.Length : 0;

		_content.CreateListCell(count, _prefab);

		rewardList.Clear();

		for (int i = 0; i < Message.messageInfo.attachment.Length; i++)
		{
			var attachment = Message.messageInfo.attachment[i];

			rewardList.Add(new RuntimeData.RewardInfo(new ChanceReward() { tid = attachment.tid, category = RewardCategory.Currency, countMin = attachment.amount }));
		}

		for (int i = 0; i < _content.childCount; i++)
		{
			var child = _content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < rewardList.Count)
			{
				UIItemReward itemReward = child.GetComponent<UIItemReward>();
				itemReward.Set(new AddItemInfo(rewardList[i]));
				itemReward.gameObject.SetActive(true);
			}
		}

	}

	public async Task GetReward()
	{
		if (Message.metadata.hasUnclaimedAttachment == false)
		{
			ToastUI.Instance.Enqueue("이미 받은 보상 입니다.");
			return;
		}
		PlatformManager.Instance.ShowLoadingRotate(true);
		bool isdone = await _parent.OnClaimMailAttachment();

		if (isdone)
		{
			PlatformManager.UserDB.AddRewards(rewardList, true);
		}
		PlatformManager.Instance.ShowLoadingRotate(false);
	}
}
