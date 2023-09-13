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

		attackCount = 0;

		Vector3 dir = (caster.target.position - caster.position).normalized;
		var currentLevel = GetEvolutinData<PowerGeyserLevel>(info.EvolutionLevel);

		float sep_angle = 0;
		int seperate = currentLevel.count;
		float angle = currentLevel.angle;
		sep_angle = angle / Mathf.Max(1, (seperate - 1));

		while (attackCount < info.AttackCount)
		{
			if (caster.IsAlive() == false)
			{
				yield break;
			}
			float multi = 1f;
			GameObject effect = attackCount == info.AttackCount - 1 ? large : small;
			multi = attackCount == info.AttackCount - 1 ? largeRangeMulti : smallRangeMulti;

			skillCameraEffect?.DoEffect(multi);
			List<Vector3> vectors = new List<Vector3>();

			for (int i = 0; i < seperate; i += 2)
			{
				if (i + 1 < seperate)
				{
					float calc_angle = angle - (sep_angle * i);
					Vector3 angled = dir.GetAngledVector3(calc_angle);
					vectors.Add(angled);
					angled = dir.GetAngledVector3(calc_angle, true);
					vectors.Add(angled);
				}
				else
				{
					Vector3 angled = dir.GetAngledVector3(0);
					vectors.Add(angled);
				}
			}

			for (int i = 0; i < vectors.Count; i++)
			{
				Attack(effect, caster, vectors[i] * (distance * (1 + attackCount)) + caster.position, info.HitRange * multi, hitInfo, info);
			}

			attackCount++;

			yield return new WaitForSeconds(info.Interval);
		}
		caster.ChangeState(StateType.IDLE, true);
	}
	void Attack(GameObject copy, Unit caster, Vector3 pos, float hitRange, AffectedInfo hitInfo, SkillInfo info)
	{
		GameObject effect = Instantiate(copy);
		effect.transform.position = pos;
		AffectOverlapCircle(caster, pos, hitRange, pos - caster.position, hitInfo, info);
	}
}
