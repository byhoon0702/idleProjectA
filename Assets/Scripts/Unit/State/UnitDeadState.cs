
using UnityEngine;
[CreateAssetMenu(fileName = "Unit Dead State", menuName = "Unit State/Dead", order = 1)]
public class UnitDeadState : UnitFSM
{
	public UnitDeadStateAction[] deadAction;
	private float elapsedTime;


	public override FSM OnEnter()
	{
		elapsedTime = 0f;

		owner.PlayAnimation(StateType.DEATH);
		owner.SetTarget(null); // 죽으면 타겟을 비워줌

		return this;

	}


	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;

		//if (elapsedTime > 1f)
		{

			owner.Death();
		}
	}

	protected void Thrown()
	{

	}
	public override void OnFixedUpdate(float fixedTime)
	{

	}
}
