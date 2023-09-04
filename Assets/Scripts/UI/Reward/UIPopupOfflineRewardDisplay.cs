using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupOfflineRewardDisplay : UIPublicPopupReward, IUIClosable
{
	[SerializeField] private TextMeshProUGUI textTime;
	[SerializeField] private TextMeshProUGUI textKill;
	[SerializeField] protected ScrollRect scrollRect;
	[SerializeField] protected GridLayoutGroup gridLayout;
	[SerializeField] private Button buttonOk;

	private float maxHeight = 600f;

	private Coroutine displayCoroutine;
	public void AddCloseListener()
	{

	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void RemoveCloseListener()
	{

	}

	public void OnClickOk()
	{
		if (displayCoroutine != null)
		{
			StopCoroutine(displayCoroutine);
		}

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (i < rewardList.Count)
			{
				UIItemReward uiReward = child.GetComponent<UIItemReward>();
				uiReward.Skip();
			}
			child.gameObject.SetActive(true);
		}

		Close();
	}

	public void SetInfo(int totalMinutes, int totalKill)
	{
		int hour = totalMinutes / 60;
		int minute = totalMinutes % 60;
		textTime.text = $"{hour.ToString("00")}:{minute.ToString("00")}";
		textKill.text = $"처치 수 : {totalKill}";
	}
	public override void OnUpdate()
	{
		uiTextTitle.SetKey("str_ui_reward");

		int differ = rewardList.Count - content.childCount;

		if (differ > 0)
		{
			for (int i = 0; i < differ; i++)
			{
				Instantiate(rewardPrefab, content);
			}
		}

		int row = rewardList.Count / gridLayout.constraintCount;
		RectTransform rectTransform = (scrollRect.transform as RectTransform);
		//rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Min((row + 1) * (gridLayout.cellSize.y + gridLayout.spacing.y), maxHeight));

		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (i < rewardList.Count)
			{
				UIItemReward uiReward = child.GetComponent<UIItemReward>();
				uiReward.Set(rewardList[i]);
			}
			child.gameObject.SetActive(false);
		}
		displayCoroutine = StartCoroutine(ShowReward());
	}

	private IEnumerator ShowReward()
	{
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (i < rewardList.Count)
			{
				UIItemReward uiReward = child.GetComponent<UIItemReward>();
				uiReward.ShowEffect();
			}
			else
			{
				child.gameObject.SetActive(false);
			}
			scrollRect.verticalNormalizedPosition = 0;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
