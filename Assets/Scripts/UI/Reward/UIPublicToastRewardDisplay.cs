using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIPublicToastRewardDisplay : UIPublicPopupReward
{
	[SerializeField] private Animator animator;

	private Queue<List<AddItemInfo>> rewardDisplayQueue = new Queue<List<AddItemInfo>>();

	public override void Show(List<AddItemInfo> _rewardList)
	{
		gameObject.SetActive(true);

		rewardDisplayQueue.Enqueue(_rewardList);

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("show") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return;
		}

		rewardList = rewardDisplayQueue.Dequeue();
		animator.Play("show", 0, 0);
		OnUpdate();
	}

	public void OnAnimationEnd()
	{
		if (rewardDisplayQueue.Count == 0)
		{
			Close();
			return;
		}

		rewardList = rewardDisplayQueue.Dequeue();
		animator.Play("show", 0, 0);
		OnUpdate();
	}
	public override void OnUpdate()
	{
		uiTextTitle.SetKey("str_ui_reward");

		int differ = rewardList.Count - content.childCount;

		if (differ > 0)
		{
			for (int i = 0; i < differ; i++)
			{
				Instantiate(rewardPrefab, content);
			}
		}

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (i < rewardList.Count)
			{
				UIItemReward uiReward = child.GetComponent<UIItemReward>();
				uiReward.Set(rewardList[i]);
				uiReward.Skip();
			}
			else
			{
				child.gameObject.SetActive(false);
			}

		}
	}

	public void Close()
	{
		gameObject.SetActive(false);
		rewardDisplayQueue.Clear();
		animator.StopPlayback();
	}

}
