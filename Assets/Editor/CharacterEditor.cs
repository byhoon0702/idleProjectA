using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
	private Character character;


	private void OnEnable()
	{
		character = target as Character;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying == false)
		{
			return;
		}

		EditorGUILayout.Space();
		ShowRecordData();
		//ShowConditionTest(); // 필요하면 주석해제
		EditorGUILayout.Space();
		ShowConditionList();
	}

	private void ShowRecordData()
	{
		EditorGUILayout.LabelField(new GUIContent($"Record {character.info.charNameAndCharId}"), "PreToolbar");
		RecordData record = GameManager.it.battleRecord.GetCharacterRecord(character.charID);
		if (record == null)
		{
			EditorGUILayout.LabelField("Record is Null");
		}
		else
		{
			EditorGUILayout.LabelField(record.ToStringEditor());
		}
	}


	private void ShowConditionList()
	{
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션 어빌리티"), "PreToolbar");

		EditorGUILayout.LabelField($"[AttackPowerRatio] Up: {character.conditionModule.ability.attackPowerUpRatio} / Down: {character.conditionModule.ability.attackPowerDownRatio}");
		EditorGUILayout.LabelField($"[AttackSpeedRatio] Up: {character.conditionModule.ability.attackSpeedUpRatio} / Down: {character.conditionModule.ability.attackSpeedDownRatio}");
		EditorGUILayout.LabelField($"[CriticalChanceRatio] Up: {character.conditionModule.ability.criticalChanceUpRatio} / Down: {character.conditionModule.ability.criticalChanceDownRatio}");
		EditorGUILayout.LabelField($"[DamageRatio] Up: {character.conditionModule.ability.damageUpRatio} / Down: {character.conditionModule.ability.damageDownRatio}");
		EditorGUILayout.LabelField($"[MoveSpeedRatio] Up: {character.conditionModule.ability.moveSpeedUpRatio} / Down: {character.conditionModule.ability.moveSpeedDownRatio}");

		EditorGUILayout.Space();
		EditorGUILayout.LabelField(new GUIContent("적용중인 컨디션"), "PreToolbar");
		foreach (var condition in character.conditionModule.conditions)
		{
			EditorGUILayout.LabelField($"{condition.conditionType}, Duration: {condition.duration}");
		}
	}

	private void ShowConditionTest()
	{
		if (GUILayout.Button("Knockback"))
		{
			character.conditionModule.AddCondition(new KnockbackCondition(character, new KnockbackConditionData(0.5f, 5)));
		}
		if (GUILayout.Button("Stun"))
		{
			character.conditionModule.AddCondition(new StunCondition(character, new StunConditionData(5)));
		}
		if (GUILayout.Button("Damage Down"))
		{
			character.conditionModule.AddCondition(new DamageDownCondition(character, new DamageDownConditionData(0.5f, 5)));
		}
		if (GUILayout.Button("Poison"))
		{
			character.conditionModule.AddCondition(new PoisonCondition(character, new PoisonConditionData(0.5f, 5)));
		}
	}
}
