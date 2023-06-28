using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class SkillGlobalUi : MonoBehaviour
{
	[SerializeField] private RectTransform skillButtonRoot;
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI buttonText;

	[SerializeField] private InteractableSkilIcon[] uiSkillIcons;

	private void Awake()
	{
		foreach (var icon in uiSkillIcons)
		{
			icon.OnUpdate(null);
		}
	}

	public void OnUpdate()
	{
		for (int i = 0; i < GameManager.UserDB.skillContainer.skillSlot.Length; i++)
		{
			uiSkillIcons[i].OnUpdate(GameManager.UserDB.skillContainer.skillSlot[i]);
		}
	}

	private void Update()
	{

	}
}
