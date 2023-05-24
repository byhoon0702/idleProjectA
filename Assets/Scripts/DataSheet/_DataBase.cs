using System.Collections.Generic;
using UnityEngine;

public interface IItemData<out T> where T : BaseData, new()
{
	public T Get(long tid);

	public T Get(string tag);


}



[System.Serializable]
public class DataSheetBase<T> : IItemData<T> where T : BaseData, new()
{
	[ReadOnly(false)] public string typeName;
	[ReadOnly(true), SerializeField] public long prefixID;

	public List<T> infos = new List<T>();


	protected virtual void Add()
	{
		T data = new T();
		if (infos.Count == 0)
		{
			data.tid = prefixID + 1;
		}
		else
		{
			var last = infos[infos.Count - 1] as T;
			data.tid = last.tid + 1;
		}

		infos.Add(data);
	}

	public List<T> GetInfosClone()
	{
		List<T> clone = new List<T>(infos);

		return clone;
	}
	public void SetInfos(List<T> _infos)
	{
		infos = new List<T>(_infos);
	}

	public T Get(long _tid)
	{

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}
		return null;
	}

	public virtual T Get(string tag)
	{
		return null;
	}

}
