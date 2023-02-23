
public class DefaultAttackData : SkillBaseData
{
	public float cooltime = 1;
}

public class DefaultAttack : SkillBase
{

	public override float skillUseTime => 0f;

	public interface IDefaultAttackEvent
	{
		void OnDefaultAttack_ActionEvent();
	}

	public override float cooltime => attackData.cooltime;

	public override float cooltimeTimeScale => base.cooltimeTimeScale * owner.AttackSpeedMul;
	private DefaultAttackData attackData;
	private IDefaultAttackEvent attackEvent;


	public DefaultAttack(DefaultAttackData _attackData) : base(_attackData)
	{
		attackData = _attackData;
	}

	public override void Action()
	{
		//base.Action();

		if (attackEvent == null)
		{
			attackEvent = owner as IDefaultAttackEvent;
		}

		attackEvent?.OnDefaultAttack_ActionEvent();
	}
	public override void CalculateSkillLevelData(int _skillLevel)
	{
	}
}
