using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePetSkilIcon : InteractableSkilIcon
{
	[SerializeField] int index;
	[SerializeField] GameObject objEmpty;
	private Unit caster;
	public override void OnUpdate(SkillSlot _skillSlot = null)
	{
		objEmpty.SetActive(true);
		base.OnUpdate(_skillSlot);

		if (_skillSlot == null)
		{

			return;
		}
		objEmpty.SetActive(_skillSlot.item == null);
	}

	protected override void OnClickSkillSlot()
	{
		if (skillSlot == null)
		{
			GameUIManager.it.uiController.TogglePet(() => { GameUIManager.it.uiController.BottomMenu.TogglePet.isOn = false; });

			return;
		}
	}
}
