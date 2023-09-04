using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class UITextMeshPro : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textmeshPro;
	public TextMeshProUGUI TextmeshPro => textmeshPro;
	[SerializeField] private LanguageContainer container;
	[SerializeField] private string key;

	private void OnEnable()
	{
		GetString();
	}


	private void OnGUI()
	{
		if (Application.isPlaying)
		{
			return;
		}
		GetString();
	}
	private void GetString(params object[] param)
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
			return;
		}

		//해당 키가 없으면 키를 그대로 표시하고 폰트 색상을 붉은색으로
		if (container.UiLanguageDictionary.Contains(key) == false)
		{
			textmeshPro.text = $"<color=red>{key}</color>";
			//textmeshPro.text = $"{key}";
			return;
		}

		//키는 있지만 값이 없을 경우 키를 그대로 표시하고 폰트를 노란색으로
		string value = container.UiLanguageDictionary[key];
		if (value.IsNullOrEmpty())
		{
			textmeshPro.text = $"<color=yellow>{key}</color>";
		}
		else
		{
			if (param != null)
			{
				textmeshPro.text = string.Format(value, param);
			}
			else
			{
				textmeshPro.text = value;
			}

		}
	}
	public UITextMeshPro SetKey(string _key, params object[] param)
	{
		key = _key;
		GetString(param);
		return this;
	}
	public void Append(string str)
	{
		textmeshPro.text += str;
	}

	public void OnUpdate()
	{

	}

}
