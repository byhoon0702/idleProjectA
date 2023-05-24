
//[CreateAssetMenu(fileName = "Unit Normal Mode", menuName = "ScriptableObject/Unit Normal Mode", order = 1)]
public class UnitNormalMode : UnitModeBase
{
	public SkillObject normalAttack;
	public override void OnModeEnter(StateType state)
	{
		OnSpawn(path, resource);

		unit.unitAnimation = modelAnimation;
		unit.SetUnitCosutmeComponent(modelAnimation.GetComponent<NormalUnitCostume>());
		if (unit.unitCostume != null)
		{
			unit.unitCostume.Init();
			if (unit is PlayerUnit)
			{
				var player = (unit as PlayerUnit);
				player.EquipWeapon();
				player.ChangeNormalUnit(GameManager.UserDB.advancementContainer.Info);

			}
			unit.ChangeCostume();
		}

		unit.ChangeState(state, true);

		//unit.normalAttack = normalAttack;

	}

	public override void OnModeExit()
	{
		modelAnimation.Release();

	}

	public override void OnAttackNormal()
	{
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
