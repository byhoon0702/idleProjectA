using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSheet", menuName = "StageDataSheet", order = 3)]
public class StageDataSheet : ScriptableObject
{
	public List<StageInfo> stageInfos = new List<StageInfo>();
}
