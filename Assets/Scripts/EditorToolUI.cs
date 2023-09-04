﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public enum EditorToolPage
{
	Animation = 0,
	Projectile = 1,

}

#if UNITY_EDITOR
public class EditorToolUI : MonoBehaviour
{
	[Header("==== Panels ====")]
	//public ProjectileEditorPanel projectileEditorPanel;
	//public AnimationEditorPanel animationEditorPanel;

	[Header("==== ====== ====")]
	[Space(1)]
	public TMP_Dropdown dropdown;
	public Toggle showDummyToggle;
	public Slider enemyCountSlider;
	public TextMeshProUGUI enemyCountSliderNumber;
	//public SkillObject projectileForEdit;

	public Toggle hyperMode;

	//private EditorUnit viewTarget;
	private UnityEditor.Animations.AnimatorController animatorController;
	private List<UnitData> unitdata;
	private List<string> animationList = new List<string>();
	private EditorToolPage currentPage = EditorToolPage.Animation;
	public void Init()
	{
		unitdata = DataManager.Get<UnitDataSheet>().GetInfosClone();

		List<string> names = new List<string>();
		for (int i = 0; i < unitdata.Count; i++)
		{
			names.Add($"{unitdata[i].tid}:{unitdata[i].name}");
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(names);

		//animationEditorPanel.Init(this);
		//projectileEditorPanel.Init(this);
	}

	//public void SetUnit(EditorUnit _unit, UnityEditor.Animations.AnimatorController _animatorController)
	//{
	//	viewTarget = _unit;
	//	animatorController = _animatorController;
	//	animationEditorPanel.SetUnit(viewTarget, animatorController);
	//	//projectileEditorPanel.SetUnit(viewTarget, animatorController);
	//}

	//public void ToggleTab(int index)
	//{
	//	currentPage = (EditorToolPage)index;

	//	bool isProjectile = currentPage == EditorToolPage.Projectile;
	//	CharacterEditor.it.SetPlayerPos(isProjectile);
	//	if (isProjectile == false)
	//	{
	//		ToggleShowDummy(showDummyToggle.isOn);
	//	}
	//}

	public void SummonEnemies()
	{


	}
	public void ClearEnemies()
	{


	}
	public void OnSliderValueChange(float value)
	{
		enemyCountSliderNumber.text = $"적 소환 수 : {(int)value}";
	}
	//public void ToggleShowDummy(bool isShow)
	//{
	//	CharacterEditor.it.ToggleShowDummy(isShow);
	//	if (currentPage == EditorToolPage.Projectile && isShow == false)
	//	{
	//		CharacterEditor.it.SetPlayerPos(true);
	//	}
	//}

	public void OnClickSpawnUnit()
	{
		//animationEditorPanel.animationSpeedSlider.value = 1;
		//animationEditorPanel.SetSliderValue(1);
		//animationEditorPanel.SetAttackTimeSliderValue(0.1f);


	}

	public void OnClickHyper(bool hyper)
	{
		//if (UnitEditor.it.viewPlayer == null)
		//{
		//	hyperMode.isOn = false;
		//	return;
		//}
		//if (hyper)
		//{
		//	UnitEditor.it.viewPlayer.ActivateHyperMode();
		//}
		//else
		//{
		//	UnitEditor.it.viewPlayer.DeactivateHyperMode();
		//}
	}


	public void OnClickSkillEditorWindow()
	{
	}
}
#endif
