using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public static class ConvertUtility
{
	// Start is called before the first frame update
	public static string ConvertStringToType(string typeName)
	{

		string convertedString = typeName;
		switch (typeName)
		{
			case "short":
				convertedString = "System.Int16";
				break;
			case "int":
				convertedString = "System.Int32";
				break;
			case "long":
				convertedString = "System.Int64";
				break;
			case "float":
				convertedString = "System.Single";
				break;
			case "double":
				convertedString = "System.Double";
				break;
			case "string":
				convertedString = "System.String";
				break;
			case "enum":
				convertedString = "System.Enum";
				break;
		}

		return convertedString;
	}
	public static string ConvertTypeToString(string typeName)
	{

		string convertedString = typeName;
		switch (typeName)
		{
			case "Int16":
				convertedString = "short";
				break;
			case "Int32":
				convertedString = "int";
				break;
			case "Int64":
				convertedString = "long";
				break;
			case "Single":
				convertedString = "float";
				break;
			case "Double":
				convertedString = "double";
				break;
			case "String":
				convertedString = "string";
				break;
			case "Enum":
				convertedString = "enum";
				break;
			case "List`1":
				convertedString = "list";
				break;
			case "Object":
				convertedString = "object";
				break;
		}

		return convertedString;
	}
}
