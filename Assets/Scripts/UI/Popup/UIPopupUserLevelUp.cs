using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopupUserLevelUp : MonoBehaviour
{
	[SerializeField] protected UITextMeshPro uiTextTitle;
	[SerializeField] protected TextMeshProUGUI textLevel;

	[SerializeField] protected GameObject rewardPrefab;
	[SerializeField] protected RectTransform content;

	protected List<AddItemInfo> rewardList;

	[SerializeField] private Animator animator;

	public void Show(int beforeLevel, int afterLevel, List<AddItemInfo> _rewardList = null)
	{
		gameObject.SetActive(true);
		rewardList = _rewardList;
		animator.Play("show", 0, 0);

		uiTextTitle.SetKey("str_ui_levelup");
		textLevel.text = $"{beforeLevel} -> {afterLevel}";
	}

	public void OnAnimationEnd()
	{
		Close();
	}

	private void OnUpdate()
	{

	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

}
