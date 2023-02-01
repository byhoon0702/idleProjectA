using UnityEngine;
public class IdleState : CharacterFSM
{
	private Unit owner;
	public void Init(Unit owner)
	{
		this.owner = owner;
	}
	public void OnEnter()
	{
		owner.PlayAnimation(StateType.IDLE);
	}

	public void OnExit()
	{

	}

	public void OnUpdate(float time)
	{

	}

	private void FindTarget()
	{

	}
}


