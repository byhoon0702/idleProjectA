using System.Collections.Generic;
[System.Serializable]
public abstract class DataBase<T> where T : new()
{
	public string typeName;
	public List<T> infos = new List<T>();
	public void Add()
	{
		infos.Add(new T());
	}
}
