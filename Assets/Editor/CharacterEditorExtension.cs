using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterEditorExtension : EditorWindow
{
	private Character character;

	private static bool conditionFoldout;

	private static StunConditionData stunConditionData = new StunConditionData();
	private static DoteConditionData doteConditionData = new DoteConditionData(ElementType.FIRE, 0.5f);
	private static MoveSpeedUpConditionData moveSpeedUpConditionData = new MoveSpeedUpConditionData(0.21f);
	private static MoveSpeedDownConditionData moveSpeedDownConditionData = new MoveSpeedDownConditionData(0.21f);
	private static KnockbackConditionData knockbackConditionData = new KnockbackConditionData();
	private static DamageUpConditionData damageUpConditionData = new DamageUpConditionData(0.5f);
	private static DamageDownConditionData damageDownConditionData = new DamageDownConditionData(0.5f);
	private static CriticalChanceUpConditionData criticalChanceUpConditionData = new CriticalChanceUpConditionData(0.035f);
	private static CriticalChanceDownConditionData criticalChanceDownConditionData = new CriticalChanceDownConditionData(0.035f);
	private static AttackSpeedUpConditionData attackSpeedUpConditionData = new AttackSpeedUpConditionData(0.14f);
	private static AttackSpeedDownConditionData attackSpeedDownConditionData = new AttackSpeedDownConditionData(0.14f);
	private static AttackPowerUpConditionData attackPowerUpConditionData = new AttackPowerUpConditionData(0.084f);
	private static AttackPowerDownConditionData attackPowerDownConditionData = new AttackPowerDownConditionData(0.084f);



	private GUIStyle labelStyle
	{
		get
		{
			GUIStyle style = new GUIStyle("Label");
			style.richText = true;
			style.wordWrap = true;

			return style;
		}
	}

	public static void ShowEditor(Character _target)
	{
		CharacterEditorExtension window = ScriptableObject.CreateInstance<CharacterEditorExtension>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();

		window.character = _target;
	}

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			GUILayout.Label("플레이중이 아님");
			return;
		}

		character = (Character)EditorGUILayout.ObjectField(character, typeof(Character), true);


		try
		{
			EditorGUILayout.Space();
			ShowConditionTest(character);
			EditorGUILayout.Space();
			ShowCharInfo(character);
			EditorGUILayout.Space();
			ShowRecordData(character);
			EditorGUILayout.Space();
			ShowConditionList(character);
		}
		catch (Exception e)
		{
			GUILayout.Label("<color=red>Invalid target</color>", labelStyle);
			GUILayout.Label($"<color=red>{e.Message}</color>", labelStyle);
		}
	}

	private void Update()
	{
		Repaint();
	}

	public static void ShowCharInfo(Character _character)
	{
		EditorGUILayout.LabelField(new GUIContent($"{_character.info.charNameAndCharId}"), "PreToolbar");
		if (_character.skillModule.hasSkill)
		{
			EditorGUILayout.LabelField($"보유스킬: ({_character.skillModule.skillAttack.skillEditorLogTitle}) - {_character.skillModule.skillAttack.preset}");
		}
		else
		{
			EditorGUILayout.LabelField($"보유스킬: 없음");
		}
		EditorGUILayout.LabelField($"HP: {_character.info.data.hp.ToString()} / {_character.rawData.hp.ToString()}");
		EditorGUILayout.LabelField($"AttackPower: {_character.info.AttackPower(false).ToString()}");
		EditorGUILayout.LabelField($"DamageMul: {_character.info.DamageMul()}");
		EditorGUILayout.LabelField($"CriticalChance: {_character.info.CriticalChanceRatio()}");
		EditorGUILayout.LabelField($"CriticalDamageMul: {_character.info.CriticalDamageMultifly()}");
		EditorGUILayout.LabelField($"MoveSpeed: {_character.info.MoveSpeed()}");
	}

	public static void ShowRecordData(Character _character)
	{
		EditorGUILayout.LabelField(new GUIContent($"Record"), "PreToolbar");
		RecordData record = VGameManager.it.battleRecord.GetCharacterRecord(_character.charID);
		if (record == null)
		{
			EditorGUILayout.LabelField("Record is Null");
		}
		else
		{
			EditorGUILayout.LabelField(record.ToStringEditor());
		}
	}

	public static void ShowConditionList(Character _character)
	{
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션 어빌리티"), "PreToolbar");

		EditorGUILayout.LabelField($"[AttackPowerRatio] Up: {_character.conditionModule.ability.attackPowerUpRatio} / Down: {_character.conditionModule.ability.attackPowerDownRatio}");
		EditorGUILayout.LabelField($"[AttackSpeedRatio] Up: {_character.conditionModule.ability.attackSpeedUpRatio} / Down: {_character.conditionModule.ability.attackSpeedDownRatio}");
		EditorGUILayout.LabelField($"[CriticalChanceRatio] Up: {_character.conditionModule.ability.criticalChanceUpRatio} / Down: {_character.conditionModule.ability.criticalChanceDownRatio}");
		EditorGUILayout.LabelField($"[DamageRatio] Up: {_character.conditionModule.ability.damageUpRatio} / Down: {_character.conditionModule.ability.damageDownRatio}");
		EditorGUILayout.LabelField($"[MoveSpeedRatio] Up: {_character.conditionModule.ability.moveSpeedUpRatio} / Down: {_character.conditionModule.ability.moveSpeedDownRatio}");

		EditorGUILayout.Space();
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션"), "PreToolbar");
		foreach (var condition in _character.conditionModule.conditions)
		{
			EditorGUILayout.LabelField($"{condition.conditionType}, Duration: {condition.duration}");
		}
	}

	public static void ShowConditionTest(Character _character)
	{
		conditionFoldout = EditorGUILayout.Foldout(conditionFoldout, "상태 테스트");
		if (conditionFoldout == false)
		{
			return;
		}

		if (GUILayout.Button(typeof(StunCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new StunCondition(_character, stunConditionData));
		}
		if (GUILayout.Button(typeof(DoteCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new DoteCondition(_character, doteConditionData));
		}
		if (GUILayout.Button(typeof(MoveSpeedUpCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new MoveSpeedUpCondition(_character, moveSpeedUpConditionData));
		}
		if (GUILayout.Button(typeof(MoveSpeedDownCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new MoveSpeedDownCondition(_character, moveSpeedDownConditionData));
		}
		if (GUILayout.Button(typeof(KnockbackCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new KnockbackCondition(_character, knockbackConditionData));
		}
		if (GUILayout.Button(typeof(DamageUpCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new DamageUpCondition(_character, damageUpConditionData));
		}
		if (GUILayout.Button(typeof(DamageDownCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new DamageDownCondition(_character, damageDownConditionData));
		}
		if (GUILayout.Button(typeof(CriticalChanceUpCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new CriticalChanceUpCondition(_character, criticalChanceUpConditionData));
		}
		if (GUILayout.Button(typeof(CriticalChanceDownCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new CriticalChanceDownCondition(_character, criticalChanceDownConditionData));
		}
		if (GUILayout.Button(typeof(AttackSpeedUpCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new AttackSpeedUpCondition(_character, attackSpeedUpConditionData));
		}
		if (GUILayout.Button(typeof(AttackSpeedDownCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new AttackSpeedDownCondition(_character, attackSpeedDownConditionData));
		}
		if (GUILayout.Button(typeof(AttackPowerUpCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new AttackPowerUpCondition(_character, attackPowerUpConditionData));
		}
		if (GUILayout.Button(typeof(AttackPowerDownCondition).ToString()))
		{
			_character.conditionModule.AddCondition(new AttackPowerDownCondition(_character, attackPowerDownConditionData));
		}
	}
}
