using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using System.IO;
using Unity.VisualScripting;

[CustomEditor(typeof(InventoryObject))]
public class InventoryObjectInspector : Editor
{
	InventoryObject inventory;
	private void OnEnable()
	{
		inventory = target as InventoryObject;
	}
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		serializedObject.Update();

		GUI.enabled = false;
		EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(inventory), typeof(InventoryObject), false);
		GUI.enabled = true;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("itemList"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Gold"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Dia"));

		EditorGUILayout.Space(5);

		EditorGUILayout.LabelField("Equipment");

		if (GUILayout.Button("Load Equip"))
		{
			UpdateEquipList(inventory.weaponItemList, "Assets/Resources/RuntimeDatas/EquipItems/Weapons");
			UpdateEquipList(inventory.armorItemList, "Assets/Resources/RuntimeDatas/EquipItems/Armors");
			UpdateEquipList(inventory.ringItemList, "Assets/Resources/RuntimeDatas/EquipItems/Rings");
			UpdateEquipList(inventory.necklaceItemList, "Assets/Resources/RuntimeDatas/EquipItems/Necklaces");
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponItemList"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("armorItemList"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("ringItemList"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("necklaceItemList"));

		EditorGUILayout.Space(5);
		if (GUILayout.Button("Load Costume"))
		{
			UpdateEquipList(inventory.costumeWeaponList, "Assets/Resources/RuntimeDatas/Costumes/Weapons");
			UpdateEquipList(inventory.costumeBodyList, "Assets/Resources/RuntimeDatas/Costumes/Bodys");
			UpdateEquipList(inventory.costumeHeadList, "Assets/Resources/RuntimeDatas/Costumes/Heads");
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeWeaponList"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeBodyList"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeHeadList"));


		EditorGUILayout.Space(5);
		EditorGUILayout.LabelField("Skill");

		EditorGUILayout.PropertyField(serializedObject.FindProperty("skillList"));

		EditorGUILayout.Space(5);
		if (GUILayout.Button("Load Pet"))
		{
			UpdatePetList(inventory.petList, "Assets/Resources/RuntimeDatas/Pets");
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("petList"));


		serializedObject.ApplyModifiedProperties();
	}

	private void UpdateEquipList(List<RuntimeData.CostumeInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.CostumeInfo info = new RuntimeData.CostumeInfo();

			CostumeItemObject costumeitemObject = (CostumeItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(CostumeItemObject));
			info.level = 1;
			info.count = 0;
			info.tid = costumeitemObject.Tid;
			info.itemObject = costumeitemObject;

			infoList.Add(info);
		}

		infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
		EditorUtility.SetDirty(target);
	}
	private void UpdateEquipList(List<RuntimeData.EquipItemInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.EquipItemInfo info = new RuntimeData.EquipItemInfo();

			EquipItemObject equipitemObject = (EquipItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(EquipItemObject));
			info.level = 1;
			info.count = 0;
			info.tid = equipitemObject.Tid;
			info.itemObject = equipitemObject;

			infoList.Add(info);
		}

		infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
		EditorUtility.SetDirty(target);
	}
	private void UpdatePetList(List<RuntimeData.PetInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.PetInfo info = new RuntimeData.PetInfo();

			PetItemObject petitemObject = (PetItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(PetItemObject));
			info.level = 1;
			info.count = 0;
			info.tid = petitemObject.Tid;
			info.itemObject = petitemObject;

			infoList.Add(info);
		}

		infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
		EditorUtility.SetDirty(target);
	}
}
