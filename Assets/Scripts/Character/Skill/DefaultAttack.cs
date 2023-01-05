public class DefaultAttack : SkillBase
{
	public override float skillUseTime => 0;

	public interface IDefaultAttackEvent
	{
		void OnDefaultAttack_ActionEvent();
	}

	public override string iconPath => string.Empty;

	private IDefaultAttackEvent attackEvent;


	public DefaultAttack(SkillBaseData _skillBaseData) : base(_skillBaseData)
	{


	}

	public override void Action()
	{
		base.Action();

		if (attackEvent == null)
		{
			attackEvent = character as IDefaultAttackEvent;
		}

		attackEvent?.OnDefaultAttack_ActionEvent();
	}
}
