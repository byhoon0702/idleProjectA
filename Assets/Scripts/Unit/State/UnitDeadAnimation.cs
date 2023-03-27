
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Dead Animation", menuName = "Unit State/Dead Action/Animation", order = 1)]

public class UnitDeadAnimation : UnitDeadStateAction
{
	public override void OnEnter(UnitBase unitBase)
	{
		unitBase.unitAnimation.PlayAnimation(StateType.DEATH);
	}

	public override void OnUpdate(UnitBase unitBase, float delta, float elapsedTime)
	{

	}
}
