using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct FieldSettings
{
	public string fieldName;
	public float width;

}
[Serializable]
public class BaseDataSheetObject : ScriptableObject
{
	public string tooltip;

	public int itemPerPage = 5;
	public List<FieldSettings> fieldSettings = new List<FieldSettings>();

	public virtual void Call()
	{

	}
}
