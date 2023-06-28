using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DG.Tweening;
using UnityEngine.Pool;

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
	public TextMeshPro floatingTextMesh;

	[SerializeField] private TMP_ColorGradient healColor;
	[SerializeField] private TMP_ColorGradient playerHitColor;
	[SerializeField] private TMP_ColorGradient enemyHitColor;
	[SerializeField] private TMP_ColorGradient criticalColor;

	[SerializeField] private float healFontSize;
	[SerializeField] private float playerHitFontSize;
	[SerializeField] private float enemyHitFontSize;
	[SerializeField] private float criticalFontSize;

	private IObjectPool<FloatingText> managedPool;

	public void SetManagedPool(IObjectPool<FloatingText> pool)
	{
		managedPool = pool;
	}

	public void Show(IdleNumber value, Vector3 position, Vector3 endPosition, TextType _textType)
	{
		gameObject.SetActive(true);

		transform.position = position;
		Camera sceneCam = SceneCamera.it.sceneCamera;

		if (floatingTextMesh != null)
		{
			floatingTextMesh.gameObject.SetActive(false);
		}
		floatingTextMesh.enableVertexGradient = true;
		switch (_textType)
		{
			case TextType.ENEMY_HIT:
				floatingTextMesh.colorGradientPreset = enemyHitColor;
				floatingTextMesh.fontSize = enemyHitFontSize;
				break;
			case TextType.PLAYER_HIT:
				floatingTextMesh.colorGradientPreset = playerHitColor;
				floatingTextMesh.fontSize = playerHitFontSize;
				break;
			case TextType.HEAL:
				floatingTextMesh.colorGradientPreset = healColor;
				floatingTextMesh.fontSize = healFontSize;
				break;
			case TextType.CRITICAL:
				floatingTextMesh.colorGradientPreset = criticalColor;
				floatingTextMesh.fontSize = criticalFontSize;
				break;
			case TextType.CRITICAL_X2:
				floatingTextMesh.colorGradientPreset = criticalColor;
				floatingTextMesh.fontSize = criticalFontSize;
				break;
		}

		floatingTextMesh.gameObject.SetActive(true);
		floatingTextMesh.alpha = 1;
		floatingTextMesh.text = value.ToFloatingString();

		void FadeFont()
		{
			floatingTextMesh.DOFade(0, 0.4f).OnComplete(OnReturnPool);
		}

		Vector2 endPos = endPosition;

		transform.transform.localScale = Vector3.one * 2;
		scale = transform.DOScale(1, 0.2f);
		movex = transform.DOMoveX(endPos.x, 0.2f);
		movey = transform.DOMoveY(endPos.y, 0.1f).OnComplete(FadeFont);
	}

	Tweener scale;
	Tweener movex;
	Tweener movey;
	void OnReturnPool()
	{
		//DOTween.Kill(transform);
		scale?.Kill();
		movex?.Kill();
		movey?.Kill();
		managedPool.Release(this);
	}
	private void OnDestroy()
	{

		scale?.Kill();
		movex?.Kill();
		movey?.Kill();
	}

}
