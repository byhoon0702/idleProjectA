using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;



public delegate void OnSkillCoolDown(float value);

public class SkillModule : MonoBehaviour
{
	public SkillSlot defaultSkill;
	public SkillSlot finishSkillSlot;
	public Dictionary<SkillActiveType, List<SkillSlot>> skillDictionary;

	public event OnSkillCoolDown onSkillCoolDown;
	[SerializeField] private Unit caster;

	private SkillSlot currentSlot;
	private HitInfo info;
	public void Init(Unit _caster, long defaultSkillTid)
	{
		caster = _caster;
		skillDictionary = new Dictionary<SkillActiveType, List<SkillSlot>>();
		skillDictionary.Add(SkillActiveType.PASSIVE, new List<SkillSlot>());
		skillDictionary.Add(SkillActiveType.ACTIVE, new List<SkillSlot>());

		defaultSkill = new SkillSlot();

		if (defaultSkillTid != 0)
		{
			defaultSkill.item = PlatformManager.UserDB.skillContainer.skillList.Find(x => x.Tid == defaultSkillTid);
		}
	}

	public void ChangeSkillSet(SkillSlot[] slots)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			var slot = slots[i];
			if (slot == null || slot.item == null || slot.item.Tid == 0)
			{
				continue;
			}
			AddSkill(slots[i]);
		}
	}

	public void DefaultAttack()
	{
		defaultSkill.Trigger(caster);
	}

	// Update is called once per frame
	void Update()
	{
		if (caster == null)
		{
			return;
		}

		if (caster is PlayerUnit)
		{
			if ((caster as PlayerUnit).hyperModule != null && (caster as PlayerUnit).hyperModule.IsHyper)
			{
				return;
			}
		}


		if (caster.IsAlive())
		{
			OnUpdateActiveSkill();
			OnUpdatePassiveSkill();
		}
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
		if (slot.item.activeType == SkillActiveType.PASSIVE)
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
		if (info.item == null)
		{ return; }
		if (info.item.activeType == SkillActiveType.PASSIVE)
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
				skill.Use();
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
				if (caster is PlayerUnit && PlatformManager.UserDB.skillContainer.isAutoSkill)
				{
					if (caster.IsTargetAlive())
					{
						caster.TriggerSkill(skill);
						PlatformManager.UserDB.skillContainer.GlobalCooldown();
					}
				}

				if (caster is Pet)
				{
					if (caster.TriggerSkill(skill))
					{
						PlatformManager.UserDB.skillContainer.PetGlobalCooldown();
					}
				}

				if (caster is EnemyUnit)
				{
					if (caster.IsTargetAlive())
					{
						caster.TriggerSkill(skill);
					}
				}
			}
			else
			{
				skill.Cooldown();
				skill.GlobalCooldown(Time.deltaTime);
			}
		}
	}

	Action onComplete;
	public void RegisterUsingSkill(SkillSlot skillSlot, HitInfo hitInfo)
	{
		currentSlot = skillSlot;
		info = hitInfo;

	}
	public void ActivateSkill()
	{
		if (currentSlot == null)
		{
			return;
		}
		currentSlot.Trigger(caster);
		currentSlot = null;
		//currentSlot.item.itemObject.Trigger(caster, )
		//skill.Trigger(caster, currentSlot.item, info);
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

		currentSlot = null;
	}
}
