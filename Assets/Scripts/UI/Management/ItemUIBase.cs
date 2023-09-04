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
	[SerializeField] protected Image imageFrame;

	[SerializeField] protected TextMeshProUGUI textInfo;


	[SerializeField] protected Button button;
	protected Action onClick;

	public abstract void OnUpdate(RuntimeData.IDataInfo _info, Action _onClick = null, ISelectListener _selectListener = null);

}
