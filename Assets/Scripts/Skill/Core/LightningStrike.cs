using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Skill", menuName = "ScriptableObject/Skill/Lightning Skill", order = 1)]

public class LightningStrike : SkillCore
{
	public SKillEvolutionData[] evolutionData;
	public float interval;
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

	protected override IEnumerator Activation(Unit caster, SkillInfo info, AffectedInfo hitInfo)
	{
		if (caster.IsAlive() == false)
		{
			yield break;
		}

		if (caster is EnemyUnit)
		{
			Vector3 pos = caster.target != null ? caster.target.position : caster.position;
			caster.skillModule.StartCoroutine(AffectNonTarget(caster, info, hitInfo, pos));
		}
		else
		{
			for (int i = 0; i < info.TargetCount; i++)
			{
				var list = UnitManager.it.GetRandomEnemies(caster.position, info.AttackRange);
				for (int ii = 0; ii < list.Count; ii++)
				{
					caster.skillModule.StartCoroutine(AffectNonTarget(caster, info, hitInfo, list[ii].position));

				}
				yield return new WaitForSeconds(interval);
			}
		}

		yield return null;
	}
	protected override IEnumerator AffectNonTarget(Unit caster, RuntimeData.SkillInfo info, AffectedInfo hitInfo, Vector3 targetPos)
	{
		GameObject effect = ShowEffect(caster, targetPos, info.EvolutionLevel);
		Vector3 pos = GetPos(caster, effect, targetPos);
		int attackCount = 0;
		while (attackCount < info.AttackCount)
		{
			if (caster.IsAlive() == false)
			{
				yield break;
			}
			if (effectPosType == EffectPosType.Self)
			{
				pos = GetPos(caster, effect, targetPos);
			}

			OnAffect(caster, pos, info.HitRange, pos - caster.position, hitInfo, info);

			skillCameraEffect?.DoEffect();
			attackCount++;
			if (info.Duration == 0)
			{
				yield return null;
			}
			else
			{
				yield return new WaitForSeconds(info.Duration / info.AttackCount);
			}
		}
	}
}
