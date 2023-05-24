using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupYouthOption : MonoBehaviour, IUIClosable
{
	[SerializeField] private Button buttonClose;
	[SerializeField] private Button buttonInfo;
	[SerializeField] private Button buttonChange;
	[SerializeField] private Button buttonAutoChange;

	[SerializeField] private UIPopupYouthOptionAutoRoll uiPopupAutoChange;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform contentRoot;

	private UIManagementYouth parent;
	private bool isAuto = false;

	public void AddCloseListener()
	{

	}

	public bool Closable()
	{

		return true;

	}

	public void Close()
	{
		uiPopupAutoChange.Close();
		gameObject.SetActive(false);
	}

	public void RemoveCloseListener()
	{

	}
	void Awake()
	{
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(Close);

		buttonInfo.onClick.RemoveAllListeners();
		buttonInfo.onClick.AddListener(OnClickInfo);
		buttonChange.onClick.RemoveAllListeners();
		buttonChange.onClick.AddListener(OnClickChangeOption);

		buttonAutoChange.onClick.RemoveAllListeners();
		buttonAutoChange.onClick.AddListener(OnClickShowAutoChangePopup);
	}

	public void OnClickShowAutoChangePopup()
	{

		uiPopupAutoChange.gameObject.SetActive(true);
		uiPopupAutoChange.OnUpdate(this);
	}
	public void OnClickChangeOption()
	{
		ChangeOption();
	}

	public bool ChangeOption()
	{
		///돈 없을때 체크
		bool isContinue = GameManager.UserDB.youthContainer.RollYouthOption();
		UpdateItem();

		if (isContinue == false)
		{
			return false;
		}
		return true;
	}

	public void OnClickInfo()
	{

	}

	public void OnUpdate(UIManagementYouth _parent)
	{
		parent = _parent;
		isAuto = false;
		UpdateItem();
	}

	public void UpdateItem()
	{
		var list = GameManager.UserDB.youthContainer.slots;
		int countForMake = list.Length - contentRoot.childCount;

		if (countForMake > 0)
		{
			for (int i = 0; i < countForMake; i++)
			{
				var item = Instantiate(itemPrefab, contentRoot);
			}
		}

		for (int i = 0; i < contentRoot.childCount; i++)
		{

			var child = contentRoot.GetChild(i);
			if (i > list.Length - 1)
			{
				child.gameObject.SetActive(false);
				continue;
			}

			child.gameObject.SetActive(true);
			UIItemYouthOption slot = child.GetComponent<UIItemYouthOption>();

			var info = list[i];
			slot.OnUpdate(this, info);
		}
	}

	public void SetAuto(bool isTrue)
	{
		isAuto = isTrue;



	}

	private void Update()
	{
		if (isAuto)
		{

			if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
			{
				isAuto = false;
				return;
			}
			if (ChangeOption() == false)
			{
				isAuto = false;
			}
		}
	}
}
