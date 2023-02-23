using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;


[CustomPropertyDrawer(typeof(SkillEffectData))]
public class SkillEffectDataPropertyDrawer : PropertyDrawer
{
	private BaseAbilitySO skillEffectSO;
	private SerializedObject skillEffectSOSerializeObject;
	private Rect origin;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect contentPosition = new Rect(position);
		origin = new Rect(position);
		label = EditorGUI.BeginProperty(position, label, property);
		contentPosition.height = EditorGUIUtility.singleLineHeight;

		contentPosition = EditorGUI.IndentedRect(position);
		contentPosition.y += EditorGUIUtility.singleLineHeight;
		contentPosition.height = EditorGUIUtility.singleLineHeight;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("tid"), new GUIContent("아이디"));
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("description"), new GUIContent("설명"));
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("name"), new GUIContent("이름"));
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("speed"), new GUIContent("속도"));
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("isDependent"), new GUIContent("애니메이션 종속"));
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		string path = PathHelper.hyperCasualFXPath.Resources().Assets();
		DrawP(contentPosition, "prefab", "시작 이펙트", property.FindPropertyRelative("spawnFxResource"), path);
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		//path = PathHelper.hyperCasualFXPath.Resources().Assets();
		//DrawP(contentPosition, "prefab", "시작 이펙트", property.FindPropertyRelative("hitFxResource"), path);

		//path = PathHelper.hyperCasualFXPath.Resources().Assets();
		//DrawP(contentPosition, "prefab", "시작 이펙트", property.FindPropertyRelative("objectResource"), path);


		path = PathHelper.skillAbilitySOPath.Resources().Assets();
		EditorGUI.BeginChangeCheck();
		DrawP(contentPosition, "BaseAbilitySO", "스킬 타입", property.FindPropertyRelative("abilitySO"), path, false);
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		var skillEffectProperty = property.FindPropertyRelative("abilitySO");
		if (EditorGUI.EndChangeCheck())
		{
			skillEffectSO = null;
		}
		if (skillEffectSO == null && (skillEffectProperty.stringValue.IsNullOrEmpty() == false && skillEffectProperty.stringValue != "Empty"))
		{
			skillEffectSO = (BaseAbilitySO)Resources.Load($"{PathHelper.skillAbilitySOPath}/{skillEffectProperty.stringValue}");

			skillEffectSOSerializeObject = new SerializedObject(skillEffectSO);
		}
		if (skillEffectSO != null)
		{
			System.Type t = skillEffectSO.GetType();
			DrawScriptableProperty(ref contentPosition, t, skillEffectSOSerializeObject);
		}

		EditorGUI.EndProperty();
	}

	private void DrawP(Rect rect, string tag, string label, SerializedProperty property, string path, bool includeEmpty = true)
	{
		List<string> nameList = new List<string>();
		if (includeEmpty)
		{
			nameList.Add("Empty");
		}
		int selectedIndex = 0;
		var guids = UnityEditor.AssetDatabase.FindAssets($"t:{tag}", new string[] { path });
		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(assetpath);

			nameList.Add(filename);
		}
		for (int i = 0; i < nameList.Count; i++)
		{

			if (nameList[i] == property.stringValue)
			{
				selectedIndex = i;
			}
		}


		selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, nameList.ToArray());
		if (nameList.Count == 0)
		{
			return;
		}
		property.stringValue = nameList[selectedIndex];
	}

	private Vector2 scrollPos;
	private void DrawScriptableProperty(ref Rect rect, System.Type t, SerializedObject serializedObj)
	{
		if (serializedObj == null || t == null)
		{
			return;
		}
		serializedObj.Update();


		var fields = t.GetFields();
		float height = 0;
		for (int i = 0; i < fields.Length; i++)
		{
			SerializedProperty subProperty = serializedObj.FindProperty(fields[i].Name);
			height += EditorGUI.GetPropertyHeight(subProperty);
		}
		height += 30;
		Rect contentRect = new Rect(rect);
		contentRect.width = rect.width - 50;
		scrollPos = GUI.BeginScrollView(new Rect(rect.x, rect.y, rect.width, rect.width / 2), scrollPos, new Rect(rect.x, rect.y, rect.width - 70, height), false, false);
		for (int i = 0; i < fields.Length; i++)
		{
			SerializedProperty subProperty = serializedObj.FindProperty(fields[i].Name);
			EditorGUI.GetPropertyHeight(subProperty);
			EditorGUI.PropertyField(contentRect, subProperty);
			contentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (subProperty.type == "Vector2" || subProperty.type == "Vector3" || subProperty.type == "Vector4")
			{
				contentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}

			if (subProperty.isArray && subProperty.isExpanded)
			{

				float posY = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

				if (subProperty.arraySize > 1)
				{
					posY *= subProperty.arraySize;
				}

				contentRect.y += posY + (EditorGUIUtility.singleLineHeight * 2);
			}
		}
		rect.y = contentRect.y;
		GUI.EndScrollView();
		serializedObj.ApplyModifiedProperties();
	}


	//public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	//{
	//	var listProperty = property.FindPropertyRelative("attackData");
	//	System.Type rawDataType = property.type.GetAssemblyType();
	//	FieldInfo[] fields = rawDataType.GetFields();

	//	return ((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (fields.Length - 1)) + EditorGUI.GetPropertyHeight(listProperty);

	//}
}
