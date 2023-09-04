using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectPosType
{
	Target,
	Self,
	HitPosition,
	Effect,
}

[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObject/Skill/New Skill", order = 1)]
public class NewSkill : SkillCore
{

	public SKillEvolutionData[] evolutionData;


	protected override T GetEvolutinData<T>(int level)
	{
		if (evolutionData.Length == 0)
		{
			return null;
		}
		if (evolutionData.Length < level)
		{
			return null;
		}
		return evolutionData[level] as T;
	}


#if UNITY_EDITOR
	public void SetEditorData(SkillData data)
	{
		tid = data.tid;
	}
#endif
	protected override IEnumerator Activation(Unit caster, RuntimeData.SkillInfo info, AffectedInfo hitInfo)
	{
		yield return base.Activation(caster, info, hitInfo);
		if (caster.currentState == StateType.SKILL)
		{
			caster.ChangeState(StateType.IDLE, true);
		}
	}

}
