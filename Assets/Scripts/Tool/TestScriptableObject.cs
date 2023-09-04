using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TestScriptableObject : ScriptableObject
{
	public long tid;
	public ExposedReference<GameObject> obj;
}
