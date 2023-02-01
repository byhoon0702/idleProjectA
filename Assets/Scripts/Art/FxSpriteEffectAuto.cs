using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using DG.Tweening;

public class FxSpriteEffectAuto : MonoBehaviour
{
	[SerializeField] private Material m_targetMaterial = null;
	[SerializeField] private float m_startDelay = 0;
	[SerializeField] private float m_duration = 1;
	[SerializeField] private int m_repeatCount = -1;
	[SerializeField] private int m_colums = 0;
	[SerializeField] private int m_rows = 0;
	[SerializeField] private GameObject m_remove_object = null;


	private int m_currentIndex = 0;
	private int m_totalFrame = 0;

	private void OnEnable()
	{
		if (m_targetMaterial != null)
		{
			if (m_remove_object == null) m_remove_object = this.gameObject;
			m_totalFrame = m_colums * m_rows;
			m_currentIndex = 0;
			DOTween.To(() => m_currentIndex, x => m_targetMaterial.SetFloat("_PlayFrame", x), m_totalFrame, m_duration).SetLoops(m_repeatCount, LoopType.Restart).SetDelay(m_startDelay).OnComplete(() => { ResetIndex(); });
		}
		else
		{
			m_currentIndex = 0;
			DOTween.To(() => m_currentIndex, x => m_currentIndex += x, m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); });
		}
	}
	private void CompleteRemoveObject()
	{
		//DestroyImmediate(m_remove_object);
		m_currentIndex = 0;
		m_totalFrame = 0;
	}

	private void ResetIndex()
	{
		m_currentIndex = 0;
		m_targetMaterial.SetFloat("_PlayFrame", 0);
		if (m_remove_object != null)
		{
			CompleteRemoveObject();
		}
	}

	private void OnDisable()
	{
		DOTween.Kill(this);
	}
}
