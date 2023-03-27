using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemCoreAbilitySlot : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI gradeText;
	[SerializeField] private TextMeshProUGUI abilityText;

	[Header("잠금")]
	[SerializeField] private GameObject lockObject;
	[SerializeField] private Button lockButton;

	[Header("진급레벨 미달")]
	[SerializeField] private GameObject noPossessedObject;
	[SerializeField] private TextMeshProUGUI noPossessedText;


	private UIManagementCoreAbility owner;
	public Grade SlotGrade { get; private set; }
	public bool IsLock => lockObject.activeSelf;
	public bool NoPossessed { get; private set; }



	private void Awake()
	{
		lockButton.onClick.RemoveAllListeners();
		lockButton.onClick.AddListener(OnLockButtonClick);
	}

	public void OnUpdate(UIManagementCoreAbility _owner, Grade _grade)
	{
		owner = _owner;
		SlotGrade = _grade;

		NoPossessed = UserInfo.info.UserGrade.grade < SlotGrade;
		if (NoPossessed)
		{
			noPossessedObject.SetActive(true);
			noPossessedText.text = $"{SlotGrade} 등급 개방";
		}
		else
		{
			noPossessedObject.SetActive(false);
		}


		var abil = UserInfo.coreAbil.GetAbility(SlotGrade);

		if (abil != null)
		{
			gradeText.text = $"{UserInfo.coreAbil.GetAbilityGrade(abil.type, abil.GetValue(1))}";
			abilityText.text = $"{abil.type.ToUIString()}, {abil.GetValue(1).ToString()}";
		}
		else
		{
			gradeText.text = $"";
			abilityText.text = $"없음";
		}
	}

	public void SetLock(bool _isLock)
	{
		lockObject.SetActive(_isLock);
		UserInfo.coreAbil.SetAbilityLock(SlotGrade, _isLock);
		UserInfo.SaveUserData();
		owner.UpdateButton();
	}

	private void OnLockButtonClick()
	{
		SetLock(IsLock == false);
	}
}
