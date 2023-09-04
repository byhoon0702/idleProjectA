using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
public enum CostumeType
{
	CHARACTER = 1,
	HYPER = 10,
	_END = 100,
}

[CreateAssetMenu(fileName = "CostumeItem", menuName = "ScriptableObject/Item/CostumeItem", order = 1)]
public class CostumeItemObject : ItemObject
{

	[SerializeField] private CostumeType type;
	[SerializeField] private GameObject effectObject;
	[SerializeField] private GameObject hitEffectObject;
	public GameObject HitEffect => hitEffectObject;
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
	public override void SetBasicData<T>(T data)
	{
		var costumeData = data as CostumeData;
		this.tid = data.tid;
		itemName = data.name;
		this.type = costumeData.costumeType;
		this.description = data.description;

	}

}
