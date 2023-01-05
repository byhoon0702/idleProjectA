public class DefaultAttack : SkillBase
{
	public override float skillUseTime => 0;

	public interface IDefaultAttackEvent
	{
		void OnDefaultAttack_ActionEvent();
	}

	public override string iconPath => string.Empty;
	public override float cooltimeTimeScale => 1 + owner.conditionModule.ability.attackSpeedRatio;

	private IDefaultAttackEvent attackEvent;


	public DefaultAttack(SkillBaseData _skillBaseData) : base(_skillBaseData)
	{


	}

	public override void Action()
	{
		base.Action();

		if (attackEvent == null)
		{
			attackEvent = owner as IDefaultAttackEvent;
		}

		attackEvent?.OnDefaultAttack_ActionEvent();
	}
}
