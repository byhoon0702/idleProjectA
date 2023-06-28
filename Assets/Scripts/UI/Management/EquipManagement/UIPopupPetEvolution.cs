using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPopupPetEvolution : MonoBehaviour
{
	[SerializeField] private UIPetSlot uiCurrentSlot;
	[SerializeField] private UIPetSlot uiNextSlot;

	[SerializeField] private Button buttonClose;
	[SerializeField] private Button buttonUpgrade;

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
		nextItemInfo.evolutionLevel++;

		uiCurrentSlot.OnUpdate(null, itemInfo);
		uiNextSlot.OnUpdate(null, nextItemInfo);
	}
	public void OnClickUpgrade()
	{
		//if (nextItemInfo == null)
		//{
		//	return;
		//}
		//if (itemInfo.count < 1)
		//{
		//	return;
		//}

		GameManager.UserDB.petContainer.EvolutionPet(itemInfo);
		parent.UpdateInfo();
		OnClose();
	}
	public void OnClose()
	{
		gameObject.SetActive(false);
	}
}
