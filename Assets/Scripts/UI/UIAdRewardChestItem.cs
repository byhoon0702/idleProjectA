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
	Vector2 endPos;
	Vector2 startpos;
	public void Show()
	{
		gameObject.SetActive(true);
		isActivated = true;

		rectTransform = GetComponent<RectTransform>();
		Vector2 parentSize = (rectTransform.parent as RectTransform).sizeDelta;
		endPos = new Vector2(-200, 0);
		startpos = new Vector2(parentSize.x + 200, 0);

		rectTransform.anchoredPosition = startpos;
		//rectTransform.anchoredPosition = new Vector2(parentRect.rect.width, endPos.y);
		//rectTransform.DOAnchorPosX(endPos.x, appearDuration).OnComplete(() =>
		//{
		//	rectTransform.DOAnchorPosX(-parentRect.rect.width, disappearDuration).SetDelay(stayDuration)
		//		.OnComplete(() => EndShow());
		//});
		elapsedTime = 0;
	}

	float elapsedTime = 0;
	void Update()
	{
		if (isActivated)
		{
			Vector2 pos = rectTransform.anchoredPosition;
			pos.y = pos.y + (Mathf.Sin(elapsedTime * 10f) * 10f);
			pos.x = pos.x - (Mathf.Sqrt(12f) * Time.deltaTime);
			rectTransform.anchoredPosition = pos;
			elapsedTime += Time.deltaTime;

			if (rectTransform.anchoredPosition.x > endPos.x)
			{
				EndShow();

			}
		}
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
		//DOTween.Kill(this);
		gameObject.SetActive(false);
		isActivated = false;
	}
}
