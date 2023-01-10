
public class DefaultAttackData : SkillBaseData
{
}

public class DefaultAttack : SkillBase
{
	public override float skillUseTime => 0;

	public interface IDefaultAttackEvent
	{
		void OnDefaultAttack_ActionEvent();
	}

	public override string iconPath => string.Empty;
	public override float cooltimeTimeScale => base.cooltimeTimeScale * owner.info.AttackSpeedMul();

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
