using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
public class BaseEditorPanel : UIBehaviour
{
	public EditorUnit viewTarget
	{
		get;
		protected set;
	}
	public UnityEditor.Animations.AnimatorController animatorController
	{
		get;
		protected set;
	}

	public EditorToolUI editorToolUI
	{
		protected set;
		get;
	}
	public virtual void Init(EditorToolUI _editorToolUI)
	{
		editorToolUI = _editorToolUI;

	}


	public virtual void SetUnit(EditorUnit _unit, UnityEditor.Animations.AnimatorController _animatorController)
	{

	}
}
#endif
