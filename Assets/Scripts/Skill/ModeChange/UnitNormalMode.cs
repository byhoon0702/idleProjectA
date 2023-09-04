
//[CreateAssetMenu(fileName = "Unit Normal Mode", menuName = "ScriptableObject/Unit Normal Mode", order = 1)]
public class UnitNormalMode : UnitModeBase
{
	public override void OnModeEnter(StateType state)
	{
		OnSpawn();

		unit.unitAnimation = modelAnimation;

		unit.ChangeState(state, true);
		unit.unitAnimation.PlayDissolve(0);
		//unit.SetUnitCosutmeComponent(modelAnimation.GetComponent<NormalUnitCostume>());
		//if (unit.unitCostume != null)
		//{
		//	unit.unitCostume.Init();
		//	if (unit is PlayerUnit)
		//	{
		//		var player = (unit as PlayerUnit);
		//		player.EquipWeapon();
		//		player.ChangeNormalUnit(PlatformManager.UserDB.advancementContainer.Info);
		//		player.hitEffectObject = null;
		//	}
		//	unit.ChangeCostume();
		//}



		//unit.normalAttack = normalAttack;

		//unit.SetTarget
		//skillModule.Init(this, 1701500001);

	}

	public override void OnModeExit()
	{
		//modelAnimation.Release();
		Destroy(modelAnimation.gameObject);
		modelAnimation = null;
	}

	public override void OnAttackNormal()
	{
		unit.PlayAnimation(StateType.ATTACK);
	}



	public override void OnHit(HitInfo hit)
	{
		//unit.Hit(hit);
		unit.unitAnimation.PlayAnimation(StateType.HIT);
		unit.unitAnimation.PlayDamageWhite();
	}


}
