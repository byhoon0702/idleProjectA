using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour
{

	public TextMeshProUGUI floatingTextMesh;
	private RectTransform rectTransform;

	private IObjectPool<FloatingText> managedPool;

	public void SetManagedPool(IObjectPool<FloatingText> pool)
	{
		managedPool = pool;
	}

	public void Show(string text, Color color, Vector2 position, bool critical, bool isPlayer = false)
	{
		gameObject.SetActive(true);
		floatingTextMesh.color = color;
		floatingTextMesh.text = text;
		rectTransform = (transform as RectTransform);
		rectTransform.anchoredPosition = position;

		if(critical)
		{
			floatingTextMesh.transform.localScale = Vector3.one * 2;
		}
		else
		{
			floatingTextMesh.transform.localScale = Vector3.one;
		}


		void FadeFont()
		{
			floatingTextMesh.DOFade(0, 0.4f).OnComplete(OnReturnPool);
		}

		Vector2 endPos = new Vector2(position.x + 100, position.y + 70);
		if (isPlayer)
		{
			endPos.x = position.x - 100;
		}
		rectTransform.DOAnchorPosX(endPos.x, 0.4f);
		rectTransform.DOAnchorPosY(endPos.y + 70, 0.6f).OnComplete(FadeFont);
		//StartCoroutine(Wait());

	}

	void OnReturnPool()
	{
		managedPool.Release(this);
	}

	void OnDisable()
	{

	}
}
