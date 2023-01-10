using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PresetDataSheet))]
public class PresetDataSheetEditor : Editor
{
	PresetDataSheet dataSheet;
	SerializedProperty characterDataSheet;

	static int selectvalue;

	bool playerFoldout;
	bool enemyFoldout;

	private void OnEnable()
	{
		dataSheet = target as PresetDataSheet;

		characterDataSheet = serializedObject.FindProperty("characterDataSheet");
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("데이터");
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(characterDataSheet);

		playerFoldout = EditorGUILayout.Foldout(playerFoldout, "플레이어", true);
		if (playerFoldout)
		{
			for (int i = 0; i < dataSheet.playerPartyPresetData.Count; i++)
			{
				ShowElement(dataSheet.playerPartyPresetData[i], i, false);
			}

			EditorGUILayout.Space(10);
			if (GUILayout.Button("파티 추가"))
			{
				dataSheet.AddPlayerParty();
			}
			if (GUILayout.Button("파티 삭제"))
			{
				dataSheet.RemovePlayerParty();
			}

		}
		EditorGUILayout.Space(20);
		enemyFoldout = EditorGUILayout.Foldout(enemyFoldout, "적", true);
		if (enemyFoldout)
		{
			for (int i = 0; i < dataSheet.enemypartyPresetDatas.Count; i++)
			{
				ShowElement(dataSheet.enemypartyPresetDatas[i], i, true);
			}
			EditorGUILayout.Space(10);
			if (GUILayout.Button("파티 추가"))
			{
				dataSheet.AddEnemyParty();
			}
			if (GUILayout.Button("파티 삭제"))
			{
				dataSheet.RemoveEnemyParty();
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(dataSheet);
		}
		serializedObject.ApplyModifiedProperties();
	}

	void ShowElement(PartyData partyList, int index, bool enemy)
	{
		if (partyList == null)
		{
			return;
		}

		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("파티");
		List<int> idList = new List<int>();
		List<string> nameList = new List<string>();

		for (int i = 0; i < dataSheet.characterDataSheet.dataSheet.infos.Count; i++)
		{
			var playerdata = dataSheet.characterDataSheet.dataSheet.infos[i];
			idList.Add((int)playerdata.tid);
			nameList.Add($"{playerdata.tid} : {playerdata.name}");
		}

		var currentpreset = partyList;
		for (int i = 0; i < currentpreset.partySlots.Count; i++)
		{
			EditorGUI.indentLevel++;
			currentpreset.partySlots[i].raceType = (RaceType)EditorGUILayout.EnumPopup("종족", currentpreset.partySlots[i].raceType);
			currentpreset.partySlots[i].characterTid = (long)EditorGUILayout.IntPopup("캐릭터 TID", (int)currentpreset.partySlots[i].characterTid, nameList.ToArray(), idList.ToArray()); ;
			currentpreset.partySlots[i].level = EditorGUILayout.IntField("슬롯 레벨", currentpreset.partySlots[i].level);
			currentpreset.partySlots[i].coord = EditorGUILayout.Vector2IntField("그리드", currentpreset.partySlots[i].coord);
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("파티원 추가"))
		{
			currentpreset.AddPartyslot();
		}
		if (GUILayout.Button("파티원 삭제"))
		{
			currentpreset.RemovePartySlot();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
	}
}
