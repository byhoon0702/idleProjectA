using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class UITalkBubble : MonoBehaviour
{

	[SerializeField] private RectTransform rootTransform;
	[SerializeField] private RectTransform scrollRootTransform;
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private Image imageBubbleTail;
	[SerializeField] private TextMeshProUGUI textSpeech;


	[SerializeField] private float maxWidth;
	[SerializeField] private float maxHeight;

	private Vector2 pivotLeft = Vector2.zero;
	private Vector2 pivotRight = new Vector2(1, 0);

	private bool isAddingRichText = false;
	private float typingSpeed = 0.04f;
	private Coroutine typingCoroutine;

	private Unit follow;
	private RectTransform selfRect => transform as RectTransform;


	public void Close()
	{
		if (typingCoroutine != null)
		{
			StopCoroutine(typingCoroutine);

		}
		gameObject.SetActive(false);
	}

	public void Show(string dialogue, Unit speaker, bool instantly = false, System.Action onComplete = null)
	{
		follow = speaker;

		Vector2 pos = GameUIManager.it.ToUIPosition(follow.HeadPosition);

		Show(dialogue, pos, follow.currentDir == 1, instantly, onComplete);
	}

	private void SetPosition(Vector2 pos, bool isRight = false)
	{
		if (isRight)
		{
			rootTransform.pivot = pivotRight;
			imageBubbleTail.rectTransform.anchorMin = pivotRight;
			imageBubbleTail.rectTransform.anchorMax = pivotRight;
			imageBubbleTail.rectTransform.localScale = new Vector3(-1, 1, 1);
			imageBubbleTail.rectTransform.anchoredPosition = new Vector2(-10, 4);
		}
		else
		{
			rootTransform.pivot = pivotLeft;
			imageBubbleTail.rectTransform.anchorMin = pivotLeft;
			imageBubbleTail.rectTransform.anchorMax = pivotLeft;
			imageBubbleTail.rectTransform.localScale = new Vector3(1, 1, 1);
			imageBubbleTail.rectTransform.anchoredPosition = new Vector2(10, 4);
		}
		selfRect.anchoredPosition = pos;
	}
	public void Show(string dialogue, Vector2 pos, bool isRight = false, bool instantly = false, System.Action onComplete = null)
	{
		SetPosition(pos, isRight);
		textSpeech.maxVisibleCharacters = 0;
		typingCoroutine = StartCoroutine(TextAnimation(dialogue, instantly, onComplete));
	}

	public IEnumerator TextAnimation(string dialogue, bool showInstantly = false, System.Action onComplete = null)
	{
		textSpeech.text = dialogue;
		Vector2 size = textSpeech.GetPreferredValues(dialogue, maxWidth, maxHeight);
		Vector2 sizeDelta = size;

		if (sizeDelta.x > maxWidth)
		{
			sizeDelta.x = maxWidth;
		}

		textSpeech.rectTransform.sizeDelta = sizeDelta;

		if (sizeDelta.y > maxHeight)
		{
			sizeDelta.y = maxHeight;
		}

		RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
		scrollRootTransform.sizeDelta = new Vector2(Mathf.CeilToInt(sizeDelta.x + scrollRectTransform.offsetMin.x + Mathf.Abs(scrollRectTransform.offsetMax.x)),
												Mathf.CeilToInt(sizeDelta.y + scrollRectTransform.offsetMin.y + Mathf.Abs(scrollRectTransform.offsetMax.y)));


		if (showInstantly)
		{
			textSpeech.maxVisibleCharacters = dialogue.Length;
			yield return new WaitForSeconds(0.5f);

			onComplete?.Invoke();
			Close();

			yield break;
		}
		var charArray = dialogue.ToCharArray();

		int index = 0;
		while (index < charArray.Length)
		{

			char c = charArray[index];
			if (c == '<' || isAddingRichText)
			{
				isAddingRichText = true;
				if (c == '>')
				{
					isAddingRichText = false;
				}
			}
			else
			{
				textSpeech.maxVisibleCharacters++;

				yield return new WaitForSeconds(typingSpeed);
			}
			scrollRect.verticalNormalizedPosition = 0;
			index++;

		}

		yield return new WaitForSeconds(0.5f);
		onComplete?.Invoke();
		Close();
	}

	void Update()
	{
		if (follow != null)
		{
			Vector2 pos = GameUIManager.it.ToUIPosition(follow.HeadPosition);
			SetPosition(pos, follow.currentDir == 1);
		}
	}
}
