using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRewardLog : MonoBehaviour
{
	[SerializeField] private UILogItem[] uiLogItems;

	private int currentIndex = 0;


	public void ShowLog(int _tid, IdleNumber _count)
	{
		UILogItem item = GetCurrentLogItem();
		item.ShowLog(_tid, _count);
		item.transform.SetAsFirstSibling();
	}

	private UILogItem GetCurrentLogItem()
	{
		currentIndex = currentIndex % uiLogItems.Length;
		UILogItem item = uiLogItems[currentIndex];

		currentIndex++;
		return item;
	}
}
