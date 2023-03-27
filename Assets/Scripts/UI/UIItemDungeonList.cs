using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemDungeonList : MonoBehaviour
{
	[SerializeField] private Image mainImage;
	[SerializeField] private GameObject consumeObj;
	[SerializeField] private Image itemIconImage;
	[SerializeField] private TextMeshProUGUI itemCount;
	[SerializeField] private Button enterButton;
	[SerializeField] private TextMeshProUGUI dungeonName;

	private UIDungeonList owner;
	private DungeonInfoData dungeonInfo;




	private void Awake()
	{
		enterButton.onClick.RemoveAllListeners();
		enterButton.onClick.AddListener(OnEnterButtonClick);
	}

	public void SetData(UIDungeonList _owner, DungeonInfoData _data)
	{
		owner = _owner;
		dungeonInfo = _data;

		if (_data.itemTidNeed != 0)
		{
			consumeObj.SetActive(true);
			var itemInfo = DataManager.GetFromAll<ItemData>(_data.itemTidNeed);

			itemIconImage.sprite = Resources.Load<Sprite>($"Icon/{itemInfo.Icon}");
			itemCount.text = $"X{_data.itemCount}";
		}
		else
		{
			consumeObj.SetActive(false);
		}

		dungeonName.text = _data.name;

		var keyItem = Inventory.it.FindItemByTid(_data.itemTidNeed);

		if (keyItem == null)
		{
			// disableButton;
			return;
		}

		if (Inventory.it.FindItemByTid(_data.itemTidNeed).Count > _data.itemCount)
		{
			// enable Button
		}
		else
		{
			// disable Button
		}
	}

	public void OnRefresh()
	{

	}

	private void OnEnterButtonClick()
	{
		owner.ShowDetail(dungeonInfo.waveType);
	}
}
