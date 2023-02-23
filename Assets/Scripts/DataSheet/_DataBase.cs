using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public abstract class DataSheetBase<T> where T : BaseData, new()
{
	[ReadOnly(false)] public string typeName;
	[ReadOnly(true), SerializeField] protected long prefixID;

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
}
