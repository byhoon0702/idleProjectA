using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

[System.Serializable]
public class LanguageDictionary : SerializableDictionary<string, string>
{ }


[System.Serializable]
[CreateAssetMenu(fileName = "Language Manager", menuName = "Language Manager", order = 1)]
public class LanguageContainer : ScriptableObject
{
	[SerializeField] private string path = "uistring.csv";


	public SystemLanguage language;

	[HideInInspector][SerializeField] private LanguageDictionary uiLanguageDictionary;
	public LanguageDictionary UiLanguageDictionary => uiLanguageDictionary;

	public string this[string key]
	{
		get
		{
			if (key.IsNullOrEmpty())
			{
				return "key is null";
			}
			if (uiLanguageDictionary.ContainsKey(key) == false)
			{
				return $"<color=red>{key}</color>";
			}
			string str = "";
			str = uiLanguageDictionary[key];
			if (str.Contains("\\n"))
			{
				str = str.Replace("\\n", "\n");
			}
			return str;
		}
	}

	private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	public void Load(UserDB _parent)
	{

	}

	public void ReadFile()
	{
		TextAsset text = Resources.Load(path) as TextAsset;
		if (text == null)
		{
			Debug.LogWarning($"{path} 에서 파일을 찾을 수 없습니다.");
			return;
		}
		string[] lines = Regex.Split(text.text, LINE_SPLIT_RE);

		uiLanguageDictionary = new LanguageDictionary();
		var line_enumerator = lines.GetEnumerator();
		int startindex = 1;
		int lineIndex = 0;
		try
		{
			while (line_enumerator.MoveNext())
			{
				if (lineIndex == 0)
				{
					lineIndex++;
					continue;
				}
				string line = (string)line_enumerator.Current;
				if (line == "")
				{
					lineIndex++;
					continue;
				}
				string[] subline = Regex.Split(line, SPLIT_RE);
				if (lineIndex == 1)
				{
					int langIndex = 2;

					for (int i = langIndex; i < subline.Length; i++)
					{
						if (GetLocalCode() == subline[i])
						{
							startindex = i;
							break;
						}
					}
					lineIndex++;
					continue;
				}
				if (subline[1].IsNullOrEmpty())
				{
					lineIndex++;
					continue;
				}
				uiLanguageDictionary.Add(subline[1], subline[startindex]);
				lineIndex++;
			}
		}
		catch (System.Exception ex)
		{
			Debug.LogError($"{lineIndex}");
		}
	}
	public string GetLocalCode()
	{
		switch (language)
		{
			case SystemLanguage.Afrikaans: return "af";
			case SystemLanguage.Arabic: return "ar";
			case SystemLanguage.Basque: return "eu";
			case SystemLanguage.Belarusian: return "be";
			case SystemLanguage.Bulgarian: return "bg";
			case SystemLanguage.Catalan: return "ca";
			case SystemLanguage.Chinese: return "zh";
			case SystemLanguage.Czech: return "cs";
			case SystemLanguage.Danish: return "da";
			case SystemLanguage.Dutch: return "nl";
			case SystemLanguage.English: return "en";
			case SystemLanguage.Estonian: return "et";
			case SystemLanguage.Faroese: return "fo";
			case SystemLanguage.Finnish: return "fi";
			case SystemLanguage.French: return "fr";
			case SystemLanguage.German: return "de";
			case SystemLanguage.Greek: return "el";
			case SystemLanguage.Hebrew: return "he";
			case SystemLanguage.Hungarian: return "hu";
			case SystemLanguage.Icelandic: return "is";
			case SystemLanguage.Indonesian: return "id";
			case SystemLanguage.Italian: return "it";
			case SystemLanguage.Japanese: return "ja";
			case SystemLanguage.Korean: return "ko";
			case SystemLanguage.Latvian: return "lv";
			case SystemLanguage.Lithuanian: return "lt";
			case SystemLanguage.Norwegian: return "no";
			case SystemLanguage.Polish: return "pl";
			case SystemLanguage.Portuguese: return "pt";
			case SystemLanguage.Romanian: return "ro";
			case SystemLanguage.Russian: return "ru";
			case SystemLanguage.SerboCroatian: return "sr";
			case SystemLanguage.Slovak: return "sk";
			case SystemLanguage.Slovenian: return "sl";
			case SystemLanguage.Spanish: return "es";
			case SystemLanguage.Swedish: return "sv";
			case SystemLanguage.Thai: return "th";
			case SystemLanguage.Turkish: return "tr";
			case SystemLanguage.Ukrainian: return "uk";
			case SystemLanguage.Vietnamese: return "vi";
			default: return "en";
		}
	}
}
