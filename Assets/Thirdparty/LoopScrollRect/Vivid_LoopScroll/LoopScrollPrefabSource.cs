using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
#if !PROJECT_B
[System.Serializable]
public class LoopScrollPrefabSource
{
	public GameObject m_prefab_source;
	public int m_pool_size;
	private bool m_use_unity_pool = true;

	private bool inited = false;

	public virtual GameObject GetObject()
	{
		if (m_use_unity_pool)
		{
			if (!inited)
			{
				UnityPoolManager.it.Init(m_prefab_source, m_pool_size);
				inited = true;
			}

			return UnityPoolManager.it.GetObject(m_prefab_source.name);
		}

		if (!inited)
		{
			SG.ResourceManager.Instance.InitPool(m_prefab_source, m_pool_size);
			inited = true;
		}
		return SG.ResourceManager.Instance.GetObjectFromPool(m_prefab_source.name);
	}

	public virtual void ReturnObject(Transform go)
	{
		go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
		if (m_use_unity_pool)
		{
			UnityPoolManager.it.ReturnObject(m_prefab_source.name, go.gameObject);
			return;
		}


		SG.ResourceManager.Instance.ReturnObjectToPool(go.gameObject);
	}

	public virtual void RemovePoolList()
	{
		
		if(UnityEnv.IsApplicationQuit)
		{
			return;
		}

		if (m_prefab_source == null)
		{
			return;
		}
		if (m_use_unity_pool)
		{
			UnityPoolManager.it.RemovePool(m_prefab_source.name, m_prefab_source);
			return;
		}

		SG.ResourceManager.Instance.RemovePool(m_prefab_source.name);
	}
}
#endif
