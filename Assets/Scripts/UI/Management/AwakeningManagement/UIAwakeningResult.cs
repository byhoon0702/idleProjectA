using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UIAwakeningResult : UIBase
{
	[SerializeField] private Button objBG;

	[SerializeField] private Transform transformStand;
	[SerializeField] private UIItemStats[] uiStats;

	RuntimeData.AwakeningInfo _info;
	private UnitAnimation unit;


	protected override void OnEnable()
	{
		base.OnEnable();
		objBG.SetButtonEvent(Close);
	}

	protected override void OnDisable()
	{
		base.OnDisable();

	}
	protected override void OnClose()
	{
		base.OnClose();
		objBG.gameObject.SetActive(false);
		if (unit != null)
		{
			Destroy(unit.gameObject);
		}
	}

	public void SetData(RuntimeData.AwakeningInfo info)
	{
		_info = info;
		objBG.gameObject.SetActive(true);

		CreateUnitForUI(PlatformManager.UserDB.costumeContainer[CostumeType.HYPER].costume);

		for (int i = 0; i < uiStats.Length; i++)
		{
			uiStats[i].OnUpdate(_info.AbilityInfos[i]);
		}
	}
	public void CreateUnitForUI(GameObject go)
	{
		if (unit != null)
		{
			Destroy(unit.gameObject);
			unit = null;
		}

		var obj = Instantiate(go);

		obj.transform.SetParent(transformStand);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		unit = obj.GetComponent<UnitAnimation>();


		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 5;
		unit.PlayAnimation("idle");
	}

}
