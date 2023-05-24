using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupYouthOptionAutoRoll : MonoBehaviour, IUIClosable
{
	[SerializeField] private Button buttonClose;

	[SerializeField] private Button buttonChange;

	[SerializeField] private Transform contentRoot;

	[SerializeField] private Toggle toggleSS;
	[SerializeField] private Toggle toggleSSS;

	private UIPopupYouthOption parent;
	public void AddCloseListener()
	{

	}

	public bool Closable()
	{
		return true;

	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void RemoveCloseListener()
	{

	}
	void Awake()
	{
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(Close);

		buttonChange.onClick.RemoveAllListeners();
		buttonChange.onClick.AddListener(OnClickChangeOption);
	}

	public void OnUpdate(UIPopupYouthOption _parent)
	{
		parent = _parent;

		toggleSS.isOn = GameManager.UserDB.youthContainer.toggleOptionSS;
		toggleSSS.isOn = GameManager.UserDB.youthContainer.toggleOptionSSS;
	}
	public void OnClickChangeOption()
	{
		Close();
		parent.SetAuto(true);
	}

	public void OnToggleSS(bool toggle)
	{
		GameManager.UserDB.youthContainer.toggleOptionSS = toggle;
	}

	public void OnToggleSSS(bool toggle)
	{
		GameManager.UserDB.youthContainer.toggleOptionSSS = toggle;
	}
}
