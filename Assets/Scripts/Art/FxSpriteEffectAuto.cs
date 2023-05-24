using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using DG.Tweening;
using System;

public class FxSpriteEffectAuto : MonoBehaviour
{
	[SerializeField] private Material m_targetMaterial = null;
	[SerializeField] private float m_startDelay = 0;
	[SerializeField] private float m_duration = 1;
	[SerializeField] private int m_repeatCount = -1;
	[SerializeField] private int m_colums = 0;
	[SerializeField] private int m_rows = 0;
	[SerializeField] private bool m_isAnimationClip = false;
	[SerializeField] private Animation m_currentAnimationClip = null;

	private int m_currentIndex = 0;
	private int m_totalFrame = 0;

	public bool IsPlaying { get; private set; }

	public void CopyMaterial()
	{
		//m_targetMaterial = Instantiate(m_targetMaterial);
	}
	public void Play(Action _OnCompleteAction = null)
	{
		IsPlaying = true;

		if (m_targetMaterial != null)
		{
			m_totalFrame = m_colums * m_rows;
			m_currentIndex = 0;
			DOTween.To(() => m_currentIndex, x => m_targetMaterial.SetFloat("_PlayFrame", x), m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); _OnCompleteAction?.Invoke(); });
		}
		else
		{
			m_currentIndex = 0;
			DOTween.To(() => m_currentIndex, x => m_currentIndex += x, m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); _OnCompleteAction?.Invoke(); });
		}

		if (m_isAnimationClip == true && m_currentAnimationClip != null)
		{
			m_currentAnimationClip.DOPlay();
			m_currentIndex = 0;
			DOTween.To(() => m_currentIndex, x => m_currentIndex += x, m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); _OnCompleteAction?.Invoke(); });
		}
	}
	public Tween PlayEditor(Action _OnCompleteAction = null)
	{
		Tween m_returnTween = null;
		IsPlaying = true;

		if (m_targetMaterial != null)
		{
			m_totalFrame = m_colums * m_rows;
			m_currentIndex = 0;
			m_returnTween = DOTween.To(() => m_currentIndex, x => m_targetMaterial.SetFloat("_PlayFrame", x), m_totalFrame, m_duration).SetLoops(m_repeatCount, LoopType.Restart).SetDelay(m_startDelay).OnComplete(() => { ResetIndex(); _OnCompleteAction?.Invoke(); });
		}
		else
		{
			m_currentIndex = 0;
			m_returnTween = DOTween.To(() => m_currentIndex, x => m_currentIndex += x, m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); _OnCompleteAction?.Invoke(); });
		}

		if (m_isAnimationClip == true && m_currentAnimationClip != null)
		{
			m_currentAnimationClip.DOPlay();
			m_currentIndex = 0;
			m_returnTween = DOTween.To(() => m_currentIndex, x => m_currentIndex += x, m_totalFrame, m_duration).OnComplete(() => { CompleteRemoveObject(); _OnCompleteAction?.Invoke(); });
		}
		return m_returnTween;
	}

	public void Stop()
	{
		if (m_targetMaterial != null)
		{
			ResetIndex();
		}
		else
		{
			CompleteRemoveObject();
		}

		DOTween.Kill(this);
	}

	private void CompleteRemoveObject()
	{
		IsPlaying = false;
	}

	private void ResetIndex()
	{
		m_currentIndex = 0;
		m_targetMaterial.SetFloat("_PlayFrame", 0);
		CompleteRemoveObject();
	}

	private void OnDisable()
	{
		DOTween.Kill(this);
	}
}
