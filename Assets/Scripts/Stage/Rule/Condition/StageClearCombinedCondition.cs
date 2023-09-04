using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Clear Combined Condition", menuName = "ScriptableObject/Stage/Condition/Stage Clear Combined", order = 1)]
public class StageClearCombinedCondition : StageClearCondition
{
	public StageClearCondition[] conditions;

	public bool isTogether;
	public override bool CheckCondition()
	{

		if (isTogether)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				if (conditions[i].CheckCondition() == false)
				{
					return false;
				}

			}
			return true;

		}
		else
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				if (conditions[i].CheckCondition())
				{
					return true;
				}

			}
			return false;
		}

	}

	public override void SetCondition()
	{
		for (int i = 0; i < conditions.Length; i++)
		{
			conditions[i].SetCondition();
		}
	}
	public override void OnUpdate(float time)
	{
		for (int i = 0; i < conditions.Length; i++)
		{
			conditions[i].OnUpdate(time);
		}
	}
}
