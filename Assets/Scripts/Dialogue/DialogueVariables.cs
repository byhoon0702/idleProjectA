using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialogueVariables
{
	private Dictionary<string, Ink.Runtime.Object> variables;

	public DialogueVariables(TextAsset inkJson)
	{
		Story globalVariableStory = new Story(inkJson.text);

		variables = new Dictionary<string, Ink.Runtime.Object>();
		foreach (string name in globalVariableStory.variablesState)
		{
			Ink.Runtime.Object value = globalVariableStory.variablesState.GetVariableWithName(name);
			variables.Add(name, value);
		}
	}
	public void StartListening(Story story)
	{
		story.variablesState.variableChangedEvent += VariableChanged;
	}
	public void StopListening(Story story)
	{
		story.variablesState.variableChangedEvent -= VariableChanged;
	}

	public void VariableChanged(string name, Ink.Runtime.Object value)
	{

	}
}
