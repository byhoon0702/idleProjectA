public class RootState : FiniteStateMachine
{
	protected float elapsedTime = 0;

	public virtual void Init()
	{

	}
	public virtual void OnEnter()
	{
		elapsedTime = 0;
		//FadeIn
		//Clear Object 
	}

	public virtual void OnExit()
	{

	}

	public virtual void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.BGLOADING);

		}
	}
}




