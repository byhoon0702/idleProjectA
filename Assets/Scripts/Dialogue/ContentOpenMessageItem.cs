using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContentOpenMessageItem : MonoBehaviour
{
	public Animator animator;
	public TextMeshProUGUI title;

	public Transform content;
	public GameObject itemprefab;

	public ContentOpenMessagePool pool;

	public void Display(ContentOpenMessage message)
	{
		animator.Play("show");
		title.text = message.message;

		int count = message.displayItems != null ? message.displayItems.Count : 0;

		content.CreateListCell(count, itemprefab);

		content.gameObject.SetActive(count > 0);

		for (int i = 0; i < content.childCount; i++)
		{
			var child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < count)
			{
				if (message.displayItems[i].tid == 0)
				{
					continue;
				}
				child.GetComponent<UIItemReward>().Set(message.displayItems[i]);
				child.gameObject.SetActive(true);
			}
		}

	}

	public void OnAnimationEnd()
	{
		pool.Release(this);
	}
}
