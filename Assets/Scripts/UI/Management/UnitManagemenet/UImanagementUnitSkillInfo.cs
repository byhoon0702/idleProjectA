using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UImanagementUnitSkillInfo : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI skillNameText;
	[SerializeField] private TextMeshProUGUI cooltimeText;
	[SerializeField] private TextMeshProUGUI descText;


	public void OnUpdate(SkillData _skillData)
	{
		icon.sprite = Resources.Load<Sprite>($"Icon/{_skillData.Icon}");
		skillNameText.text = _skillData.skillName;
		descText.text = _skillData.skillDesc;

		if (_skillData.isHyperSkill)
		{
			titleText.text = "파이널 어택";
			cooltimeText.text = "하이퍼모드 전용";
		}
		else
		{
			titleText.text = "고유스킬";
			cooltimeText.text = $"쿨타임: {_skillData.cooltime}초";
		}
	}
}
