using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UI;

public enum TextType
{
	ENEMY_HIT,
	PLAYER_HIT,
	HEAL,
	CRITICAL,
	CRITICAL_X2,
}


public class FloatingText : MonoBehaviour
{
	public TextMeshProUGUI floatingTextMesh;

	[SerializeField] private Image _image;
	[SerializeField] private TMP_ColorGradient healColor;
	[SerializeField] private TMP_ColorGradient playerHitColor;
	[SerializeField] private TMP_ColorGradient enemyHitColor;
	[SerializeField] private TMP_ColorGradient criticalColor;
	[SerializeField] private TMP_ColorGradient superCriticalColor;

	[SerializeField] private float healFontSize;
	[SerializeField] private float playerHitFontSize;
	[SerializeField] private float enemyHitFontSize;
	[SerializeField] private float criticalFontSize;
	[SerializeField] private float superCriticalFontSize;

	private IObjectPool<FloatingText> managedPool;
	Tweener scale;

	Tweener movey;
	public void SetManagedPool(IObjectPool<FloatingText> pool)
	{
		managedPool = pool;
	}

	public void Show(IdleNumber value, Vector3 position, Vector3 endPosition, TextType _textType, Sprite sprite)
	{
		gameObject.SetActive(true);

		Vector2 startPos = GameUIManager.it.ToUIPosition(position);
		Vector2 endPos = GameUIManager.it.ToUIPosition(endPosition);

		RectTransform rect = transform as RectTransform;

		_image.enabled = sprite != null;
		_image.sprite = sprite;
		Camera sceneCam = SceneCamera.it.sceneCamera;

		if (floatingTextMesh != null)
		{
			floatingTextMesh.gameObject.SetActive(false);
		}
		floatingTextMesh.enableVertexGradient = true;
		float scaleSize = 1f;
		switch (_textType)
		{
			case TextType.ENEMY_HIT:
				floatingTextMesh.colorGradientPreset = enemyHitColor;
				scaleSize = enemyHitFontSize;
				break;
			case TextType.PLAYER_HIT:
				floatingTextMesh.colorGradientPreset = playerHitColor;
				scaleSize = playerHitFontSize;
				break;
			case TextType.HEAL:
				floatingTextMesh.colorGradientPreset = healColor;
				scaleSize = healFontSize;
				break;
			case TextType.CRITICAL:
				floatingTextMesh.colorGradientPreset = criticalColor;
				scaleSize = criticalFontSize;
				break;
			case TextType.CRITICAL_X2:
				floatingTextMesh.colorGradientPreset = superCriticalColor;
				scaleSize = superCriticalFontSize;
				break;
		}

		floatingTextMesh.gameObject.SetActive(true);
		floatingTextMesh.alpha = 1;
		floatingTextMesh.text = value.ToString();

		_image.color = Color.white;

		void FadeFont()
		{
			floatingTextMesh.DOFade(0, 0.2f).SetDelay(0.2f).OnComplete(OnReturnPool);
			_image.DOFade(0, 0.2f).SetDelay(0.2f);
			movey = rect.DOAnchorPosY(endPos.y + 0.5f, 0.2f).SetDelay(0.2f);
		}


		endPos.x = startPos.x;
		rect.anchoredPosition = startPos;
		transform.transform.localScale = Vector3.one * 3 * scaleSize;
		scale = transform.DOScale(1 * scaleSize, 0.1f).OnComplete(FadeFont);

	}


	void OnReturnPool()
	{
		//DOTween.Kill(transform);
		scale?.Kill();

		movey?.Kill();
		managedPool.Release(this);
	}
	private void OnDestroy()
	{

		scale?.Kill();
		//movex?.Kill();
		movey?.Kill();
	}

}
