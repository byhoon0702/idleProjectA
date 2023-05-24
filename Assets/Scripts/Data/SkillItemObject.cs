using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillItem", menuName = "ScriptableObject/SkillItem", order = 1)]
public class SkillItemObject : ItemObject
{

	[SerializeField] private bool hideInUI;
	public bool HideInUI => hideInUI;

	[SerializeField] private Grade grade;
	public Grade Grade => grade;
	/// <summary>
	/// 무조건 Instantiate 해서 쓸것
	/// </summary>
	[SerializeField] private NewSkill skill;
	public NewSkill Skill => skill;
	public float cooldownTime;

	public AbilityInfo skillUseAbility;

	public void SetBasicData(long tid, string name, string description, float cooltime)
	{
		this.tid = tid;
		itemName = name;
		this.description = description;
		cooldownTime = cooltime;
	}

	public void SetUseAbility(ItemStats buff)
	{
		AbilityInfo info = new AbilityInfo(buff);
		skillUseAbility = info;
	}

	public void Trigger(Unit caster, Unit target, RuntimeData.SkillInfo skillInfo, AffectedInfo affectedInfo)
	{
		skillUseAbility = skillInfo.skillAbility.Clone();
		target.Hit(affectedInfo as HitInfo);
	}
}
