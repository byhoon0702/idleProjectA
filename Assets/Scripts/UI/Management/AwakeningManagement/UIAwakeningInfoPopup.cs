using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAwakeningInfoPopup : UIBase
{
	[SerializeField] private Button bg;
	[SerializeField] private Button _buttonClose;
	[SerializeField] private Transform _content;
	[SerializeField] private GameObject _prefab;

	protected override void OnEnable()
	{
		base.OnEnable();
		bg.SetButtonEvent(Close);
	}
	public void OnUpdate(List<RuntimeData.AbilityInfo> abilityInfos)
	{
		_content.CreateListCell(abilityInfos.Count, _prefab);

		for (int i = 0; i < _content.childCount; i++)
		{
			var child = _content.GetChild(i);
			child.gameObject.SetActive(false);
			if (i < abilityInfos.Count)
			{
				child.GetComponent<UIItemStats>().OnUpdate(abilityInfos[i]);
				child.gameObject.SetActive(true);
			}
		}
		bg.gameObject.SetActive(true);
	}

	protected override void OnClose()
	{
		base.OnClose();
		bg.gameObject.SetActive(false);
	}
}
