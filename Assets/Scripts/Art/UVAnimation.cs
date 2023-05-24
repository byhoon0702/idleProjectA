using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class UVAnimation : MonoBehaviour
{
	public float duration;
	public SpriteRenderer spriteRenderer;
	public MaterialPropertyBlock block;
	private float progress;

	private Action onComplete;
	private bool isStart = false;
	public void Play(Action _onComplete)
	{
		onComplete = _onComplete;
		progress = 0;
		isStart = true;
		block = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(block);
		block.SetFloat("_Duration", duration);
		spriteRenderer.SetPropertyBlock(block);
	}

	void Update()
	{
		if (!isStart)
		{
			return;
		}
		progress += Time.deltaTime;
		block.SetFloat("_Progress", progress);
		spriteRenderer.SetPropertyBlock(block);
		if (progress >= duration)
		{
			isStart = false;
			if (onComplete == null)
			{

				Destroy(gameObject);
				return;
			}
			onComplete?.Invoke();

		}
	}
}
