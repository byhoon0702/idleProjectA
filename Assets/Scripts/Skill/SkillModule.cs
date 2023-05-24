using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;



public delegate void OnSkillCoolDown(float value);

public class SkillModule : MonoBehaviour
{
	public SkillSlot defaultSkill;
	public Dictionary<SkillActiveType, List<SkillSlot>> skillDictionary;

	public event OnSkillCoolDown onSkillCoolDown;
	[SerializeField] private Unit caster;

	public void Init(Unit _caster, long defaultSkillTid)
	{
		skillDictionary = new Dictionary<SkillActiveType, List<SkillSlot>>();
		skillDictionary.Add(SkillActiveType.PASSIVE, new List<SkillSlot>());
		skillDictionary.Add(SkillActiveType.ACTIVE, new List<SkillSlot>());

		defaultSkill = new SkillSlot();

		if (defaultSkillTid == 0)
		{
			return;
		}
		caster = _caster;
		defaultSkill.item = GameManager.UserDB.skillContainer.skillList.Find(x => x.tid == defaultSkillTid);//new RuntimeData.SkillInfo(defaultSkillTid);
	}

	public void DefaultAttack(HitInfo hitinfo)
	{
		defaultSkill.Trigger(caster);
	}

	// Update is called once per frame
	void Update()
	{
		OnUpdateActiveSkill();
		OnUpdatePassiveSkill();
	}
	public void ResetSkills()
	{

		foreach (var skills in skillDictionary)
		{
			for (int i = 0; i < skills.Value.Count; i++)
			{
				SkillSlot slot = skills.Value[i];
				slot.ResetCoolDown(caster);
			}
		}
	}

	public void AddSkill(SkillSlot slot)
	{
		if (slot.item.rawData.activeType == SkillActiveType.PASSIVE)
		{
			var skill = skillDictionary[SkillActiveType.PASSIVE].Find(x => x.itemTid == slot.itemTid);
			if (skill == null)
			{
				skillDictionary[SkillActiveType.PASSIVE].Add(slot);
			}
		}
		else
		{

			var skill = skillDictionary[SkillActiveType.ACTIVE].Find(x => x.itemTid == slot.itemTid);
			if (skill == null)
			{
				skillDictionary[SkillActiveType.ACTIVE].Add(slot);
			}

		}
	}

	public void RemoveSkill(SkillSlot info)
	{
		if (info.item.rawData.activeType == SkillActiveType.PASSIVE)
		{
			var skill = skillDictionary[SkillActiveType.PASSIVE].Find(x => x.itemTid == info.itemTid);
			if (skill == null)
			{
				skillDictionary[SkillActiveType.PASSIVE].Remove(info);
			}
		}
		else
		{

			var skill = skillDictionary[SkillActiveType.ACTIVE].Find(x => x.itemTid == info.itemTid);
			if (skill == null)
			{
				skillDictionary[SkillActiveType.ACTIVE].Remove(info);
			}

		}
	}

	void OnUpdatePassiveSkill()
	{
		var list = skillDictionary[SkillActiveType.PASSIVE];
		if (list.Count == 0)
		{
			return;
		}

		for (int i = 0; i < list.Count; i++)
		{
			var skill = list[i];
			if (skill.IsUsable() == false)
			{
				continue;
			}
			if (skill.IsReady())
			{
				skill.Trigger(caster);
			}
			else
			{
				skill.Cooldown();
				skill.GlobalCooldown(Time.deltaTime);
			}
		}
	}

	void OnUpdateActiveSkill()
	{


		var list = skillDictionary[SkillActiveType.ACTIVE];

		for (int i = 0; i < list.Count; i++)
		{
			var skill = list[i];
			if (skill.IsUsable() == false)
			{
				continue;
			}

			if (skill.IsReady())
			{
				if (caster is PlayerUnit && GameManager.UserDB.skillContainer.isAutoSkill)
				{
					caster.TriggerSkill(skill);
				}

			}
			else
			{
				skill.Cooldown();
				skill.GlobalCooldown(Time.deltaTime);
			}
		}
	}

	public void ActivateSkill(SkillSlot skillSlot, HitInfo hitInfo)
	{
		if (skillSlot == null || skillSlot.IsUsable() == false)
		{
			return;
		}

		if (skillSlot.IsReady())
		{
			skillSlot.Trigger(caster);
		}
	}
}
