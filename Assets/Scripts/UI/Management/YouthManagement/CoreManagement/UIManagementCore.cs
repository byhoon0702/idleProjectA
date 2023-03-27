using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManagementCore : MonoBehaviour
{
	[SerializeField] private UICoreInfoPopup coreInfoPopup;

	[Header("하이퍼모드정보")]
	[SerializeField] private TextMeshProUGUI userGradeText;
	[SerializeField] private TextMeshProUGUI hyperDescText;


	[Header("Point")]
	[SerializeField] private TextMeshProUGUI propertyPointText;

	[Header("리스트")]
	[SerializeField] private UIItemCore itemPrefab;
	[SerializeField] private Transform itemRoot;

	[Header("버튼")]
	[SerializeField] private Button upgradeButton;




	private void Awake()
	{
		upgradeButton.onClick.RemoveAllListeners();
		upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
	}

	public void OnUpdate(bool _refreshGrid)
	{
		userGradeText.text = $"{UserInfo.info.UserGrade.grade}";
		UpdateHyperText();
		UpdateItem(_refreshGrid);
		UpdateMoney();
	}

	private void UpdateHyperText()
	{
		string text = "";
		// 하이퍼모드 강화
		text += $"공격력: {UserInfo.info.UserGrade.hyperAttackPower}\n";
		text += $"공속: {UserInfo.info.UserGrade.hyperAttackSpeed}\n";
		text += $"이속: {UserInfo.info.UserGrade.hyperMoveSpeed}\n";
		text += $"치피: {UserInfo.info.UserGrade.hyperCriticalAttackPower}";

		hyperDescText.text = text;
	}

	public void UpdateItem(bool _refresh)
	{
		if (_refresh == false)
		{
			foreach (var v in itemRoot.GetComponentsInChildren<UIItemCore>())
			{
				Destroy(v.gameObject);
			}

			foreach (var v in DataManager.Get<ItemDataSheet>().GetByItemType(ItemType.Core))
			{
				UICoreData uiData = new UICoreData();
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

		foreach (var v in itemRoot.GetComponentsInChildren<UIItemCore>())
		{
			v.OnRefresh();
		}
	}

	public void UpdateMoney()
	{
		propertyPointText.text = Inventory.it.ItemCount("corepoint").ToString();
	}

	public void OnUpgradeButtonClick()
	{
		coreInfoPopup.gameObject.SetActive(true);
		coreInfoPopup.OnUpdate(this);
	}
}
