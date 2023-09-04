using UnityEngine;
using System.Threading.Tasks;
public interface IntroFSM
{
	Task<IntroFSM> OnEnter();
	void OnUpdate(float time);
	void OnExit();

	IntroFSM RunNextState(float time);
}

public abstract class RootState : IntroFSM
{
	protected float elapsedTime = 0;

	public abstract void Init();

	public abstract Task<IntroFSM> OnEnter();
	public abstract void OnExit();

	public abstract void OnUpdate(float time);

	public abstract IntroFSM RunNextState(float time);
}




