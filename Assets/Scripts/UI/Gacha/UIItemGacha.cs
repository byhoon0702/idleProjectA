using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemGacha : MonoBehaviour
{
	[SerializeField] private Button helpButton;

	[Header("재화")]
	[SerializeField] private TextMeshProUGUI moneyCount;

	[Header("레벨")]
	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI expText;

	[Header("버튼")]
	[SerializeField] private UIGachaButton gachaButton;

	private UIGacha owner;
	private UIGachaData uiData;




	private void Awake()
	{
		helpButton.onClick.RemoveAllListeners();
		helpButton.onClick.AddListener(OnHelpButtonClick);
	}
	private void OnEnable()
	{
		EventCallbacks.onItemChanged += OnItemChanged;
	}

	private void OnDisable()
	{
		EventCallbacks.onItemChanged -= OnItemChanged;
	}

	private void OnItemChanged(List<long> _changedItems)
	{
		foreach (var tid in _changedItems)
		{
			if (tid != Inventory.it.GoldTid) // 아이템 TID찾는것보다 이게 더 효율적일듯
			{
				UpdateExp();
				gachaButton.OnUpdate(uiData);
				return;
			}
		}
	}

	public void OnUpdate(UIGacha _owner, UIGachaData _gachaData)
	{
		owner = _owner;
		uiData = _gachaData;

		UpdateMoney();
		UpdateExp();
		gachaButton.OnUpdate(uiData);
	}

	public void UpdateMoney()
	{
		moneyCount.text = Inventory.it.ItemCount(Inventory.it.DiaTid).ToString();
	}

	public void UpdateExp()
	{
		int level = 1;
		long currExp = 0;
		long nextExp = 0;

		switch (uiData.GachaType)
		{
			case GachaType.Equip:
				level = UserInfo.GachaEquipLv;
				currExp = UserInfo.GachaEquipExp;
				nextExp = UserInfo.NextGachaEquipExp;
				break;
			case GachaType.Skill:
				level = UserInfo.GachaSkillLv;
				currExp = UserInfo.GachaSkillExp;
				nextExp = UserInfo.NextGachaSkillExp;
				break;
			case GachaType.Pet:
				level = UserInfo.GachaPetLv;
				currExp = UserInfo.GachaPetExp;
				nextExp = UserInfo.NextGachaPetExp;
				break;
		}


		float expRatio = Mathf.Clamp01((float)currExp / nextExp);

		expText.text = $"Lv. {level} - {currExp} / {nextExp}";
		expSlider.value = expRatio;
	}



	private void OnHelpButtonClick()
	{
		owner.ShowProbabilityPopup(uiData.GachaLevel, uiData);
	}
}
