using System;
using Ink.Runtime;

public class InkExternalFunction
{
	public void Bind(Story story, string function, Action action)
	{
		story.BindExternalFunction(function, action);
	}
	public void Bind<T1>(Story story, string function, Action<T1> action)
	{
		story.BindExternalFunction(function, action);
	}
	public void Bind<T1, T2>(Story story, string function, Action<T1, T2> action)
	{
		story.BindExternalFunction(function, action);
	}

	public void Unbind(Story story, string function)
	{
		story.UnbindExternalFunction(function);
	}

}
