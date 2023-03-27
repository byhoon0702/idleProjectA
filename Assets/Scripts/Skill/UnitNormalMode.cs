using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Normal Mode", menuName = "ScriptableObject/Unit Normal Mode", order = 1)]
public class UnitNormalMode : UnitModeBase
{
	public override void OnModeEnter(StateType state)
	{
		OnSpawn(resource);
		unit.isNormalAttack = false;
		unit.isSkillAttack = false;

		unit.unitAnimation = modelAnimation;
		unit.ChangeState(state, true);
		//modelAnimation.PlayAnimation(state);

	}

	public override void OnModeExit()
	{
		modelAnimation.Release();

	}

	public override void OnAttackNormal()
	{
		unit.SetAttack(unit.GetSkillEffectData());
		unit.PlayAnimation(StateType.ATTACK);
	}

	public override void OnAttackSkill()
	{

	}

	public override void OnMove()
	{

	}

	public override void OnHit(HitInfo hit)
	{
		//unit.Hit(hit);
		unit.unitAnimation.PlayAnimation(StateType.HIT);
		unit.unitAnimation.PlayDamageWhite();
	}


}
