using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class SkillGlobalUi : MonoBehaviour
{
	[SerializeField] private RectTransform skillButtonRoot;
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI buttonText;

	[SerializeField] private InteractableSkilIcon[] uiSkillIcons;
	[SerializeField] private InteractableSkilIcon[] petSkillIcons;



	private void Awake()
	{
		foreach (var icon in uiSkillIcons)
		{
			icon.OnUpdate(null);
		}
		foreach (var icon in petSkillIcons)
		{
			icon.OnUpdate(null);
		}
	}

	public void OnUpdate()
	{
		for (int i = 0; i < PlatformManager.UserDB.skillContainer.skillSlot.Length; i++)
		{
			uiSkillIcons[i].OnUpdate(PlatformManager.UserDB.skillContainer.skillSlot[i]);
		}
		for (int i = 0; i < PlatformManager.UserDB.skillContainer.petSkillSlot.Length; i++)
		{
			petSkillIcons[i].OnUpdate(PlatformManager.UserDB.skillContainer.petSkillSlot[i]);
		}

	}

}
