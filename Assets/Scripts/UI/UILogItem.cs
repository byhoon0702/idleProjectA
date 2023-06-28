using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using RuntimeData;

public class UILogItem : MonoBehaviour
{
	[SerializeField] private Image iconImage;
	[SerializeField] private TextMeshProUGUI countText;
	[SerializeField] CanvasGroup canvasGroup;

	public void ShowLog(RewardInfo info, IdleNumber _count)
	{
		countText.text = $"{info.name} +{_count.ToString()}";

		iconImage.sprite = info.iconImage;

		DOTween.Kill(this);
		canvasGroup.alpha = 0f;

		canvasGroup.DOFade(1f, 0.2f)
			.SetEase(Ease.Linear)
			.OnComplete(() =>
			{
				canvasGroup.DOFade(1f, 0.5f)
					.SetEase(Ease.Linear)
					.OnComplete(() =>
					{
						canvasGroup.DOFade(0f, 0.3f)
							.SetEase(Ease.Linear);
					});
			});
	}



	private void OnDestroy()
	{
		DOTween.Kill(this);
	}
}
