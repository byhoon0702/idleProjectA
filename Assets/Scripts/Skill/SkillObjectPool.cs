using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SkillObjectPool : VObjectPool<GameObject>
{
	private static SkillObjectPool instance;

	public static SkillObjectPool it
	{
		get
		{

			if (instance == null)
			{
				var go = GameObject.Find("SkillObjectPool");
				if (go != null)
				{
					instance = go.GetComponent<SkillObjectPool>();

					if (instance == null)
					{
						instance = go.AddComponent<SkillObjectPool>();
					}
				}
				else
				{
					go = new GameObject("SkillObjectPool");
					instance = go.AddComponent<SkillObjectPool>();
				}

			}

			return instance;

		}
	}


	protected override void SetObject(GameObject _object, IObjectPool<GameObject> _pool)
	{
		if (_object == null)
		{
			return;
		}
		//_object.Set(_pool);
	}

	protected override string GetPath(string _path, string _name)
	{
		return $"{PathHelper.particleEffectPath}/{_name}";
	}
	protected override GameObject OnCreateObject(GameObject _object)
	{
		if (_object == null)
		{
			return null;
		}
		//if (GetResource(_object.name) == null)
		//{
		//	return null;
		//}
		GameObject effect = Instantiate(_object, transform);
		effect.name = effect.name.Replace("(Clone)", "");
		return effect;
	}


	protected override GameObject OnCreateObject(string _path, string _name)
	{
		if (GetResource("", _name) == null)
		{
			return null;
		}
		GameObject effect = Instantiate(GetResource("", _name), transform);
		effect.name = _name;
		return effect;
	}

	protected override void OnGetObject(GameObject _object)

	{
		if (_object == null)
		{
			return;
		}

		base.OnGetObject(_object);
		_object.gameObject.SetActive(true);
	}

	protected override void OnReleaseObject(GameObject _object)
	{
		if (_object == null)
		{
			return;
		}
		base.OnReleaseObject(_object);


		_object.gameObject.SetActive(false);
	}

	protected override void OnDestroyObject(GameObject _object)
	{
		if (_object == null)
		{
			return;
		}
		Destroy(_object.gameObject);
	}
}
