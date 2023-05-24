using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;


public class FloatingText : MonoBehaviour
{

	public TextMeshPro floatingTextMesh;
	public TextMeshPro floatingTextMeshForDamage;
	public TextMeshPro floatingTextMeshForCritical;
	public TextMeshPro floatingTextMeshForCriticalX2;


	private TextMeshPro currentTextMesh;
	private IObjectPool<FloatingText> managedPool;

	public void SetManagedPool(IObjectPool<FloatingText> pool)
	{
		managedPool = pool;
	}

	public void Show(IdleNumber value, Vector3 position, Vector3 endPosition, CriticalType _criticalType)
	{
		gameObject.SetActive(true);


		transform.position = position;
		Camera sceneCam = SceneCamera.it.sceneCamera;

		if (currentTextMesh != null)
		{
			currentTextMesh.gameObject.SetActive(false);
		}
		if (_criticalType == CriticalType.CriticalX2)
		{
			currentTextMesh = floatingTextMeshForCriticalX2;
		}
		else if (_criticalType == CriticalType.Critical)
		{
			currentTextMesh = floatingTextMeshForCritical;
		}
		else
		{
			currentTextMesh = floatingTextMeshForDamage;
		}
		currentTextMesh.gameObject.SetActive(true);
		currentTextMesh.alpha = 1;
		currentTextMesh.text = value.ToFloatingString();

		void FadeFont()
		{
			currentTextMesh.DOFade(0, 0.4f).OnComplete(OnReturnPool);
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
