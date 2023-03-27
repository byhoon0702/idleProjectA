using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIItemEquipSlot : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private Image iconImage;

	private UIManagementMain owner;
	private EquipType itemType;
	private long selectedItemTid;

	[Header("레벨")]
	[SerializeField] private GameObject levelObj;
	[SerializeField] private TextMeshProUGUI levelText;

	[Header("게이지(승급)")]
	[SerializeField] private GameObject sliderObj;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI sliderText;

	[Header("아이템 개수")]
	[SerializeField] private GameObject itemCountObj;
	[SerializeField] private TextMeshProUGUI itemCountText;

	[Header("능력")]
	[SerializeField] private GameObject gradeObj;
	[SerializeField] private GameObject[] gradeList;
	[SerializeField] private GameObject starObj;
	[SerializeField] private GameObject[] starList;

	[Header("돌파")]
	[SerializeField] private GameObject upgradeObj;
	[SerializeField] private TextMeshProUGUI upgradeText;

	[Header("초월")]
	[SerializeField] private GameObject mergeObj;
	[SerializeField] private TextMeshProUGUI mergeText;

	[SerializeField] private GameObject notOwnedMark;
	[SerializeField] private TextMeshProUGUI notOwnedText;

	public EquipSlot itemData { get; private set; }
	private Action<ItemData> onClick;
	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	/// <summary>
	/// 강화,승급 레벨업등이 가능한 아이템용
	/// </summary>
	public void OnUpdate(EquipSlot _itemData, int _level = 0, long _sliderCurrent = 0, long _sliderMax = 0, int _upgradeLv = 0, int _mergeLv = 0)
	{
		itemData = _itemData;
		iconImage.sprite = itemData.icon;

		UpdateLevel(_level);
		UpdateSlider(_sliderCurrent, _sliderMax);
		//UpdateGrade(_itemData.itemType, _itemData.itemGrade);
		//UpdateStar(_itemData.starLv);
		UpdateUpgrade(_upgradeLv);
		UpdateMerge(_mergeLv);
	}


	/// 일반 아이템용
	/// </summary>
	public void OnUpdate(EquipSlot _itemData, IdleNumber _itemCount)
	{
		itemData = _itemData;
		iconImage.sprite = itemData.icon;//Resources.Load<Sprite>(PathHelper.Icon(_itemData.Icon));

		UpdateLevel(0);
		UpdateItemCount(_itemCount);
		//UpdateGrade(_itemData.itemType, _itemData.itemGrade);
		//UpdateStar(_itemData.starLv);
		UpdateUpgrade(0);
		UpdateMerge(0);
	}

	public void OnUpdate(EquipSlot _itemEquip)
	{
		var item = VGameManager.it.userDB.inventory.FindEquipItem(_itemEquip.itemTid, _itemEquip.type);
		OnUpdate(_itemEquip, item != null ? item.level : 0, 0, 0, 0, 0);
	}

	public void UpdateLevel(int _level = 0)
	{
		if (_level == 0)
		{
			levelObj.SetActive(false);
		}
		else
		{
			levelObj.SetActive(true);
			levelText.text = $"Lv. {_level}";
		}
	}

	public void UpdateSlider(long _current, long _max)
	{
		itemCountObj.SetActive(false);
		if (_current == 0 && _max == 0)
		{
			sliderObj.gameObject.SetActive(false);
		}
		else
		{
			sliderObj.gameObject.SetActive(true);
			slider.value = Mathf.Clamp01((float)_current / _max);
			sliderText.text = $"{_current}/{_max}";
		}
	}

	public void UpdateItemCount(IdleNumber _itemCount)
	{
		sliderObj.gameObject.SetActive(false);

		if (_itemCount == 0)
		{
			itemCountObj.gameObject.SetActive(false);
		}
		else
		{
			itemCountObj.gameObject.SetActive(true);
			itemCountText.text = _itemCount.ToString();
		}
	}

	public void UpdateGrade(ItemType _itemType, Grade _grade)
	{
		foreach (var v in gradeList)
		{
			v.gameObject.SetActive(false);
		}

		if (_itemType == ItemType.Weapon ||
			_itemType == ItemType.Armor ||
			_itemType == ItemType.Ring ||
			_itemType == ItemType.Necklace ||
			_itemType == ItemType.Skill ||
			_itemType == ItemType.Unit ||
			_itemType == ItemType.Pet)
		{

			try
			{
				gradeList[(int)_grade].gameObject.SetActive(true);
			}
			catch
			{
				VLog.LogError($"Invalid Grade. {_grade}");
			}
		}
	}

	public void UpdateStar(int _star = 0)
	{
		for (int i = 0; i < starList.Length; i++)
		{
			starList[i].SetActive(_star >= (i + 1));
		}
	}

	public void UpdateUpgrade(int _upgradeLv = 0)
	{
		if (_upgradeLv == 0)
		{
			upgradeObj.SetActive(false);
		}
		else
		{
			upgradeObj.SetActive(true);
			upgradeText.text = $"+{_upgradeLv}";
		}
	}

	public void UpdateMerge(int _mergeLv = 0)
	{
		if (_mergeLv == 0)
		{
			mergeObj.SetActive(false);
		}
		else
		{
			mergeObj.SetActive(true);
			mergeText.text = $"+{_mergeLv}";
		}
	}

	public void OnUpdate(UIManagementMain _owner, EquipType _itemType, EquipSlot slotData)
	{
		owner = _owner;
		itemType = _itemType;
		selectedItemTid = 0;

		if (slotData != null && slotData.item != null && slotData.item != null)
		{
			selectedItemTid = slotData.item.tid;
		}

		if (_itemType == EquipType.WEAPON)
		{

		}

		OnUpdate(slotData);
	}

	private void OnClick()
	{
		owner.ShowEquipUi(itemType);
	}

}
