public class FeverState : RootState
{
	public override void OnEnter()
	{
		elapsedTime = 0;
		//FadeIn
		//Clear Object 
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;

	}
}
