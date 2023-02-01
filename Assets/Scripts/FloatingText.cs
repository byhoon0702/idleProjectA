using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UIElements;

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

	public void Show(string text, Color color, Vector2 position, CriticalType _criticalType, bool isPlayer = false)
	{
		gameObject.SetActive(true);
		//// floatingTextMesh.color = color;
		//if (color != Color.white)
		//{
		//	if (color == Color.magenta)
		//	{
		//		currentTextMesh = floatingTextMeshForCritical;
		//		floatingTextMeshForDamage.gameObject.SetActive(false);
		//	}
		//	else
		//	{
		//		floatingTextMeshForCritical.gameObject.SetActive(false);
		//		currentTextMesh = floatingTextMeshForDamage;
		//	}
		//	floatingTextMesh.gameObject.SetActive(false);

		//}
		//else
		//{
		//	floatingTextMeshForDamage.gameObject.SetActive(false);
		//	floatingTextMeshForCritical.gameObject.SetActive(false);
		//	currentTextMesh = floatingTextMesh;
		//}

		//if (!currentTextMesh.gameObject.activeSelf) currentTextMesh.gameObject.SetActive(true);
		//currentTextMesh.text = text;
		//currentTextMesh.color = new Color(1, 1, 1, 1); // DoFade 때문에 Alpha가 0인 상태로 넘어옴
		//											   //rectTransform = (transform as RectTransform);
		//											   //rectTransform.anchoredPosition = position;
		transform.position = position;
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
		currentTextMesh.text = text;

		void FadeFont()
		{
			currentTextMesh.DOFade(0, 0.4f).OnComplete(OnReturnPool);
		}

		Vector2 endPos = new Vector2(position.x + Random.Range(0.1f, 0.5f), position.y + Random.Range(0.1f, 0.5f));
		if (isPlayer)
		{
			endPos.x = position.x - Random.Range(1, 5);
		}
		transform.DOMoveX(endPos.x, 0.4f);
		transform.DOMoveY(endPos.y + 1, 0.6f).OnComplete(FadeFont);
	}

	void OnReturnPool()
	{
		managedPool.Release(this);
	}

	void OnDisable()
	{
		DOTween.Kill(this);
	}
}
