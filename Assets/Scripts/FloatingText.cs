using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class FloatingText : MonoBehaviour
{
	public TextMeshProUGUI floatingTextMesh;
	public void Show(string text, Color color, Vector2 position)
	{
		gameObject.SetActive(true);
		floatingTextMesh.color = color;
		floatingTextMesh.text = text;

		(transform as RectTransform).anchoredPosition = position;
		StartCoroutine(Wait());
	}

	IEnumerator Wait()
	{

		yield return new WaitForSeconds(.5f);
		gameObject.SetActive(false);
	}

	void OnDisable()
	{

	}
}
