
using UnityEngine;
[CreateAssetMenu(fileName = "Unit Dead State", menuName = "Unit State/Dead", order = 1)]
public class UnitDeadState : UnitFSM
{

	public UnitDeadStateAction[] deadAction;
	private float elapsedTime;

	public void Init(Unit _owner)
	{
		owner = _owner;
	}
	public override void OnEnter()
	{
		elapsedTime = 0f;

		owner.PlayAnimation(StateType.DEATH);
		owner.SetTarget(null); // 죽으면 타겟을 비워줌
		owner.conditionModule.Collect();

		if (owner.ControlSide == ControlSide.ENEMY)
		{
			VGameManager.it.battleRecord.killCount++;
			UIController.it.UiStageInfo.RefreshKillCount();
		}

		if (deadAction != null)
		{
			for (int i = 0; i < deadAction.Length; i++)
			{
				//deadAction[i].OnEnter(owner);
			}
		}
	}
	public override void OnEnter<T>(T data)
	{
		OnEnter();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;

		if (deadAction != null)
		{
			for (int i = 0; i < deadAction.Length; i++)
			{
				//deadAction[i].OnUpdate(owner, time, elapsedTime);
			}
		}

		if (elapsedTime > 3f)
		{
			owner.DisposeModel();
			owner.gameObject.SetActive(false);
			owner.ReleaseSkillEffectObject();
		}
	}

	protected void Thrown()
	{

	}
}
