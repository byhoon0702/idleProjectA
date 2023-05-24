using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public enum CostumeType
{
	HEAD = 0,
	BODY = 1,
	WEAPON = 2,
	_EQUIP_END = 3,
	WHOLEBODY = 10,
	WHOLEFACE = 12,

	HYPER_BODY = 30,
	HYPER_HEAD = 31,
	HYPER_WEAPON = 32,
	_END = 100,

}

[CreateAssetMenu(fileName = "CostumeItem", menuName = "ScriptableObject/Item/CostumeItem", order = 1)]
public class CostumeItemObject : ItemObject
{

	[SerializeField] private CostumeType type;
	[SerializeField] private GameObject effectObject;

	public GameObject EffectObject => effectObject;
	public CostumeType Type => type;

	[SerializeField] private GameObject costumeObject;
	public GameObject CostumeObject => costumeObject;


#if UNITY_EDITOR
	public void FindIconResource()
	{
		string folder = $"Assets/AssetFolder/Raw/ArtAsset/NewUI/ItemIcon/Costume/{type.ToString().ToLower().FirstCharacterToUpper()}s";
		var guids = AssetDatabase.FindAssets($"t:sprite", new string[] { folder });
		for (int i = 0; i < guids.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(path);

			if (filename.Contains(tid.ToString()))
			{
				icon = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
				break;
			}
		}
	}

	public void FindObject()
	{
		string folder = $"Assets/AssetFolder/Resources/B/Player";
		var guids = AssetDatabase.FindAssets($"t:GameObject", new string[] { folder });
		for (int i = 0; i < guids.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(path);

			if (filename.Contains(tid.ToString()))
			{
				costumeObject = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
				break;
			}
		}
	}
#endif
	public void SetBasicData(long tid, string name, string description, CostumeType type, Grade grade, int starLv)
	{
		this.tid = tid;
		itemName = name;
		this.type = type;
		this.description = description;

	}

}
