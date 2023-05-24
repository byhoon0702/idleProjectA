using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemOptionText : MonoBehaviour
{
	[SerializeField] private GameObject gradeObject;
	[SerializeField] private TextMeshProUGUI optionText;
	public void OnUpdate(Grade grade, string text)
	{
		gameObject.SetActive(grade != Grade._END);
		//gradeObject.SetActive(grade != Grade.END);
		optionText.text = text;
	}
}
