using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif




[CreateAssetMenu(fileName = "TrainingItem", menuName = "ScriptableObject/Item/TrainingItem", order = 1)]
public class TrainingItemObject : ItemObject
{
	//[SerializeField] private StatsType type;
	//public StatsType Type => type;
	//[SerializeField] private IdleNumber basicValue;
	//public IdleNumber BasicValue => basicValue;
	//[SerializeField] private IdleNumber perLevel;
	//public IdleNumber PerLevel => perLevel;
	//[SerializeField] StatModeType modeType;
	//public StatModeType ModeType => modeType;

	//[SerializeField] private int maxLevel;
	//public int MaxLevel => maxLevel;

	//[SerializeField] private StatsType preconditionType;
	//public StatsType PreconditionType => preconditionType;

	//[SerializeField] private long preconditionLevel;
	//public long PreconditionLevel => preconditionLevel;

	//[SerializeField] private long basicCost;
	//public long BasicCost => basicCost;
	//[SerializeField] private float basicCostInc;
	//public float BasicCostInc => basicCostInc;
	//[SerializeField] private float basicCostWeight;
	//public float BasicCostWeight => basicCostWeight;

	public bool ispercentage;
	public override string tailChar
	{
		get
		{
			if (ispercentage)
			{
				return "%";
			}
			return base.tailChar;
		}
	}
#if UNITY_EDITOR
	public void FindIconResource()
	{
		//string folder = $"Assets/AssetFolder/Raw/ArtAsset/NewUI/ItemIcon/Equip/{type.ToString().ToLower().FirstCharacterToUpper()}s";
		//var guids = AssetDatabase.FindAssets($"t:sprite", new string[] { folder });
		//for (int i = 0; i < guids.Length; i++)
		//{
		//	string path = AssetDatabase.GUIDToAssetPath(guids[i]);
		//	string filename = System.IO.Path.GetFileNameWithoutExtension(path);

		//	if (filename.Contains(tid.ToString()))
		//	{
		//		icon = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		//		break;
		//	}
		//}
	}

#endif



	public override void SetBasicData<T>(T data)
	{
		var trainingData = data as TrainingData;
		tid = trainingData.tid;
		itemName = trainingData.name;
		description = trainingData.description;
		//type = data.buff.type;
		//basicValue = (IdleNumber)data.buff.value;
		//perLevel = (IdleNumber)data.buff.perLevel;
		//modeType = data.buff.modeType;
		//maxLevel = data.maxLevel;
		//preconditionType = data.preconditionType;
		//preconditionLevel = data.preconditionLevel;
		//basicCost = data.basicCost;
		//basicCostInc = data.basicCostInc;
		//basicCostWeight = data.basicCostWeight;
		ispercentage = trainingData.buff.isPercentage;
	}

}
