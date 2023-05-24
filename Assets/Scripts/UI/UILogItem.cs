using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UILogItem : MonoBehaviour
{
	[SerializeField] private Image iconImage;
	[SerializeField] private TextMeshProUGUI countText;
	[SerializeField] CanvasGroup canvasGroup;

	public void ShowLog(int _tid, IdleNumber _count)
	{
		ItemData itemData = DataManager.GetFromAll<ItemData>(_tid);

		countText.text = $"{itemData.name} {_count.ToString()}";

		//SetItemIcon(itemData.Icon);

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

	private void SetItemIcon(string _iconName)
	{
		iconImage.sprite = Resources.Load<Sprite>($"Icon/{_iconName}");
	}

	private void OnDestroy()
	{
		DOTween.Kill(this);
	}
}
