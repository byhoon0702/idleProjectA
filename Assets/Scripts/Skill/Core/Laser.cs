using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;


[CreateAssetMenu(fileName = "Laser", menuName = "ScriptableObject/Skill/Laser", order = 1)]
public class Laser : SkillCore
{
	public float width = 1f;
	public GameObject laser;
	private LineRenderer lineRenderer;
	protected override GameObject ShowEffect(Unit caster, Vector3 pos, int level = 1)
	{
		GameObject go = Instantiate(laser);
		lineRenderer = go.GetComponent<LineRenderer>();
		lineRenderer.startWidth = width;
		lineRenderer.SetPosition(0, caster.position);
		lineRenderer.SetPosition(1, (caster.target.position - caster.position) * 10f);

		Destroy(lineRenderer.gameObject, 0.5f);

		return go;
	}
	protected override IEnumerator Activation(Unit caster, SkillInfo info, AffectedInfo hitInfo)
	{
		ShowEffect(caster, caster.position, info.EvolutionLevel);
		int attackCount = 0;
		while (attackCount < info.AttackCount)
		{
			if (caster.IsAlive() == false)
			{
				yield break;
			}
			Vector2 casterPos = caster.transform.position;
			Vector2 targetPos = caster.target.position;
			//var colliders = Physics2D.CircleCastAll(caster.transform.position, width, targetPos - casterPos, 10f, layer);

			//for (int i = 0; i < colliders.Length; i++)
			//{
			//	Unit target = colliders[i].transform.GetComponent<Unit>();
			//	target.Hit(hitInfo);

			//	if (target.IsAlive() == false)
			//	{
			//		caster.killCount++;
			//	}
			//}
			AffectLaser(caster, casterPos, width, targetPos - casterPos, hitInfo as HitInfo, info);
			attackCount++;
			yield return new WaitForSeconds(info.Interval);
		}

		yield return null;

		caster.ChangeState(StateType.IDLE);
	}
	protected void AffectLaser(Unit caster, Vector3 pos, float hitrange, Vector3 knockbackDir, HitInfo hitInfo, RuntimeData.SkillInfo skillInfo)
	{
		Vector2 casterPos = caster.transform.position;
		Vector2 targetPos = caster.target.position;

		var colliders = Physics2D.CircleCastAll(pos, hitrange, targetPos - casterPos, 10f, hitInfo.targetLayer);
		for (int i = 0; i < colliders.Length; i++)
		{
			Unit target = colliders[i].transform.GetComponent<Unit>();
			HitTarget(caster, target, hitInfo, skillInfo.KnockbackPower, knockbackDir, skillInfo);
		}
	}
}
