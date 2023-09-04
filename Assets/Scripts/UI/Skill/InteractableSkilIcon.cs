using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractableSkilIcon : MonoBehaviour
{
	[SerializeField] protected Button button;
	[SerializeField] protected Image icon;

	[SerializeField] protected Image globalCooltimeGauge;
	[SerializeField] protected Image cooltimeGauge;
	[SerializeField] protected TextMeshProUGUI textCooltime;

	[SerializeField] protected GameObject skillIcon;

	[SerializeField] protected GameObject cooltimeIcon; // 쿨타임중


	protected SkillSlot skillSlot;

	public virtual void OnUpdate(SkillSlot _skillSlot = null)
	{
		skillSlot = _skillSlot;

		cooltimeIcon.SetActive(false);
		skillIcon.SetActive(false);
		textCooltime.text = "";
		if (_skillSlot == null || _skillSlot.item == null)
		{
			return;
		}

		skillIcon.SetActive(true);
		icon.sprite = _skillSlot.icon;
		cooltimeGauge.sprite = _skillSlot.icon;
		globalCooltimeGauge.sprite = _skillSlot.icon;
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClickSkillSlot);
	}

	protected virtual void OnClickSkillSlot()
	{
		if (skillSlot == null)
		{
			GameUIManager.it.uiController.ToggleManagement(() => { GameUIManager.it.uiController.BottomMenu.ToggleHero.isOn = false; }, UIManagement.ViewType.Skill);
			return;
		}
		if (skillSlot.IsUsable() == false)
		{
			return;
		}
		if (skillSlot.IsReady() == false)
		{
			return;
		}

		if (UnitManager.it.Player == null)
		{
			return;
		}

		if (UnitManager.it.Player.IsAlive() == false)
		{
			return;
		}
		if (UnitManager.it.Player.IsTargetAlive() == false)
		{
			return;
		}

		if (UnitManager.it.Player.hyperModule.IsHyper)
		{
			return;
		}


		UnitManager.it.Player.TriggerSkill(skillSlot);
		PlatformManager.UserDB.skillContainer.GlobalCooldown();
	}

	protected void Update()
	{
		if (skillSlot == null || skillSlot.IsUsable() == false)
		{
			return;
		}


		if (skillSlot.IsReady() == false)
		{
			globalCooltimeGauge.gameObject.SetActive(skillSlot.GlobalCooldownProgress > 0);
			globalCooltimeGauge.fillAmount = skillSlot.GlobalCooldownProgress / 1f;
			if (cooltimeGauge.gameObject.activeSelf == false)
			{
				cooltimeGauge.gameObject.SetActive(true);
			}
			if (skillSlot.cooldown.coolTime > 0)
			{
				textCooltime.text = $"{skillSlot.cooldown.coolTime.ToString("0.0")}s";
			}
			else
			{
				textCooltime.text = "";
			}
			var ratio = skillSlot.cooldown.Progress();
			cooltimeGauge.fillAmount = ratio;
		}
		else
		{
			textCooltime.text = "";
			if (globalCooltimeGauge.gameObject.activeSelf)
			{
				globalCooltimeGauge.gameObject.SetActive(false);
			}
			if (cooltimeGauge.gameObject.activeSelf)
			{
				cooltimeGauge.gameObject.SetActive(false);
			}
			cooltimeGauge.fillAmount = 0;


		}
	}
}
