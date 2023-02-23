using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAdRewardChestItem : MonoBehaviour
{
	[SerializeField] private UIAdRewardChest owner;
	[SerializeField] private RectTransform parentRect;

	[SerializeField] private float appearDuration = 3f;
	[SerializeField] private float stayDuration = 30f;
	[SerializeField] private float disappearDuration = 5f;

	[SerializeField] private int rewardTid;
	[SerializeField] private int rewardCount;

	[Space]
	[SerializeField] private Button openAdButton = null;

	private bool isActivated = false;

	private RectTransform rectTransform;

	public bool IsActivated => isActivated;

	private void OnEnable()
	{
		openAdButton.onClick.RemoveAllListeners();
		openAdButton.onClick.AddListener(OnClickOpenAd);
	}

	public void Show()
	{
		gameObject.SetActive(true);
		isActivated = true;

		rectTransform = GetComponent<RectTransform>();

		Vector2 endPos = new Vector2(parentRect.rect.width / 2, Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2));

		rectTransform.anchoredPosition = new Vector2(parentRect.rect.width, endPos.y);
		rectTransform.DOAnchorPosX(endPos.x, appearDuration).OnComplete(() =>
		{
			rectTransform.DOAnchorPosX(-parentRect.rect.width, disappearDuration).SetDelay(stayDuration)
				.OnComplete(() => EndShow());
		});
	}

	private void EndShow()
	{
		owner.ResetFrequency();

		gameObject.SetActive(false);
		isActivated = false;
	} 

	private void OnClickOpenAd()
	{
		owner.OpenAdReward(rewardTid, (IdleNumber)rewardCount);
		owner.ResetFrequency();
		DOTween.Kill(this);
		gameObject.SetActive(false);
		isActivated = false;
	}
}
