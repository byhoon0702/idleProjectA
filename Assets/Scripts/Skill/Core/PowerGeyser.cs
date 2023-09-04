using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

[System.Serializable]
public class PowerGeyserLevel : SKillEvolutionData
{
	[Header("PowerGeyser")]
	public float angle;
	public int count;
	public GameObject small;
	public GameObject large;
	//public float distance;

}

[CreateAssetMenu(fileName = "PowerGeyser Skill", menuName = "ScriptableObject/Skill/PowerGeyser", order = 1)]
public class PowerGeyser : SkillCore
{
	public PowerGeyserLevel[] evolutionLevels;
	public GameObject small;
	public GameObject large;
	private int attackCount = 0;
	public float distance;

	public float smallRangeMulti = 1;
	public float largeRangeMulti = 1;

	protected override T GetEvolutinData<T>(int level)
	{
		return evolutionLevels[level] as T;
	}
	protected override GameObject ShowEffect(Unit caster, Vector3 pos, int level = 1)
	{
		return base.ShowEffect(caster, pos, level);
	}

	protected override IEnumerator Activation(Unit caster, SkillInfo info, AffectedInfo hitInfo)
	{
		ShowEffect(caster, caster.position, info.EvolutionLevel);
		attackCount = 0;
		Vector3 pos = caster.position;
		Vector3 dir = (caster.target.position - caster.position).normalized;
		Vector3 headingVector = dir;

		var currentLevel = GetEvolutinData<PowerGeyserLevel>(info.EvolutionLevel);

		float angleStep = 0f;
		float halfAngle = 0f;
		if (currentLevel.count > 1)
		{
			angleStep = currentLevel.angle / (currentLevel.count - 1);
			halfAngle = currentLevel.angle / 2f;
		}

		while (attackCount < info.AttackCount)
		{
			if (caster.IsAlive() == false)
			{
				yield break;
			}

			GameObject effect = null;
			float multi = 1;
			for (int i = 0; i < currentLevel.count; i++)
			{
				pos = caster.position;
				Quaternion offset = Quaternion.Euler(0, 0, (angleStep * i) - halfAngle);
				headingVector = offset * dir;
				pos += headingVector;
				pos *= 1 + (distance * attackCount);
				if (attackCount == info.AttackCount - 1)
				{
					effect = Instantiate(large);
					multi = largeRangeMulti;
					skillCameraEffect?.DoEffect();
				}
				else
				{
					effect = Instantiate(small);
					multi = smallRangeMulti;
				}
				effect.transform.position = pos;
				AffectOverlapCircle(caster, pos, info.HitRange * multi, pos - caster.position, hitInfo, info);
			}
			attackCount++;

			yield return new WaitForSeconds(info.Interval);
		}
		caster.ChangeState(StateType.IDLE, true);
	}
}
