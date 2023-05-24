using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class UITextMeshPro : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textmeshPro;
	[SerializeField] private LanguageContainer container;
	[SerializeField] private string key;

	private void OnEnable()
	{
		GetString();
	}


	private void OnGUI()
	{
		GetString();
	}
	private void GetString()
	{

		if (container == null)
		{
			container = (LanguageContainer)Resources.Load("RuntimeDatas/Containers/Language Container");
			if (container == null)
			{
				return;
			}
		}
		if (textmeshPro == null)
		{
			textmeshPro = GetComponent<TextMeshProUGUI>();
			if (textmeshPro == null)
			{
				return;
			}
		}
		if (key.IsNullOrEmpty())
		{
			//textmeshPro.text = "";
			return;
		}
		if (container.UiLanguageDictionary.Contains(key) == false)
		{
			textmeshPro.text = $"<color=red>{key}</color>";
			return;
		}

		string value = container.UiLanguageDictionary[key];
		if (value.IsNullOrEmpty())
		{
			textmeshPro.text = $"<color=yellow>{key}</color>";
		}
		else
		{
			textmeshPro.text = container.UiLanguageDictionary[key];
		}

	}

	public void OnUpdate()
	{

	}

}
