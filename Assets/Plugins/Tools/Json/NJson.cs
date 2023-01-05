using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using JsonFx;

public class NJson
{

	public static string Serialize(object in_object)
	{
		try
		{
			return JsonFx.Json.JsonWriter.Serialize(in_object);
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("Error Json Encode : {0}", e.Message));
		}
		return "";
	}

	public static T Deserialize<T>(string in_string)
	{
		try
		{
			return JsonFx.Json.JsonReader.Deserialize<T>(in_string);
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("Error Json Decode : {0}", e.Message + " " + e.InnerException));
		}

		return default(T);
	}

	public static T Decoding<T>(string p_Value)
	{
		try
		{
			return UnityEngine.JsonUtility.FromJson<T>(p_Value);
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("Error Json Decode : {0}", e.Message + " " + e.InnerException));
		}

		return default(T);
	}

	public static object Decode(string p_Value, Type p_Type, bool p_toObj = false)
	{
		try
		{
			return JsonFx.Json.JsonReader.Deserialize(p_Value, p_Type);
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("Error Json Decode : {0}", e.Message));
		}
		return null;
	}

	public static T Decode<T>(string p_Value, bool p_isObj)
	{
		try
		{
			object obj = JsonFx.Json.JsonReader.Deserialize(p_Value);
			Debug.Log("Decode = " + obj.ToString());
			return JsonFx.Json.JsonReader.Deserialize<T>(obj.ToString());
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("Error Json Decode : {0}", e.Message));
		}
		return default(T);
	}

	public static T[] FromJson<T>(string json)
	{
		Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
		return wrapper.items;
	}

	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.items = array;
		return UnityEngine.JsonUtility.ToJson(wrapper);
	}

	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.items = array;
		return UnityEngine.JsonUtility.ToJson(wrapper, prettyPrint);
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] items;
	}
}
