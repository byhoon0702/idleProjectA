using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractableSkilIcon : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private Image[] icons;

	[SerializeField] private Image cooltimeGauge;
	[SerializeField] private Image remainTimeGauge;

	[SerializeField] private GameObject skillIcon;
	[SerializeField] private GameObject usingIcon; // 사용중
	[SerializeField] private GameObject cooltimeIcon; // 쿨타임중
	[SerializeField] private GameObject useableIcon; // 사용가능


	private SkillBase skill;




	public void OnUpdate(SkillBase _skill = null)
	{
		skill = _skill;

		usingIcon.SetActive(false);
		cooltimeIcon.SetActive(false);
		useableIcon.SetActive(false);
		skillIcon.SetActive(false);

		Sprite iconSprite;
		if (skill == null)
		{
			iconSprite = Resources.Load<Sprite>($"Icon/_default");
		}
		else
		{
			iconSprite = Resources.Load<Sprite>($"Icon/{_skill.skillIcon}");
			skillIcon.SetActive(true);
		}

		foreach (var v in icons)
		{
			v.sprite = iconSprite;
		}

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() =>
		{
			if (skill.Usable())
			{
				skill.Action();
			}
		});
	}

	private void Update()
	{
		if (skill == null)
		{
			return;
		}

		float cooltimeRatio = skill.remainCooltime / skill.cooltime;
		cooltimeGauge.fillAmount = cooltimeRatio;

		float usingRatio = skill.skillUseRemainTime / skill.skillUseTime;
		remainTimeGauge.fillAmount = usingRatio;

		usingIcon.SetActive(usingRatio > 0);
		cooltimeIcon.SetActive(cooltimeRatio > 0);
		useableIcon.SetActive(usingIcon.activeInHierarchy == false && cooltimeIcon.activeInHierarchy == false);
	}
}
