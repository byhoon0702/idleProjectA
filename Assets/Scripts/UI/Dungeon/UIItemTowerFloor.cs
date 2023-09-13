using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemTowerFloor : MonoBehaviour
{
	[SerializeField] private Button buttonFloor;
	[SerializeField] private Image imageStge;
	[SerializeField] private TextMeshProUGUI textFloor;
	[SerializeField] private GameObject objClearMark;

	[SerializeField] private Sprite spriteSelected;
	[SerializeField] private Sprite spriteNotSelected;
	[SerializeField] private Sprite spriteComplete;

	private RuntimeData.StageInfo info;
	private UIPageBattleTower selectListener;

	//private void OnEnable()
	//{
	//	selectListener?.AddSelectListener(OnSelect);
	//}

	//private void OnDisable()
	//{
	//	selectListener?.RemoveSelectListener(OnSelect);
	//}

	private void Awake()
	{
		buttonFloor.onClick.RemoveAllListeners();
		buttonFloor.onClick.AddListener(OnClickFloor);
	}

	public void OnUpdate(UIPageBattleTower _selectListener, RuntimeData.StageInfo _info)
	{
		info = _info;
		selectListener = _selectListener;
		textFloor.text = $"{info.StageNumber}층";
		objClearMark.SetActive(info.isClear);
	}

	public void OnClickFloor()
	{
		if (info.isClear)
		{
			return;
		}

		selectListener?.SetSelectedTid(info.StageNumber);
	}

	public void OnSelect(long stageNumber)
	{
		if (info.StageNumber == stageNumber)
		{
			imageStge.sprite = spriteSelected;
		}
		else
		{
			if (info.isClear)
			{
				imageStge.sprite = spriteComplete;
			}
			else
			{
				imageStge.sprite = spriteNotSelected;
			}

		}
	}
}
