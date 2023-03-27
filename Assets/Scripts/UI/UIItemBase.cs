using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemBase : MonoBehaviour
{
	[SerializeField] private Image iconImage;
	[SerializeField] private Button button;

	[Space(10)]
	[SerializeField] private GameObject selectedObj;
	[SerializeField] private GameObject noPossessedObj;
	[SerializeField] private GameObject equipedObj;

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


	public ItemData itemData { get; private set; }
	private Action<ItemData> onClick;


	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	/// <summary>
	/// 강화,승급 레벨업등이 가능한 아이템용
	/// </summary>
	public void OnUpdate(ItemData _itemData, int _level = 0, long _sliderCurrent = 0, long _sliderMax = 0, int _upgradeLv = 0, int _mergeLv = 0)
	{
		itemData = _itemData;
		iconImage.sprite = Resources.Load<Sprite>(PathHelper.Icon(_itemData.Icon));

		SetSelected(false);
		SetEquipped(false);
		SetNoPossessed(false);


		UpdateLevel(_level);
		UpdateSlider(_sliderCurrent, _sliderMax);
		UpdateGrade(_itemData.itemType, _itemData.itemGrade);
		UpdateStar(_itemData.starLv);
		UpdateUpgrade(_upgradeLv);
		UpdateMerge(_mergeLv);
	}

	public void OnUpdate(MetaRewardInfo _metaReward)
	{
		var itemData = DataManager.GetFromAll<ItemData>(_metaReward.tid);
		if (itemData == null)
		{
			VLog.LogError($"Invalid Reward tid. {_metaReward.tid}");
			gameObject.SetActive(false);
		}

		OnUpdate(itemData, (IdleNumber)_metaReward.count);
	}

	public void OnUpdate(StageRewardInfo _stageReward)
	{
		var itemData = DataManager.GetFromAll<ItemData>(_stageReward.Tid);
		if (itemData == null)
		{
			VLog.LogError($"Invalid Reward tid. {_stageReward.Tid}");
			gameObject.SetActive(false);
		}

		OnUpdate(itemData, _stageReward.RewardCountDefault());
	}

	/// <summary>
	/// 일반 아이템용
	/// </summary>
	public void OnUpdate(ItemData _itemData, IdleNumber _itemCount)
	{
		itemData = _itemData;
		iconImage.sprite = Resources.Load<Sprite>(PathHelper.Icon(_itemData.Icon));

		SetSelected(false);
		SetEquipped(false);
		SetNoPossessed(false);


		UpdateLevel(0);
		UpdateItemCount(_itemCount);
		UpdateGrade(_itemData.itemType, _itemData.itemGrade);
		UpdateStar(_itemData.starLv);
		UpdateUpgrade(0);
		UpdateMerge(0);
	}

	public void OnUpdate(ItemEquip _itemEquip)
	{
		OnUpdate(_itemEquip.data, _itemEquip.Level, _itemEquip.Exp, _itemEquip.nextExp, _itemEquip.UpgradeLevel, _itemEquip.MergeLevel);
	}

	public void AddClickEvent(Action<ItemData> _onClick)
	{
		onClick = _onClick;
	}

	private void OnClick()
	{
		onClick?.Invoke(itemData);
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


	public void SetSelected(bool _isSelect)
	{
		selectedObj.gameObject.SetActive(_isSelect);
	}

	public void SetEquipped(bool _isEquip)
	{
		equipedObj.gameObject.SetActive(_isEquip);
	}

	public void SetNoPossessed(bool _isNoPossessed)
	{
		noPossessedObj.gameObject.SetActive(_isNoPossessed);
	}
}
