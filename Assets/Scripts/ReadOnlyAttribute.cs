using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute
{
	public readonly bool runtimeOnly;
	public ReadOnlyAttribute(bool runtimeOnly)
	{
		this.runtimeOnly = runtimeOnly;
	}
}
