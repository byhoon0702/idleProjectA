
public static class ConvertUtility
{
	public static System.Type ConvertStringToType(string name)
	{
		string converted = ConvertStringToSystemTypeName(name);
		System.Type type = System.Type.GetType(converted);
		if (type == null)
		{
			type = converted.GetAssemblyType();
		}
		return type;
	}
	public static string ConvertStringToSystemTypeName(string typeName)
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
			case "Enum":
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

			case "List`1":
				convertedString = "list";
				break;
			default:
				{

					System.Type type = System.Type.GetType(GetAssemblyName(typeName));
					if (type.BaseType.Equals(typeof(System.Enum)))
					{
						convertedString = "enum";
					}
					else
					{
						convertedString = "object";
					}

				}
				break;
		}

		return convertedString;
	}
	public static string GetAssemblyName(this string type)
	{
		if (type.Contains("_"))
		{
			type = type.Split("_")[0];
		}
		return $"{type}, Assembly-CSharp";
	}
	public static System.Type GetAssemblyType(this string type)
	{
		if (type.Contains("_"))
		{
			type = type.Split("_")[0];
		}
		return System.Type.GetType(type.GetAssemblyName());
	}

}
