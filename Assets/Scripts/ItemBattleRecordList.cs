using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBattleRecordList : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textCharacterName;
	[SerializeField] private TextMeshProUGUI textCharacterDamage;

	private Character character;
	private RecordData recordData;

	public void SetData(Character _character, RecordData _recordData)
	{
		character = _character;
		recordData = _recordData;

		textCharacterName.text = character.info.charName;
	}

	public void UpdateDamage(IdleNumber _totalDamage)
	{
		textCharacterDamage.text = $"{recordData.TotalDamage().ToString()} ({(recordData.TotalDamage() / _totalDamage).ToString()})";
	}
}
