using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gacha Object", menuName = "ScriptableObject/Gacha/Gacha Object", order = 1)]
public class GachaObject : ItemObject
{
	[SerializeField] private GachaType type;
	[Tooltip("가챠 성급 별 고정 확률")]
	[SerializeField] private List<int> gachaStarChance;

	public override void SetBasicData<T>(T data)
	{
		var gachaData = data as GachaData;
		tid = gachaData.tid;
		type = gachaData.gachaType;
	}
}
