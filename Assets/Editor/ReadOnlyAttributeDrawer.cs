
using UnityEngine;


#if UNITY_EDITOR
namespace UnityEditor
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyAttributeDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
			EditorGUI.PropertyField(position, property);
			GUI.enabled = true;
		}
	}
}
#endif
