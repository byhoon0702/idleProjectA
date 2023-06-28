using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;


public class ItemToast : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text = null;
	[SerializeField] private Animator animator;
	private float m_lifeTime;

	public void OnUpdate(string _text)
	{
		text.text = _text;
		VLog.Log($"[Toast] {_text}");
		m_lifeTime = 3;
	}

	public void Skip()
	{
		animator.SetTrigger("end");
	}

	public void EndAnimation()
	{
		gameObject.SetActive(false);
	}

	//private void Update()
	//{
	//	if (m_lifeTime > 0)
	//	{
	//		m_lifeTime -= Time.unscaledDeltaTime;

	//		if (m_lifeTime <= 0)
	//		{
	//			Destroy(gameObject);
	//		}
	//	}
	//}
}
