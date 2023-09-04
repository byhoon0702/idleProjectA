using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPopupPetEvolution : UIBase
{
	[SerializeField] private UIPetSlot uiCurrentSlot;
	[SerializeField] private UIPetSlot uiNextSlot;

	[SerializeField] private Button buttonClose;
	[SerializeField] private Button buttonUpgrade;
	public Button ButtonUpgrade => buttonUpgrade;

	[SerializeField] private TextMeshProUGUI textCurrentInfo;
	[SerializeField] private TextMeshProUGUI textNextInfo;

	private RuntimeData.PetInfo itemInfo;
	private RuntimeData.PetInfo nextItemInfo;
	private UIManagementPet parent;
	private void Awake()
	{
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClose);
		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickUpgrade);
	}
	public void OnUpdate(UIManagementPet _parent, RuntimeData.PetInfo info)
	{
		gameObject.SetActive(true);
		parent = _parent;

		itemInfo = info;

		nextItemInfo = info.Clone();
		nextItemInfo.Evolution(false);
		nextItemInfo.UpdatePetSkill();

		uiCurrentSlot.OnUpdate(null, itemInfo);
		uiNextSlot.OnUpdate(null, nextItemInfo);

		var data = PlatformManager.CommonData.PetEvolutionLevelDataList[info.evolutionLevel];
		bool canEvolution = info.Count >= data.consumeCount;

		buttonUpgrade.interactable = canEvolution;

		info.PetSkill.MakeCompareDescritption(nextItemInfo.PetSkill, out string current, out string next);

		textCurrentInfo.text = current;
		textNextInfo.text = next;
	}
	public void OnClickUpgrade()
	{
		if (PlatformManager.UserDB.petContainer.EvolutionPet(itemInfo))
		{
			parent.OnUpdate(true);
			OnClose();
		}
	}
}
