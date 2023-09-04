using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAdRewardChestItem : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private UIAdRewardChest owner;
	[SerializeField] private RectTransform parentRect;


	[Space]
	[SerializeField] private Button openAdButton = null;

	private bool isActivated = false;

	public bool IsActivated => isActivated;

	private void OnEnable()
	{
		openAdButton.onClick.RemoveAllListeners();
		openAdButton.onClick.AddListener(OnClickOpenAd);
	}

	GameObject chectObject;
	public void Show()
	{
		gameObject.SetActive(true);
		isActivated = true;
		if (chectObject != null)
		{
			Destroy(chectObject);
		}
		//if (PlatformManager.UserDB.inventory.SelectRewardChest.uiPrefab == null)
		//{
		//	return;
		//}
		if (transform == null)
		{
			return;
		}

		//chectObject = Instantiate(PlatformManager.UserDB.inventory.SelectRewardChest.uiPrefab, transform.position, Quaternion.identity, transform);
		animator.Play("show");
	}

	public void EndShow()
	{
		animator.StopPlayback();
		gameObject.SetActive(false);
		isActivated = false;

		if (chectObject != null)
		{
			Destroy(chectObject);
		}
		owner.Init();
	}

	private void OnClickOpenAd()
	{
		EndShow();
		owner.OpenPopup();
	}
}
