using System.Reflection;
using System;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class EditorHelper
{
	public static void AddNewElement(SerializedProperty property, long tid)
	{
		System.Type type = ConvertUtility.ConvertStringToType(property.type);
		System.Type baseType = type;
		System.Type check = type;
		while (baseType != null)
		{
			check = baseType;
			baseType = check.BaseType;
		}

		bool isObject = check.Equals(typeof(System.Object));

		if (isObject)
		{
			var newData = Activator.CreateInstance(type);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (var field in fields)
			{
				var relativeProperty = property.FindPropertyRelative(field.Name);

				if (relativeProperty == null)
				{
					continue;
				}
				object value = field.GetValue(newData);
				if (field.Name == "tid")
				{
					value = tid;
				}

				try
				{
					EditorHelper.SetSerializedPropertyValue(relativeProperty, value);

				}
				catch (Exception e)
				{
					Debug.LogError($"{relativeProperty.propertyType} , {field.Name}");
				}
			}
		}
	}
	public static void SetValue(this SerializedProperty p, object value)
	{
		switch (p.propertyType)
		{
			case SerializedPropertyType.AnimationCurve:
				p.animationCurveValue = value as AnimationCurve;
				break;
			case SerializedPropertyType.ArraySize:
				p.intValue = (int)value;
				break;
			case SerializedPropertyType.Boolean:
				p.boolValue = (bool)value;
				break;
			case SerializedPropertyType.Bounds:
				p.boundsValue = (Bounds)value;
				break;
			case SerializedPropertyType.Character:
				p.stringValue = (string)value;
				break;
			case SerializedPropertyType.Color:
				p.colorValue = (Color)value;
				break;
			case SerializedPropertyType.Enum:
				p.enumValueIndex = (int)value;
				break;
			case SerializedPropertyType.Float:
				p.floatValue = (float)value;
				break;
			case SerializedPropertyType.Generic:
				Debug.LogWarning("Get/Set of Generic SerializedProperty not supported");
				break;
			case SerializedPropertyType.Gradient:
				Debug.LogWarning("Get/Set of Gradient SerializedProperty not supported");
				break;
			case SerializedPropertyType.Integer:
				p.intValue = (int)value;
				break;
			case SerializedPropertyType.LayerMask:
				p.intValue = (int)value;
				break;
			case SerializedPropertyType.ObjectReference:
				p.objectReferenceValue = value as UnityEngine.Object;
				break;
			case SerializedPropertyType.Quaternion:
				p.quaternionValue = (Quaternion)value;
				break;
			case SerializedPropertyType.Rect:
				p.rectValue = (Rect)value;
				break;
			case SerializedPropertyType.String:
				p.stringValue = (string)value;
				break;
			case SerializedPropertyType.Vector2:
				p.vector2Value = (Vector2)value;
				break;
			case SerializedPropertyType.Vector3:
				p.vector3Value = (Vector3)value;
				break;
			case SerializedPropertyType.Vector4:
				p.vector4Value = (Vector4)value;
				break;
		}
	}



	public static void SetSerializedPropertyValue(SerializedProperty p, object value)
	{
		System.Type type = ConvertUtility.ConvertStringToType(p.type);
		if (type == null)
		{
			p.SetValue(value);
			return;
		}
		if (type.Equals(typeof(int)))
		{
			p.intValue = (int)value;
		}
		else if (type.Equals(typeof(long)))
		{
			p.longValue = (long)value;
		}
		else if (type.Equals(typeof(string)))
		{
			if (value == null)
			{
				p.stringValue = "";
			}
			else
			{
				p.stringValue = (string)value;
			}
		}
		else
		{
			p.SetValue(value);
		}
	}
	public static long ReplacePrefixTid(long originID, long prefix)
	{
		int count = 0;

		long origin = prefix;

		while (origin % 10 == 0)
		{
			origin /= 10;
			count++;
		}

		if (count > 0)
		{
			originID = originID % (int)Mathf.Pow(10, count) + prefix;
		}
		return originID;
	}
	public static FieldInfo[] GetSerializedField(Type type)
	{
		FieldInfo[] temp = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		int count = 0;

		for (int i = 0; i < temp.Length; i++)
		{
			if (temp[i].IsNotSerialized || temp[i].IsInitOnly)
			{
				continue;
			}
			count++;
		}

		FieldInfo[] fieldInfos = new FieldInfo[count];

		System.Type baseType = type;
		while (baseType != null)
		{
			FieldInfo[] declaredField = baseType.GetDeclaredFields();
			if (declaredField == null || declaredField.Length == 0)
			{
				baseType = baseType.BaseType;
				continue;
			}
			int fixindex = 0;
			for (int i = 0; i < declaredField.Length; i++)
			{
				if (declaredField[i].IsNotSerialized || declaredField[i].IsInitOnly)
				{
					fixindex++;
					continue;
				}
			}
			int startIndex = count - (declaredField.Length - fixindex);
			fixindex = 0;
			for (int i = 0; i < declaredField.Length; i++)
			{
				if (declaredField[i].IsNotSerialized || declaredField[i].IsInitOnly)
				{
					fixindex++;
					continue;
				}

				fieldInfos[startIndex + i - fixindex] = declaredField[i];
			}

			count -= (declaredField.Length - fixindex);
			baseType = baseType.BaseType;
		}

		return fieldInfos;
	}
}
#endif
