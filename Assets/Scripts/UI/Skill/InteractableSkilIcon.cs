using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InteractableSkilIcon : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private Image icon;

	[SerializeField] private Image globalCooltimeGauge;
	[SerializeField] private Image cooltimeGauge;


	[SerializeField] private GameObject skillIcon;

	[SerializeField] private GameObject cooltimeIcon; // 쿨타임중


	SkillSlot skillSlot;

	public void OnUpdate(SkillSlot _skillSlot = null)
	{
		skillSlot = _skillSlot;

		cooltimeIcon.SetActive(false);
		skillIcon.SetActive(false);

		if (_skillSlot == null || _skillSlot.item == null)
		{
			return;
		}

		skillIcon.SetActive(true);
		icon.sprite = _skillSlot.icon;
		cooltimeGauge.sprite = _skillSlot.icon;
		globalCooltimeGauge.sprite = _skillSlot.icon;
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() =>
		{
			if (skillSlot == null)
			{
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
			GameManager.UserDB.skillContainer.GlobalCooldown();
			//if (skillSlot.item != null)
			//{

			//	skillSlot.Trigger(UnitManager.it.Player);
			//}
		});
	}

	private void Update()
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

			var ratio = skillSlot.cooldown.Progress();
			cooltimeGauge.fillAmount = ratio;
		}
		else
		{
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
