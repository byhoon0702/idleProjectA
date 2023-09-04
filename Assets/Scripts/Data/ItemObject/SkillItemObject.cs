using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "SkillItem", menuName = "ScriptableObject/SkillItem", order = 1)]
public class SkillItemObject : ItemObject
{

	[SerializeField] private bool hideInUI;
	public bool HideInUI => hideInUI;

	[SerializeField] private Grade grade;
	public Grade Grade => grade;
	/// <summary>
	/// 무조건 Instantiate 해서 쓸것
	/// </summary>
	[SerializeField] private SkillCore skill;
	public SkillCore Skill => skill;
	public float cooldownTime;

	public RuntimeData.AbilityInfo skillUseAbility;

	public override void SetBasicData<T>(T data)
	{
		var skillData = data as SkillData;
		this.tid = skillData.tid;
		itemName = skillData.name;
		this.description = skillData.description;
		cooldownTime = skillData.detailData.cooldownValue;
	}

	public void SetUseAbility(ItemStats buff)
	{
		RuntimeData.AbilityInfo info = new RuntimeData.AbilityInfo(buff);
		skillUseAbility = info;
	}


}
