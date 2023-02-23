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

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => { UnitGlobal.it.skillModule.auto = !UnitGlobal.it.skillModule.auto; });
	}

	public void OnUpdate()
	{
		for (int i = 0; i < UserInfo.skills.Length; i++)
		{
			long itemTid = UserInfo.skills[i];
			if (itemTid == 0)
			{
				uiSkillIcons[i].OnUpdate(null);
			}
			else
			{
				ItemData itemData = DataManager.Get<ItemDataSheet>().Get(itemTid);
				SkillBase skillBase = UnitGlobal.it.skillModule.FindSkillBase(itemData.skillTid);

				if (skillBase == null)
				{
					VLog.SkillLogError($"스킬모듈에 등록되지 않는 스킬정보를 초기화하려고 시도. skilltid:{itemData.skillTid}");
					uiSkillIcons[i].OnUpdate(null);
				}
				else
				{
					uiSkillIcons[i].OnUpdate(skillBase);
				}
			}
		}
	}

	private void Update()
	{
		buttonText.text = $"auto: {UnitGlobal.it.skillModule.auto}";
	}
}
