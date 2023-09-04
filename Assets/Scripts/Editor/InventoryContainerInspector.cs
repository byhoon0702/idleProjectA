using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using System.IO;
using Unity.VisualScripting;

[CustomEditor(typeof(InventoryContainer))]
public class InventoryContainerInspector : Editor
{
	InventoryContainer inventory;
	//SerializedProperty adsRewardChest;
	private void OnEnable()
	{
		inventory = target as InventoryContainer;

		//adsRewardChest = serializedObject.FindProperty("adsRewardChestList");
	}
	public override void OnInspectorGUI()
	{
		//bas
		//e.OnInspectorGUI();
		serializedObject.Update();
		GUI.enabled = false;
		EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(inventory), typeof(InventoryContainer), false);
		GUI.enabled = true;

		EditorGUILayout.Space(5);

		EditorGUILayout.LabelField("Currency");
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("Gold"));
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("Dia"));
		if (GUILayout.Button("Load Currency"))
		{
			UpdateCurrencyList(inventory.currencyList, "Assets/Resources/RuntimeDatas/CurrencyItems");
		}
		if (GUILayout.Button("Save"))
		{
			inventory.Save();
			//UpdateCurrencyList(inventory.currencyList, "Assets/Resources/RuntimeDatas/CurrencyItems");
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("currencyList"));

		//EditorGUILayout.PropertyField(adsRewardChest);
		//EditorGUILayout.Space(5);

		//EditorGUILayout.LabelField("Equipment");

		//if (GUILayout.Button("Load Equip"))
		//{
		//	UpdateEquipList(inventory.weaponItemList, "Assets/Resources/RuntimeDatas/EquipItems/Weapons");
		//	UpdateEquipList(inventory.armorItemList, "Assets/Resources/RuntimeDatas/EquipItems/Armors");
		//	UpdateEquipList(inventory.ringItemList, "Assets/Resources/RuntimeDatas/EquipItems/Rings");
		//	UpdateEquipList(inventory.necklaceItemList, "Assets/Resources/RuntimeDatas/EquipItems/Necklaces");
		//}
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponItemList"));

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("armorItemList"));

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("ringItemList"));

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("necklaceItemList"));

		//EditorGUILayout.Space(5);
		//if (GUILayout.Button("Load Costume"))
		//{
		//	UpdateEquipList(inventory.costumeWeaponList, "Assets/Resources/RuntimeDatas/Costumes/Weapons");
		//	UpdateEquipList(inventory.costumeBodyList, "Assets/Resources/RuntimeDatas/Costumes/Bodys");
		//	UpdateEquipList(inventory.costumeHeadList, "Assets/Resources/RuntimeDatas/Costumes/Heads");
		//}
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeWeaponList"));

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeBodyList"));

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("costumeHeadList"));


		//EditorGUILayout.Space(5);
		//if (GUILayout.Button("Load Skill"))
		//{
		//	UpdateSkillList(inventory.skillList, "Assets/Resources/RuntimeDatas/Skills");
		//}
		//EditorGUILayout.LabelField("Skill");

		//EditorGUILayout.PropertyField(serializedObject.FindProperty("skillList"));

		//EditorGUILayout.Space(5);
		//if (GUILayout.Button("Load Pet"))
		//{
		//	UpdatePetList(inventory.petList, "Assets/Resources/RuntimeDatas/Pets");
		//}
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("petList"));


		serializedObject.ApplyModifiedProperties();
	}


	private void UpdatePetList(List<RuntimeData.PetInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.PetInfo info = new RuntimeData.PetInfo();

			//PetItemObject petitemObject = (PetItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(PetItemObject));
			//info.level = 1;
			//info.count = 0;
			//info.tid = petitemObject.Tid;


			//infoList.Add(info);
		}

		//	infoList.Sort((x, y) => { return x.Tid.CompareTo(y.Tid); });
		EditorUtility.SetDirty(target);
	}
	private void UpdateCurrencyList(List<RuntimeData.CurrencyInfo> infoList, string path)
	{
		infoList.Clear();
		var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

			RuntimeData.CurrencyInfo info = new RuntimeData.CurrencyInfo();

			//CurrencyItemObject currencyitemObject = (CurrencyItemObject)AssetDatabase.LoadAssetAtPath(assetpath, typeof(CurrencyItemObject));
			//info.level = 0;
			//info.count = 0;
			//info.tid = currencyitemObject.Tid;
			//infoList.Add(info);
		}

		infoList.Sort((x, y) => { return x.Tid.CompareTo(y.Tid); });
		EditorUtility.SetDirty(target);
	}
	//private void UpdateSkillList(List<RuntimeData.SkillInfo> infoList, string path)
	//{
	//	infoList.Clear();
	//	var guids = AssetDatabase.FindAssets("t:scriptableobject", new string[] { path });

	//	for (int i = 0; i < guids.Length; i++)
	//	{
	//		string assetpath = AssetDatabase.GUIDToAssetPath(guids[i]);

	//		RuntimeData.SkillInfo info = new RuntimeData.SkillInfo();

	//		NewSkill skillitemObject = (NewSkill)AssetDatabase.LoadAssetAtPath(assetpath, typeof(NewSkill));
	//		info.level = 1;
	//		info.count = 0;
	//		info.tid = skillitemObject.Tid;
	//		info.itemObject = skillitemObject;

	//		infoList.Add(info);
	//	}

	//	infoList.Sort((x, y) => { return x.tid.CompareTo(y.tid); });
	//	EditorUtility.SetDirty(target);
	//}
}
