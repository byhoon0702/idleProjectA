using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModelPoolManager : MonoBehaviour
{
	private static UnitModelPoolManager instance;
	public static UnitModelPoolManager Instance => instance;
	public static UnitModelPoolManager it => instance;

	[SerializeField] private Transform modelParentTransform;

	private Dictionary<string, List<GameObject>> dictionaryModel = new Dictionary<string, List<GameObject>>();

	private void Awake()
	{
		instance = this;
	}

	public GameObject GetModel(string _modelName)
	{
		if (dictionaryModel.TryGetValue(_modelName, out var listModel) == true)
		{
			for (int i = 0; i < listModel.Count; i++)
			{
				var model = listModel[i];
				if (model == null)
				{
					continue;
				}
				if (model.GetComponent<UnitAnimation>().IsResetComplete() == false)
				{
					continue;
				}
				if (model.transform.parent == modelParentTransform)
				{
					model.gameObject.SetActive(true);
					return model;
				}
			}
			var newModel = Instantiate(Resources.Load(_modelName)) as GameObject;
			listModel.Add(newModel);
			newModel.gameObject.SetActive(true);
			return newModel;
		}
		else
		{
			var list = new List<GameObject>();
			var model = Instantiate(Resources.Load(_modelName), modelParentTransform) as GameObject;
			list.Add(model);
			dictionaryModel.Add(_modelName, list);

			model.gameObject.SetActive(true);
			return model;
		}
	}

	public void ReturnModel(GameObject _model)
	{
		if (_model == null)
		{
			return;
		}
		_model.transform.SetParent(modelParentTransform, false);
		_model.GetComponent<UnitAnimation>().ResetAnimation();
	}

	public void ClearPool()
	{
		foreach (var model_list in dictionaryModel.Values)
		{
			for (int i = 0; i < model_list.Count; i++)
			{
				var model = model_list[i];
				Destroy(model.gameObject);
			}
			model_list.Clear();
		}
		dictionaryModel.Clear();
		dictionaryModel = new Dictionary<string, List<GameObject>>();
	}
}
