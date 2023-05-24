using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class ItemUIBase : MonoBehaviour
{
	[SerializeField] protected Image icon;
	[SerializeField] protected Image bg;
	[SerializeField] protected TextMeshProUGUI levelText;
	[SerializeField] protected TextMeshProUGUI countText;

	[SerializeField] protected GameObject frameObject;

	[SerializeField] protected Button button;
	protected Action onClick;

	public abstract void OnUpdate(RuntimeData.IDataInfo _info, Action _onClick = null, ISelectListener _selectListener = null);

}
