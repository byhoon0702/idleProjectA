using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
	// Start is called before the first frame update
	public static bool HaveParam(this string in_str)
	{
		return (false == in_str.IsNullOrEmpty() && false == in_str.Equals("0"));
	}
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

}
