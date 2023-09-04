using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupCollection : UIBase
{
	[SerializeField] private UIContentToggle toggleEquip;
	[SerializeField] private UIContentToggle toggleSkill;
	[SerializeField] private UIContentToggle togglePet;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject itemPrefab;

	[SerializeField] private Transform contentStats;
	[SerializeField] private GameObject itemPrefabStats;

	private List<RuntimeData.CollectionInfo> infoList;

	private void Awake()
	{
		toggleEquip.onValueChanged.RemoveAllListeners();
		toggleEquip.onValueChanged.AddListener(OnToggleEquip);
		toggleSkill.onValueChanged.RemoveAllListeners();
		toggleSkill.onValueChanged.AddListener(OnToggleSkill);
		togglePet.onValueChanged.RemoveAllListeners();
		togglePet.onValueChanged.AddListener(OnTogglePet);

	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (GameManager.it != null)
		{
			GameManager.it.AddRewardEvent += Refresh;
		}
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		if (GameManager.it != null)
		{
			GameManager.it.AddRewardEvent -= Refresh;
		}
	}

	public void OnToggleEquip(bool isTrue)
	{
		if (isTrue)
		{
			OnUpdate(CollectionTab.EQUIP);
		}

	}
	public void OnToggleSkill(bool isTrue)
	{
		if (isTrue)
		{
			OnUpdate(CollectionTab.SKILL);
		}
	}
	public void OnTogglePet(bool isTrue)
	{
		if (isTrue)
		{
			OnUpdate(CollectionTab.PET);
		}
	}

	public void Show()
	{
		if (Activate() == false)
		{
			return;
		}
		toggleEquip.SetIsOnWithoutNotify(true);
		OnUpdate(CollectionTab.EQUIP);
	}
	CollectionTab currentTab;
	public void Refresh()
	{
		OnUpdate(currentTab);
	}
	public void OnUpdate(CollectionTab tab)
	{
		currentTab = tab;
		infoList = new List<RuntimeData.CollectionInfo>();
		infoList = PlatformManager.UserDB.collectionContainer[tab];

		SetGrid();
	}

	private void SetGrid()
	{
		content.CreateListCell(infoList.Count, itemPrefab, null);
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < infoList.Count)
			{
				UIItemCollection collection = child.GetComponent<UIItemCollection>();
				collection.OnUpdate(this, infoList[i]);
				child.gameObject.SetActive(true);
			}
		}


		var list = PlatformManager.UserDB.collectionContainer.CollectionBuff;
		contentStats.CreateListCell(list.Count, itemPrefabStats, null);

		StatsType[] typeLIst = new StatsType[list.Count];
		list.Keys.CopyTo(typeLIst, 0);

		RuntimeData.AbilityInfo[] abilityInfos = new RuntimeData.AbilityInfo[list.Count];
		list.Values.CopyTo(abilityInfos, 0);

		for (int i = 0; i < contentStats.childCount; i++)
		{
			Transform child = contentStats.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < list.Count)
			{
				UIItemStats collection = child.GetComponent<UIItemStats>();
				collection.OnUpdate(typeLIst[i].ToUIString(), $"+{abilityInfos[i].Value.ToString()}%");
				child.gameObject.SetActive(true);
			}
		}
	}
}
