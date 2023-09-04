using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UIItemDungeonList : MonoBehaviour
{
	[SerializeField] private Image mainImage;
	[SerializeField] private GameObject consumeObj;
	[SerializeField] private Image itemIconImage;
	[SerializeField] private TextMeshProUGUI itemCount;
	[SerializeField] private Button enterButton;
	[SerializeField] private UITextMeshPro uiTextdungeonName;
	[SerializeField] private UITextMeshPro uiTextDungeonDescription;
	[SerializeField] private Material grayScale;

	private UIPageBattleDungeon parent;
	private BattleData battleData;
	public BattleData BattleData => battleData;

	private void Awake()
	{
		enterButton.onClick.RemoveAllListeners();
		enterButton.onClick.AddListener(OnEnterButtonClick);
	}
	bool condition;
	string conditionMessage;
	public void SetData(UIPageBattleDungeon _owner, BattleData _data)
	{
		parent = _owner;
		battleData = _data;

		var rawData = DataManager.Get<ContentsDataSheet>().GetByType(_data.contentType);
		condition = true;
		if (rawData != null)
		{
			condition = rawData.Condition.IsFulFillCondition(out conditionMessage);
		}

		if (_data.dungeonItemTid != 0)
		{
			consumeObj.SetActive(true);
			PlatformManager.UserDB.inventory.GetScriptableObject(_data.dungeonItemTid, out CurrencyItemObject itemObject);
			if (itemObject != null)
			{
				var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

				itemIconImage.sprite = itemObject.ItemIcon;
				itemCount.text = $"{item.Value.ToString()}";
			}
		}
		else
		{
			consumeObj.SetActive(false);
		}
		var scriptable = PlatformManager.UserDB.stageContainer.GetScriptableObject<DungeonItemObject>(battleData.tid);
		if (scriptable != null)
		{
			mainImage.sprite = scriptable.ItemIcon;
		}

		uiTextdungeonName.SetKey(battleData.name);
		uiTextDungeonDescription.SetKey(battleData.description);

		if (condition == false)
		{
			mainImage.material = grayScale;
		}
		else
		{
			mainImage.material = null;
		}
	}

	public void OnRefresh()
	{
		if (battleData.dungeonItemTid != 0)
		{
			consumeObj.SetActive(true);
			PlatformManager.UserDB.inventory.GetScriptableObject(battleData.dungeonItemTid, out CurrencyItemObject itemObject);
			if (itemObject != null)
			{
				var item = PlatformManager.UserDB.inventory.FindCurrency(itemObject.currencyType);

				itemIconImage.sprite = itemObject.ItemIcon;
				itemCount.text = $"{item.Value.ToString()}";
			}
		}
		else
		{
			consumeObj.SetActive(false);
		}

		var scriptable = PlatformManager.UserDB.stageContainer.GetScriptableObject<DungeonItemObject>(battleData.tid);
		if (scriptable != null)
		{
			mainImage.sprite = scriptable.ItemIcon;
		}
		uiTextdungeonName.SetKey(battleData.name);
		uiTextDungeonDescription.SetKey(battleData.description);
	}

	public void OnEnterButtonClick()
	{
		if (condition == false)
		{
			ToastUI.Instance.Enqueue(conditionMessage);
			return;
		}

		var info = PlatformManager.UserDB.stageContainer.LastPlayedStage(battleData.stageType, battleData.tid);

		if (info.Rule is StageImmortal || info.Rule is StageInfinity)
		{
			parent.ShowDungeonPopup(battleData);
		}
		else
		{
			parent.ShowDungeonStagePopup(battleData);
		}
	}
}
