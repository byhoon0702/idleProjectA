using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum WeaponType
{
	SWORD,
	AXE,
	SPEAR,
	HAMMER,
	FIST,

}



[CreateAssetMenu(fileName = "EquipItem", menuName = "ScriptableObject/Item/EquipItem", order = 1)]
public class EquipItemObject : ItemObject
{
	[SerializeField] private EquipType type;
	[SerializeField] private GameObject effectObject;
	public GameObject EffectObject => effectObject;

	public GameObject equipObject;
	public AnimatorOverrideController animatorOverrideController;

#if UNITY_EDITOR
	public void FindIconResource()
	{
		string folder = $"Assets/AssetFolder/Raw/ArtAsset/NewUI/ItemIcon/Equip/{type.ToString().ToLower().FirstCharacterToUpper()}s";
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
	public void FindEquipObejct()
	{
		if (type != EquipType.WEAPON)
		{
			return;
		}
		string folder = $"Assets/AssetFolder/Resources/{type.ToString().ToLower().FirstCharacterToUpper()}s";
		var guids = AssetDatabase.FindAssets($"t:GameObject", new string[] { folder });
		for (int i = 0; i < guids.Length; i++)
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(path);

			if (filename.Contains(tid.ToString()))
			{
				equipObject = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
				break;
			}
		}
	}
#endif

	public override void SetBasicData<T>(T data)
	{
		var equipData = data as EquipItemData;
		this.tid = equipData.tid;
		itemName = equipData.name;
		this.type = equipData.equipType;
		this.description = equipData.description;


	}
}
