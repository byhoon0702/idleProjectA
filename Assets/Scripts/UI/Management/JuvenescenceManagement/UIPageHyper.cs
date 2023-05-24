using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;


public class UIPageHyper : MonoBehaviour
{

	[SerializeField] private Transform pivot;

	[SerializeField] Toggle toggleWarrior;
	[SerializeField] Toggle toggleMercenary;
	[SerializeField] Toggle toggleKnight;
	[SerializeField] Toggle toggleShooter;

	[SerializeField] Button buttonClassChange;
	[SerializeField] Button buttonUpgrade;

	[SerializeField] private UIStatHyperClass[] uiStatHyperClass;
	[SerializeField] private UIItemHyperClass uiStatHyperClassCenter;

	private UIManagementJuvenescence parent;
	private UnitCostume unitCostume;
	private List<RuntimeData.HyperClassInfo> infos;
	private RuntimeData.HyperClassInfo currentInfo;

	public void Init(UIManagementJuvenescence _parent)
	{
		parent = _parent;
	}

	private void Awake()
	{
		buttonClassChange.onClick.RemoveAllListeners();
		buttonClassChange.onClick.AddListener(OnClickChangeClass);
		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickLevelUp);
	}

	private void OnEnable()
	{
		infos = new List<RuntimeData.HyperClassInfo>(GameManager.UserDB.juvenescenceContainer.hyperClassInfos);
		toggleWarrior.isOn = true;
		ToggleWarrior(toggleWarrior.isOn);

	}

	private void OnUpdate()
	{
		if (currentInfo == null)
		{
			return;
		}
		for (int i = 0; i < uiStatHyperClass.Length; i++)
		{
			uiStatHyperClass[i].OnUpdate(this, currentInfo.subLevels[i]);
		}
		uiStatHyperClassCenter.OnUpdate(this, currentInfo);
	}

	public void OnClickLevelUp()
	{
		currentInfo.LevelUp();
		OnUpdate();
	}

	public void OnClickChangeClass()
	{

	}

	public void ToggleWarrior(bool isOn)
	{
		if (isOn)
		{
			currentInfo = infos.Find(x => x.rawData.hyperClass == HyperClass.WARRIOR);
			OnUpdate();
		}
	}

	public void ToggleMercenary(bool isOn)
	{
		if (isOn)
		{
			currentInfo = infos.Find(x => x.rawData.hyperClass == HyperClass.MERCENARY_KING);
			OnUpdate();
		}
	}

	public void ToggleKnight(bool isOn)
	{
		if (isOn)
		{
			currentInfo = infos.Find(x => x.rawData.hyperClass == HyperClass.KNIGHT_KING);
			OnUpdate();
		}
	}

	public void ToggleShooter(bool isOn)
	{
		if (isOn)
		{
			currentInfo = infos.Find(x => x.rawData.hyperClass == HyperClass.SHARPSHOOTER);
			OnUpdate();
		}
	}

	public void CreateUnitForUI(string resource)
	{

		//if (unitCostume != null)
		//{
		//	Destroy(unitCostume.gameObject);
		//	unitCostume = null;
		//}
		//var obj = UnitModelPoolManager.it.Get("B/Player", resource);

		//obj.transform.SetParent(pivot);
		//obj.transform.localPosition = Vector3.zero;
		//obj.transform.localScale = Vector3.one;
		//obj.transform.localRotation = Quaternion.identity;

		//unitCostume = obj.GetComponent<UnitCostume>();
		//unitCostume.Init();
		//var head = GameManager.UserDB.costumeContainer.defaultHead;
		//var body = GameManager.UserDB.costumeContainer.defaultBody;
		//var weapon = GameManager.UserDB.costumeContainer.defaultWeapon;

		//unitCostume.ChangeCostume(head, body, weapon);

		////UnitFacial unitFacial = obj.GetComponent<UnitFacial>();
		////unitFacial.ChangeFacial(info.level);
		//SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
		//sortingGroup.sortingLayerName = "UI";
		//sortingGroup.sortingOrder = 1;
	}

}

