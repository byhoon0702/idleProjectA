using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICoreAbilityProbabilityPopup : MonoBehaviour, IUIClosable
{
	[SerializeField] private Button closeButton;
	[SerializeField] private TextMeshProUGUI abilityText;
	[SerializeField] private TextMeshProUGUI[] gradeTextList;




	private void Awake()
	{
		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(() =>
		{
			if (Closable())
			{
				Close();
			}
		});
	}

	public void OnUpdate()
	{
		var gradeList = DataManager.Get<UserGradeDataSheet>().GetGradeList();
		var abilitySheet = DataManager.Get<CoreAbilityDataSheet>();

		string text = "";
		foreach (Ability v in abilitySheet.GetAbilityTypes())
		{
			text += $"{GetAbilityText(v)}\n";
		}

		abilityText.text = text;


		var probabilitySheet = DataManager.Get<CoreAbilityProbabilityDataSheet>();
		double accum = 0;
		for (int i = 0; i < gradeList.Count; i++)
		{
			double before = accum;
			accum += probabilitySheet.infos[i].probability;
			double after = accum;
			gradeTextList[i].text = $"{gradeList[i]}\n{((after - before) * 100).ToString("F1")}%";
		}
	}

	public string GetAbilityText(Ability _ability)
	{
		UserInfo.coreAbil.abilityGenerator.GetAbilityValueRange(Grade.D, _ability, out var d_min, out var d_max);
		UserInfo.coreAbil.abilityGenerator.GetAbilityValueRange(Grade.D, _ability, out var sss_min, out var sss_max);

		var sheet = DataManager.Get<StatusDataSheet>();
		return $"{sheet.GetData(_ability).description} ({d_min.ToString()} ~ {sss_max.ToString()})";
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
