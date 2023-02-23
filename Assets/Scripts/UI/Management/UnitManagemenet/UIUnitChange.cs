using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



/// <summary>
/// <see cref="ChangeCurrentUnit"/> 보여지는 유닛 변경(UI만 갱신)
/// <see cref="OnEquipButtonClick" /> 장착유닛 변경(UI, 착용)
/// <see cref="OnLevelupButtonClick"/> 유닛 레벨업 시도
/// 
/// ★ 리스트 갱신은 필요시에만 해주세요 ★
/// </summary>
public class UIUnitChange : MonoBehaviour, IUIClosable
{
	[Header("캐릭터")]
	[SerializeField] private Image icon;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI gradeText;
	[SerializeField] private TextMeshProUGUI toOwnText;
	[SerializeField] private TextMeshProUGUI levelText;

	[SerializeField] private Slider expSlider;
	[SerializeField] private TextMeshProUGUI textExp;


	[Header("스킬")]
	[SerializeField] private UImanagementUnitSkillInfo unitSkill;
	[SerializeField] private UImanagementUnitSkillInfo finalSkill;

	[Header("리스트")]
	[SerializeField] private UIItemUnitChange itemPrefab;
	[SerializeField] private RectTransform itemRoot;


	[Header("버튼")]
	[SerializeField] private Button closeButton;
	[SerializeField] private Button equipUnitButton;
	[SerializeField] private RepeatButton levelupButton;
	[SerializeField] private TextMeshProUGUI levelupConsumeText;

	private UIUnitData selectedUnitInfo;



	private void Awake()
	{
		equipUnitButton.onClick.RemoveAllListeners();
		equipUnitButton.onClick.AddListener(OnEquipButtonClick);

		levelupButton.repeatCallback += OnLevelupButtonClick;

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(Close);
	}

	private void OnDisable()
	{
		UserInfo.SaveUserData();
	}

	public void OnUpdate(long _itemTid, bool _refreshGrid)
	{
		UpdateItems(_refreshGrid);

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemUnitChange>())
		{
			if(v.UIData.ItemTid == _itemTid)
			{ 
				selectedUnitInfo = v.UIData;
				break;
			}
		}
		UpdateSelected();


		UpdateUnitInfo();
		UpdateLevelupInfo();
		UpdateSkill();
		UpdateButton();
	}


	public void ChangeCurrentUnit(long _itemTid)
	{
		OnUpdate(_itemTid, true);
	}

	public void UpdateUnitInfo()
	{
		nameText.text = selectedUnitInfo.UnitName;
		gradeText.text = selectedUnitInfo.UnitGradeText;

		icon.sprite = Resources.Load<Sprite>($"Icon/{selectedUnitInfo.Icon}");
	}

	public void UpdateLevelupInfo()
	{
		toOwnText.text = selectedUnitInfo.ToOwnText;
		levelText.text = selectedUnitInfo.UnitLevelText;
		expSlider.value = selectedUnitInfo.ExpRatio;
		textExp.text = selectedUnitInfo.ExpText;
	}

	public void UpdateSkill()
	{
		unitSkill.OnUpdate(selectedUnitInfo.unitSkillData);
		finalSkill.OnUpdate(selectedUnitInfo.finalSkillData);
	}

	private void UpdateButton()
	{
		levelupConsumeText.text = $"{Inventory.it.ItemCount(selectedUnitInfo.LevelupConsumeHashtag).ToString()} / {selectedUnitInfo.LevelupConsumeCount.ToString()}";

		var item = Inventory.it.FindItemByTid(selectedUnitInfo.ItemTid) as ItemUnit;
		if (item != null)
		{
			bool buttonActive = item.Count > 0;
			bool equipped = selectedUnitInfo.ItemTid != UserInfo.EquipUnitItemTid;
			bool levelupable = selectedUnitInfo.Levelupable();

			equipUnitButton.interactable = buttonActive && equipped;
			levelupButton.SetInteractable(buttonActive && levelupable);
		}
		else
		{
			equipUnitButton.interactable = false;
			levelupButton.SetInteractable(false);
		}
	}

	private void UpdateItems(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemUnitChange>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Unit))
			{
				UIUnitData uiData = new UIUnitData();
				VResult result = uiData.Setup(v);
				if(result.Fail())
				{
					VLog.LogError(result.ToString());
					continue;
				}

				var item = Instantiate(itemPrefab, itemRoot);
				item.OnUpdate(this, uiData);
			}
		}

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemUnitChange>())
		{
			v.OnRefresh();
		}
	}

	private void UpdateSelected()
	{
		foreach (var v in itemRoot.GetComponentsInChildren<UIItemUnitChange>())
		{
			v.SetSelect(v.UIData.ItemTid == selectedUnitInfo.ItemTid);
			v.SetEquipped(v.UIData.ItemTid == UserInfo.EquipUnitItemTid);
		}
	}


	private void OnEquipButtonClick()
	{
		var item = selectedUnitInfo.GetItem();

		if (item == null || item.Count == 0)
		{
			ToastUI.it.Create("미보유 아이템");
			return;
		}

		if (item.Equipable())
		{
			UserInfo.EquipUnit(item.Tid);
			OnUpdate(item.Tid, true);
			StageManager.it.ResetStage();
		}
	}

	private void OnLevelupButtonClick()
	{
		selectedUnitInfo.LevelupItem(() =>
		{
			UpdateLevelupInfo();
			UpdateItems(true);
			UpdateSelected();
			UpdateButton();
		});
	}

	public bool Closable()
	{
		return true;
	}

	public void Close()
	{
		gameObject.SetActive(false);

		var uiMain = FindObjectOfType<UIManagementMain>();
		if (uiMain != null)
		{
			uiMain.UpdateUnitInfo();
			uiMain.UpdateSkill();
		}
	}
}
