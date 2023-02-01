using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class UnitEditorUtility
{
	private static bool conditionFoldout;

	private static MoveSpeedUpConditionData moveSpeedUpConditionData = new MoveSpeedUpConditionData(0.21f);
	private static MoveSpeedDownConditionData moveSpeedDownConditionData = new MoveSpeedDownConditionData(0.21f);
	private static KnockbackConditionData knockbackConditionData = new KnockbackConditionData();
	private static CriticalChanceUpConditionData criticalChanceUpConditionData = new CriticalChanceUpConditionData(0.035f);
	private static CriticalChanceDownConditionData criticalChanceDownConditionData = new CriticalChanceDownConditionData(0.035f);
	private static AttackSpeedUpConditionData attackSpeedUpConditionData = new AttackSpeedUpConditionData(0.14f);
	private static AttackSpeedDownConditionData attackSpeedDownConditionData = new AttackSpeedDownConditionData(0.14f);
	private static AttackPowerUpConditionData attackPowerUpConditionData = new AttackPowerUpConditionData(0.084f);
	private static AttackPowerDownConditionData attackPowerDownConditionData = new AttackPowerDownConditionData(0.084f);


	public static void ShowCharacterInfo(UnitInfo _info, IdleNumber _hp, IdleNumber _maxHp)
	{
		EditorGUILayout.LabelField($"HP: {_hp.ToString()} / {_maxHp.ToString()}");
		EditorGUILayout.LabelField($"AttackPower: {_info.AttackPower(false).ToString()}");
		EditorGUILayout.LabelField($"CriticalChance: {_info.CriticalChanceRatio()}");
		EditorGUILayout.LabelField($"CriticalDamageMul: {_info.CriticalDamageMultifly()}");
		EditorGUILayout.LabelField($"MoveSpeed: {_info.MoveSpeed()}");
	}

	public static void ShowConditionList(ConditionModule _module)
	{
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션 어빌리티"), "PreToolbar");

		EditorGUILayout.LabelField($"[AttackPowerRatio] Up: {_module.ability.attackPowerUpRatio} / Down: {_module.ability.attackPowerDownRatio}");
		EditorGUILayout.LabelField($"[AttackSpeedRatio] Up: {_module.ability.attackSpeedUpRatio} / Down: {_module.ability.attackSpeedDownRatio}");
		EditorGUILayout.LabelField($"[CriticalChanceRatio] Up: {_module.ability.criticalChanceUpRatio} / Down: {_module.ability.criticalChanceDownRatio}");
		EditorGUILayout.LabelField($"[MoveSpeedRatio] Up: {_module.ability.moveSpeedUpRatio} / Down: {_module.ability.moveSpeedDownRatio}");

		EditorGUILayout.Space();
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션"), "PreToolbar");
		foreach (var condition in _module.conditions)
		{
			EditorGUILayout.LabelField($"{condition.conditionType}, Duration: {condition.duration}");
		}
	}

	public static void ShowHyperInfo(UnitBase _unit, HyperModule _module)
	{
		EditorGUILayout.LabelField(new GUIContent("적용중인 하이퍼 모드"), "PreToolbar");

		EditorGUILayout.LabelField($"AttackPower: {_module.GetHyperAbility(_unit, AbilityType.AttackPower)}");
		EditorGUILayout.LabelField($"MoveSpeed: {_module.GetHyperAbility(_unit, AbilityType.MoveSpeed)}");
		EditorGUILayout.LabelField($"AttackSpeed: {_module.GetHyperAbility(_unit, AbilityType.AttackSpeed)}");
		EditorGUILayout.LabelField($"CriticalAttackPower: {_module.GetHyperAbility(_unit, AbilityType.CriticalAttackPower)}");
		EditorGUILayout.LabelField($"SkillAttackPower: {_module.GetHyperAbility(_unit, AbilityType.SkillAttackPower)}");
		EditorGUILayout.LabelField($"BossAttackPower: {_module.GetHyperAbility(_unit, AbilityType.BossAttackPower)}");
	}

	public static void ShowConditionTest(UnitBase _attacker, ConditionModule _module)
	{
		conditionFoldout = EditorGUILayout.Foldout(conditionFoldout, "상태 테스트");
		if (conditionFoldout == false)
		{
			return;
		}

		if (GUILayout.Button(typeof(MoveSpeedUpCondition).ToString()))
		{
			_module.AddCondition(new MoveSpeedUpCondition(_attacker, moveSpeedUpConditionData));
		}
		if (GUILayout.Button(typeof(MoveSpeedDownCondition).ToString()))
		{
			_module.AddCondition(new MoveSpeedDownCondition(_attacker, moveSpeedDownConditionData));
		}
		if (GUILayout.Button(typeof(KnockbackCondition).ToString()))
		{
			_module.AddCondition(new KnockbackCondition(_attacker, knockbackConditionData));
		}
		if (GUILayout.Button(typeof(CriticalChanceUpCondition).ToString()))
		{
			_module.AddCondition(new CriticalChanceUpCondition(_attacker, criticalChanceUpConditionData));
		}
		if (GUILayout.Button(typeof(CriticalChanceDownCondition).ToString()))
		{
			_module.AddCondition(new CriticalChanceDownCondition(_attacker, criticalChanceDownConditionData));
		}
		if (GUILayout.Button(typeof(AttackSpeedUpCondition).ToString()))
		{
			_module.AddCondition(new AttackSpeedUpCondition(_attacker, attackSpeedUpConditionData));
		}
		if (GUILayout.Button(typeof(AttackSpeedDownCondition).ToString()))
		{
			_module.AddCondition(new AttackSpeedDownCondition(_attacker, attackSpeedDownConditionData));
		}
		if (GUILayout.Button(typeof(AttackPowerUpCondition).ToString()))
		{
			_module.AddCondition(new AttackPowerUpCondition(_attacker, attackPowerUpConditionData));
		}
		if (GUILayout.Button(typeof(AttackPowerDownCondition).ToString()))
		{
			_module.AddCondition(new AttackPowerDownCondition(_attacker, attackPowerDownConditionData));
		}
	}
}
