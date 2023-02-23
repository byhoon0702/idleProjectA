using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData : BaseData
{
	public string name;
	public string resource;
	public string hitEffect;

	public ProjectileData()
	{

	}
	public ProjectileData(long _tid, string _description, string _name, string _resource, string _hitEffect)
	{
		tid = _tid;
		description = _description;
		name = _name;
		resource = _resource;
		hitEffect = _hitEffect;
	}

	public ProjectileData Clone()
	{
		ProjectileData clone = new ProjectileData();
		clone.tid = tid;
		clone.description = description;
		clone.name = name;
		clone.resource = resource;
		clone.hitEffect = hitEffect;
		return clone;
	}

}

[System.Serializable]
public class ProjectileDataSheet : DataSheetBase<ProjectileData>
{
	public ProjectileData GetData(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}
		return null;
	}

#if UNITY_EDITOR
	/// <summary>
	/// 에디터용이 아니면 절대로 사용하면 안됨
	/// </summary>
	public void SetData(long tid, ProjectileData data)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				infos[i] = data;
				break;
			}
		}
	}

	public void AddData(ProjectileData data)
	{
		infos.Add(data);
	}
#endif

}
