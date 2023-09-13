using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupMailboxDetail : UIBase
{
	[SerializeField] private TextMeshProUGUI _textTitle;
	[SerializeField] private TextMeshProUGUI _textDetail;

	[SerializeField] private Transform _content;
	[SerializeField] private GameObject _prefab;


	[SerializeField] private Button _buttonGet;

	private InboxMessage _message;
	private UIItemMail _item;
	UIPopupMailbox _parent;
	private void Awake()
	{
		_buttonGet.SetButtonEvent(OnClick);

	}

	public void Open(UIPopupMailbox parent, UIItemMail item)
	{
		_parent = parent;
		_item = item;
		_message = item.Message;
		gameObject.SetActive(true);

		_textTitle.text = _message.messageInfo.title;
		_textDetail.text = _message.messageInfo.content;


		int count = _message.messageInfo.attachment != null ? _message.messageInfo.attachment.Length : 0;
		_content.CreateListCell(count, _prefab);
		for (int i = 0; i < _content.childCount; i++)
		{
			var child = _content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < count)
			{
				var reward = _message.messageInfo.attachment[i];
				UIItemReward itemReward = child.GetComponent<UIItemReward>();
				itemReward.Set(new AddItemInfo(new RuntimeData.RewardInfo(new ChanceReward() { tid = reward.tid, category = RewardCategory.Currency, countMin = reward.amount })));
				child.gameObject.SetActive(true);
			}
		}

	}

	public async void OnClick()
	{
		Close();

		await _item.GetReward();


	}
}
