using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorUnit Filter Behavior", menuName = "ScriptableObject/Filter/EditorUnit")]
public class TargetEditorUnitBehaviorSO : TargetFilterBehaviorSO
{

	public override UnitBase[] GetObject()
	{
		return FilterObject<EditorUnit>();
	}

}
