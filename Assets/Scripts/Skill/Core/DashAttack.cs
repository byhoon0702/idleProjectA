using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public class DashEvolutionLevel : SKillEvolutionData
{
	[Header("Dash")]
	public float speed;

}


[CreateAssetMenu(fileName = "Dash Skill", menuName = "ScriptableObject/Skill/Dash Skill", order = 1)]
public class DashAttack : SkillCore
{
	[SerializeField] private DashEvolutionLevel[] evolutionLevels;
	protected override T GetEvolutinData<T>(int level)
	{
		return evolutionLevels[level] as T;
	}

	private WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
	protected override GameObject ShowEffect(Unit caster, Vector3 pos, int level = 1)
	{
		return base.ShowEffect(caster, pos, level);
	}

	protected override IEnumerator Activation(Unit caster, RuntimeData.SkillInfo info, AffectedInfo hitInfo)
	{
		if (caster.target == null)
		{
			yield break;
		}

		var effect = ShowEffect(caster, caster.position);
		int attackCount = 0;

		float distance = Vector3.Distance(caster.target.position, caster.position);
		Vector3 pos = effect.transform.localPosition;
		pos.y += 0.5f;
		effect.transform.localPosition = pos;
		var data = GetEvolutinData<DashEvolutionLevel>(info.EvolutionLevel);
		while (attackCount < info.TargetCount)
		{
			if (caster.target == null)
			{
				Destroy(effect);
				caster.ChangeState(StateType.IDLE, true);
				yield break;
			}
			if (caster.IsAlive() == false)
			{
				Destroy(effect);
				caster.ChangeState(StateType.IDLE, true);
				yield break;
			}

			if (distance > 1)
			{
				Vector2 dirVec = caster.HeadingToTarget();
				caster.rigidbody2D.MovePosition(caster.rigidbody2D.position + dirVec * data.speed * Time.deltaTime);
				yield return null;
			}
			else
			{
				var target = caster.target;
				if (target != null)
				{
					if (info.KnockbackPower > 0)
					{
						target.KnockBack(info.KnockbackPower, (target.position - caster.position).normalized, 1);
					}
					target.Hit(hitInfo as HitInfo, info);

					if (target.IsAlive() == false)
					{
						caster.killCount++;
					}
				}
				skillCameraEffect?.DoEffect();

				caster.RandomTarget(Time.deltaTime, true);
				attackCount++;
				yield return waitForSeconds;
			}
			if (caster.IsTargetAlive())
			{
				distance = Vector3.Distance(caster.target.position, caster.position);
			}
		}

		Destroy(effect);
		caster.ChangeState(StateType.IDLE, true);
	}
}
