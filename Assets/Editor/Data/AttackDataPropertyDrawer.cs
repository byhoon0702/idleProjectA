using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

[CustomPropertyDrawer(typeof(AttackData))]
public class AttackDataPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect contentPosition = position;
		label = EditorGUI.BeginProperty(position, label, property);
		contentPosition.height = EditorGUIUtility.singleLineHeight;
		if (property.isExpanded = EditorGUI.Foldout(contentPosition, property.isExpanded, label, true))
		{

			EditorGUI.indentLevel += 1;
			contentPosition = EditorGUI.IndentedRect(position);
			contentPosition.y += EditorGUIUtility.singleLineHeight;
			contentPosition.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("timing"), new GUIContent("공격 타이밍"));
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			string path = PathHelper.hyperCasualFXPath.Resources().Assets();
			DrawP(contentPosition, "prefab", "피격 이펙트", property.FindPropertyRelative("hitFxResource"), path);
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("damageRate"), new GUIContent("공격 당 데미지 비율"));
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("damageCount"), new GUIContent("데미지 적용 횟수"));
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("damageInterval"), new GUIContent("데미지 적용 간격"));
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			if (GUI.Button(contentPosition, "공격 에디트"))
			{
				ShowChildWindow(contentPosition, property);
			}
			contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;


			EditorGUI.indentLevel -= 1;
		}

		EditorGUI.EndProperty();
	}

	private void DrawP(Rect rect, string tag, string label, SerializedProperty property, string path)
	{
		List<string> nameList = new List<string>();
		int selectedIndex = 0;
		var guids = UnityEditor.AssetDatabase.FindAssets($"t:{tag}", new string[] { path });
		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(assetpath);

			if (filename == property.stringValue)
			{
				selectedIndex = i;
			}
			nameList.Add(filename);
		}

		selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, nameList.ToArray());
		if (nameList.Count == 0)
		{
			return;
		}
		property.stringValue = nameList[selectedIndex];
	}
	private void DrawScriptableObject(Rect rect, string tag, string label, SerializedProperty property, string path)
	{
		List<string> nameList = new List<string>();
		int selectedIndex = 0;
		var guids = UnityEditor.AssetDatabase.FindAssets($"t:{tag}", new string[] { path });
		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			var asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetpath);
			System.Type type = asset.GetType();
			if (type.Name == property.stringValue)
			{
				selectedIndex = i;
			}
			nameList.Add(type.Name);

			Resources.UnloadAsset(asset);
		}

		selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, nameList.ToArray());
		if (nameList.Count == 0)
		{
			return;
		}
		property.stringValue = nameList[selectedIndex];
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (!property.isExpanded)
		{
			return EditorGUIUtility.singleLineHeight;
		}
		else
		{
			System.Type rawDataType = property.type.GetAssemblyType();
			FieldInfo[] fields = rawDataType.GetFields();
			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * fields.Length + EditorGUIUtility.singleLineHeight;
		}

	}
	public void ShowChildWindow(Rect rect, SerializedProperty serializedProperty)
	{
		AttackObjectWindow window = new AttackObjectWindow();
		window.Init(serializedProperty);

		PopupWindow.Show(new Rect(rect.x + rect.width, rect.y - 100, rect.width, rect.height), window);

	}
}


public class AttackObjectWindow : PopupWindowContent
{

	private SerializedProperty property;
	private SerializedProperty objectResourceProperty;
	private SerializedProperty actionBehaviorProperty;
	private SerializedProperty damageBehaviorProperty;

	private SerializedObject skillActionSerializeObject;
	private SerializedObject appliedDamageSerializeObject;
	private Vector2 size = new Vector2(400, 300);
	private Vector2 scrollPos;


	public void Init(SerializedProperty _serializedProperty)
	{
		property = _serializedProperty;
		objectResourceProperty = property.FindPropertyRelative("objectResource");
		actionBehaviorProperty = property.FindPropertyRelative("actionBehavior");
		damageBehaviorProperty = property.FindPropertyRelative("damageBehavior");
	}

	public override Vector2 GetWindowSize()
	{
		return new Vector2(size.x == 0 ? 400 : size.x, size.y == 0 ? 300 : size.y);
	}



	[SerializeField]
	private SkillActionBehavior skillActionBehavior;
	[SerializeField]
	private AppliedDamageBehavior appliedDamageBehavior;

	float height;
	public override void OnGUI(Rect rect)
	{
		Rect contentPosition = rect;
		contentPosition.y += EditorGUIUtility.singleLineHeight;
		contentPosition.height = EditorGUIUtility.singleLineHeight;
		property.serializedObject.Update();

		scrollPos = GUI.BeginScrollView(new Rect(rect.x, rect.y, size.x, size.y), scrollPos, new Rect(rect.x, rect.y, size.x, height));
		string path = PathHelper.projectilePath.Resources().AssetFolder().Assets();
		DrawP(contentPosition, "prefab", "공격 오브젝트", objectResourceProperty, path);
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		EditorGUI.BeginChangeCheck();
		path = PathHelper.projectileBehaviorPath.Resources().Assets();
		DrawScriptableObject(contentPosition, "SkillActionBehavior", "동작방식", actionBehaviorProperty, path);
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		if (EditorGUI.EndChangeCheck())
		{
			skillActionBehavior = null;
		}
		if (skillActionBehavior == null && (actionBehaviorProperty.stringValue.IsNullOrEmpty() == false && actionBehaviorProperty.stringValue != "Empty"))
		{
			skillActionBehavior = (SkillActionBehavior)Resources.Load($"{PathHelper.projectileBehaviorPath}/{actionBehaviorProperty.stringValue}");

			skillActionSerializeObject = new SerializedObject(skillActionBehavior);
		}
		if (skillActionBehavior != null)
		{
			System.Type t = skillActionBehavior.GetType();
			DrawScriptableProperty(ref contentPosition, t, skillActionSerializeObject);
		}
		EditorGUI.BeginChangeCheck();

		path = PathHelper.applyDamageBehaviorPath.Resources().Assets();
		DrawScriptableObject(contentPosition, "AppliedDamageBehavior", "데미지 적용 방식", damageBehaviorProperty, path);
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		if (EditorGUI.EndChangeCheck())
		{
			appliedDamageBehavior = null;
		}
		if (appliedDamageBehavior == null && (damageBehaviorProperty.stringValue.IsNullOrEmpty() == false && damageBehaviorProperty.stringValue != "Empty"))
		{
			appliedDamageBehavior = (AppliedDamageBehavior)Resources.Load($"{PathHelper.applyDamageBehaviorPath}/{damageBehaviorProperty.stringValue}");

			appliedDamageSerializeObject = new SerializedObject(appliedDamageBehavior);
		}
		if (appliedDamageBehavior != null)
		{
			System.Type t = appliedDamageBehavior.GetType();
			DrawScriptableProperty(ref contentPosition, t, appliedDamageSerializeObject);

		}
		contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		height = contentPosition.y + contentPosition.height;
		GUI.EndScrollView();
		property.serializedObject.ApplyModifiedProperties();
	}


	private void DrawScriptableProperty(ref Rect rect, System.Type t, SerializedObject serializedObj)
	{
		if (serializedObj == null || t == null)
		{
			return;
		}
		serializedObj.Update();

		var fields = t.GetFields();
		for (int i = 0; i < fields.Length; i++)
		{

			SerializedProperty subProperty = serializedObj.FindProperty(fields[i].Name);
			EditorGUI.PropertyField(rect, subProperty);
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			if (subProperty.type == "Vector2" || subProperty.type == "Vector3" || subProperty.type == "Vector4")
			{
				rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}

			if (subProperty.isArray && subProperty.isExpanded)
			{

				float posY = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

				if (subProperty.arraySize > 1)
				{
					posY *= subProperty.arraySize;
				}

				rect.y += posY + (EditorGUIUtility.singleLineHeight * 2);
			}
		}

		serializedObj.ApplyModifiedProperties();
	}

	private void DrawP(Rect rect, string tag, string label, SerializedProperty property, string path)
	{
		List<string> nameList = new List<string>();
		int selectedIndex = 0;
		var guids = UnityEditor.AssetDatabase.FindAssets($"t:{tag}", new string[] { path });
		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(assetpath);

			if (filename == property.stringValue)
			{
				selectedIndex = i;
			}
			nameList.Add(filename);
		}

		selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, nameList.ToArray());
		if (nameList.Count == 0)
		{
			return;
		}
		property.stringValue = nameList[selectedIndex];
	}
	private void DrawScriptableObject(Rect rect, string tag, string label, SerializedProperty property, string path)
	{
		List<string> nameList = new List<string>();
		int selectedIndex = 0;
		var guids = UnityEditor.AssetDatabase.FindAssets($"t:{tag}", new string[] { path });
		for (int i = 0; i < guids.Length; i++)
		{
			string assetpath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			string filename = System.IO.Path.GetFileNameWithoutExtension(assetpath);
			if (filename == property.stringValue)
			{
				selectedIndex = i;
			}
			nameList.Add(filename);

			//Resources.UnloadAsset(asset);
		}

		selectedIndex = EditorGUI.Popup(rect, label, selectedIndex, nameList.ToArray());
		if (nameList.Count == 0)
		{
			return;
		}
		property.stringValue = nameList[selectedIndex];
	}
}
