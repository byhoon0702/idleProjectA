using UnityEditor;
using UnityEngine;


//[CustomPropertyDrawer(typeof(IdleNumber))]
public class IdleNumberDrawer : PropertyDrawer
{

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight * 2;
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var amountRect = new Rect(position.x, position.y, 100, position.height);
		var unitRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, 50, position.height);

		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.LabelField(new Rect(amountRect.x, amountRect.y, 20, amountRect.height), "값");
		EditorGUI.PropertyField(new Rect(amountRect.x + 13, amountRect.y, 80, amountRect.height), property.FindPropertyRelative("Value"), GUIContent.none);
		EditorGUI.LabelField(new Rect(unitRect.x, unitRect.y, 20, unitRect.height), "승수");
		EditorGUI.PropertyField(new Rect(unitRect.x + 13, unitRect.y, 80, unitRect.height), property.FindPropertyRelative("Exp"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
