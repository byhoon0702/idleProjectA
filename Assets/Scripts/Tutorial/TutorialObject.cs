using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/ 필수* TutorialObject")]
public class TutorialObject : ScriptableObject
{
	public QuestGoalType questGoalType;
	public TutorialStep[] tutorialSteps;
	public ITutorial CurrentStep;

	bool begin;
	public void BeginTutorial(RuntimeData.QuestInfo questInfo)
	{

		if (questInfo.GoalType != questGoalType)
		{
			return;
		}

		for (int i = 0; i < tutorialSteps.Length; i++)
		{
			if (tutorialSteps[i] == null)
			{
				Debug.LogWarning($"Tutorial Index {i} is Null");
				continue;
			}
			if (i + 1 < tutorialSteps.Length)
			{
				tutorialSteps[i].next = tutorialSteps[i + 1];
			}
			else
			{
				tutorialSteps[i].next = null;
			}

			if (i - 1 >= 0)
			{
				tutorialSteps[i].prev = tutorialSteps[i - 1];
			}
			else
			{
				tutorialSteps[i].prev = null;
			}
		}

		CurrentStep = tutorialSteps[0];

		CurrentStep.Enter(questInfo);

		//begin = true;
	}
	public void OnUpdate(float time)
	{
		if (CurrentStep != null)
		{
			CurrentStep = CurrentStep.OnUpdate(time);
		}
	}
}
