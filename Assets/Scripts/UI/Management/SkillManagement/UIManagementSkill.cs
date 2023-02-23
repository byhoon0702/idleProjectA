using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManagementSkill : MonoBehaviour
{
	[SerializeField] private UISkillInfoPopup skillInfoPopup;

	[Header("장착스킬")]
	[SerializeField] private UIItemSkillSlot[] skillSlots;
	
	
	[Header("아이템리스트")]
	[SerializeField] private UIItemSkillSlot itemPrefab;
	[SerializeField] private RectTransform itemRoot;

	[Header("Button")]
	[SerializeField] private Button summonButton;
	[SerializeField] private Button upgradeButton;



	public void OnUpdate(bool _refreshGrid)
	{
		UpdateEquipItem();
		UpdateItem(_refreshGrid);
		UpdateButton();
	}

	public void UpdateEquipItem()
	{
		for(int i=0 ; i<UserInfo.skills.Length ; i++)
		{
			long itemTid = UserInfo.skills[i];
			if(itemTid == 0)
			{
				skillSlots[i].gameObject.SetActive(false);
			}
			else
			{
				UISkillData uiData = new UISkillData();
				VResult result = uiData.Setup(itemTid);
				if(result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				skillSlots[i].gameObject.SetActive(true);
				skillSlots[i].OnUpdate(this, uiData);
			}
		}
	}

	public void UpdateItem(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemSkillSlot>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Skill))
			{
				UISkillData uiData = new UISkillData();
				VResult result = uiData.Setup(v);
				if (result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				if (uiData.IsHyperSkill)
				{
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemSkillSlot>())
		{
			v.OnRefresh();
		}
	}

	private void UpdateButton()
	{
		summonButton.interactable = false;
		upgradeButton.interactable = false;
	}

	public void ShowItemInfo(UISkillData _uiData)
	{
		skillInfoPopup.gameObject.SetActive(true);
		skillInfoPopup.OnUpdate(this, _uiData);
	}

	private int GetEmptySlot()
	{
		for (int i = 0 ; i < UserInfo.skills.Length ; i++)
		{
			if (UserInfo.skills[i] == 0)
			{
				return i;
			}
		}

		return -1;
	}


	public void EquipSkill(long _itemTid)
	{
		int emptySlotIndex = GetEmptySlot();

		if (emptySlotIndex == -1)
		{
			ToastUI.it.Create("비어있는 슬롯이 엄슴");
			return;
		}

		UserInfo.EquipSkill(emptySlotIndex, _itemTid);

		UpdateEquipItem();
		UpdateItem(true);

		UnitGlobal.it.skillModule.InitSkill(UserInfo.skills);
		UnitGlobal.it.skillModule.SetUnit(UnitManager.it.Player);
		UIController.it.SkillGlobal.OnUpdate();
	}

	public void UnEquipSkill(long _itemTid)
	{
		//스킬 사용중일땐 빼기 불가
		SkillBase skillBase = UnitGlobal.it.skillModule.FindSkillBase(_itemTid);
		if (skillBase != null)
		{
			if (skillBase.skillUseRemainTime > 0)
			{
				ToastUI.it.Create("스킬 사용중일땐 못뺌");
				return;
			}
		}

		for (int i = 0 ; i < UserInfo.skills.Length ; i++)
		{
			if (UserInfo.skills[i] == _itemTid)
			{
				UserInfo.EquipSkill(i, 0);
			}
		}

		UpdateEquipItem();
		UpdateItem(true);

		UnitGlobal.it.skillModule.InitSkill(UserInfo.skills);
		UnitGlobal.it.skillModule.SetUnit(UnitManager.it.Player);
		UIController.it.SkillGlobal.OnUpdate();
	}
}
