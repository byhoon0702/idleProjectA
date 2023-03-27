
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit Hyper Mode", menuName = "ScriptableObject/Unit Hyper Mode", order = 1)]
public class UnitHyperMode : UnitModeBase
{
	public override void OnModeEnter(StateType state)
	{
		OnSpawn(resource);
		unit.unitAnimation = modelAnimation;
		unit.isNormalAttack = false;
		unit.isSkillAttack = false;

		unit.ChangeState(state, true);

		unit.unitAnimation.PlayAnimation("change", -1);
		OnSpawnEffect(unit.position);
		Wait();
	}

	private async void Wait()
	{
		await Task.Delay(2000);
		unit.unitAnimation.PlayAnimation("change", 0);
	}

	public override void OnModeExit()
	{
		modelAnimation.Release();
	}

	public override void OnAttackNormal()
	{
		var skillbase = unit.GetHyperSkillData();
		unit.SetAttack(skillbase);
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
	}
}
