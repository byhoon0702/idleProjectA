using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class UIPopupLevelupBaseItem<T> : MonoBehaviour
{

	protected UIManagementEquip parent;
	protected T itemInfo;

	[SerializeField] protected Button buttonExit;
	[SerializeField] protected Button buttonUpgrade;

	[SerializeField] protected TextMeshProUGUI textMeshProName;
	[SerializeField] protected TextMeshProUGUI textEquipBuff;
	[SerializeField] protected TextMeshProUGUI textOwnedBuff;


	protected virtual void Awake()
	{
		buttonExit.onClick.RemoveAllListeners();
		buttonExit.onClick.AddListener(OnClose);

		buttonUpgrade.onClick.RemoveAllListeners();
		buttonUpgrade.onClick.AddListener(OnClickLevelUp);
	}

	public abstract void OnUpdate(UIManagementEquip parent, T info);
	public abstract void OnClickLevelUp();
	public void OnClose()
	{
		gameObject.SetActive(false);
	}
}
