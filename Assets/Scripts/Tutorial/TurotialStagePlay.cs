using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

public class TurotialStagePlay : TutorialStep
{
	public override ITutorial Enter(QuestInfo quest)
	{
		_quest = quest;
		return this;
	}

	public override ITutorial OnUpdate(float time)
	{
		return next == null ? this : next.Enter(_quest);
	}
}
