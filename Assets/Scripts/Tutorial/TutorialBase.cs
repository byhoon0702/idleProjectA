using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITutorial
{
	ITutorial Enter(RuntimeData.QuestInfo info);
	ITutorial Back();
	ITutorial OnUpdate(float time);
}


public abstract class TutorialStep : ScriptableObject, ITutorial
{
	public ITutorial next;
	public ITutorial prev;
	protected RuntimeData.QuestInfo _quest;
	public abstract ITutorial Enter(RuntimeData.QuestInfo quest);
	public virtual ITutorial Back()
	{
		return prev == null ? this : prev.Enter(_quest);
	}

	public abstract ITutorial OnUpdate(float time);

	public T FindObject<T>() where T : Component
	{
		return GameObject.FindObjectOfType<T>(true);
	}

}
