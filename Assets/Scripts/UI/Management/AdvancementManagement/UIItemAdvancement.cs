using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using System;
using JetBrains.Annotations;

public class UIItemAdvancement : MonoBehaviour
{
	[SerializeField] private Transform pivot;
	[SerializeField] private UIStatsInfoCell[] statInfos;
	[SerializeField] private TextMeshProUGUI textCondition;
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private Button buttonActivate;
	[SerializeField] private Button buttonCostume;

	private NormalUnitCostume unitCostume;
	private UnitAdvancementInfo info;
	private Action onclick;

	private UIManagementAdvancement parent;


	private void Awake()
	{
		buttonActivate.onClick.RemoveAllListeners();
		buttonActivate.onClick.AddListener(OnClickActivate);
		buttonCostume.onClick.RemoveAllListeners();
		buttonCostume.onClick.AddListener(OnClickCostumeChange);
	}
	public void OnUpdate(UIManagementAdvancement _parent, UnitAdvancementInfo _info)
	{
		parent = _parent;
		info = _info;

		for (int i = 0; i < statInfos.Length; i++)
		{
			if (i < info.stats.Count)
			{
				statInfos[i].gameObject.SetActive(true);
				statInfos[i].OnUpdate(info.stats[i].type.ToUIString(), $"<color=green>+{info.stats[i].value}%</color>");
			}
			else
			{
				statInfos[i].gameObject.SetActive(false);
			}
		}

		textTitle.text = info.nameKey;

		CreateUnitForUI();
		UpdateInfo();
	}

	public void UpdateInfo()
	{


		string message = "";
		bool isCleared = false;
		if (GameManager.UserDB.advancementContainer.Info.CostumeIndex == info.level)
		{
			message = "착용중";
		}
		else
		{
			var data = DataManager.Get<DungeonStageDataSheet>().Get(info.requirement.tid);
			if (data == null)
			{
				return;
			}
			isCleared = GameManager.UserDB.stageContainer.IsCleared(data.stageType, data.difficulty, data.areaNumber, info.requirement.stageNumber);
			if (isCleared == false)
			{
				message = $"{data.name} {data.areaNumber}_{info.requirement.stageNumber} 클리어";
			}
		}
		buttonActivate.gameObject.SetActive(isCleared && GameManager.UserDB.advancementContainer.Info.Level + 1 == info.level);
		buttonCostume.gameObject.SetActive(isCleared && GameManager.UserDB.advancementContainer.Info.CostumeIndex != info.level);
		textCondition.text = message;
	}


	public void CreateUnitForUI()
	{
		if (info.resource.IsNullOrEmpty())
		{
			return;
		}

		if (unitCostume != null)
		{
			Destroy(unitCostume.gameObject);
			unitCostume = null;
		}
		var obj = UnitModelPoolManager.it.Get("B/Player", "player_01_old"/*info.resource*/);

		obj.transform.SetParent(pivot);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;

		unitCostume = obj.GetComponent<NormalUnitCostume>();
		unitCostume.Init();
		var head = GameManager.UserDB.costumeContainer.defaultHead;
		var body = GameManager.UserDB.costumeContainer.defaultBody;
		var weapon = GameManager.UserDB.costumeContainer.defaultWeapon;

		unitCostume.ChangeCostume(head, body, weapon);

		UnitFacial unitFacial = obj.GetComponent<UnitFacial>();
		unitFacial.ChangeFacial(info.level);
		SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		sortingGroup.sortingLayerName = "UI";
		sortingGroup.sortingOrder = 1;
	}

	public void OnClickActivate()
	{
		GameManager.UserDB.advancementContainer.LevelUp(UnitManager.it.Player, info);
		parent.Refresh();
	}

	public void OnClickCostumeChange()
	{
		if (UnitManager.it.Player != null)
		{
			GameManager.UserDB.advancementContainer.ChangeCostume(UnitManager.it.Player, info);
		}
		parent.Refresh();
	}
}
