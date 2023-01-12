using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleRecord : MonoBehaviour
{
	[SerializeField] private ItemBattleRecordList itemBattleRecord;
	[SerializeField] private Transform listItemParent;

	List<RecordData> recordList = new List<RecordData>();
	List<ItemBattleRecordList> itemBattleRecordList = new List<ItemBattleRecordList>();

	public void Ready()
	{
		foreach (Transform item in listItemParent)
		{
			Destroy(item.gameObject);
		}

		recordList.Clear();
		recordList = new List<RecordData>();

		itemBattleRecordList.Clear();
		itemBattleRecordList = new List<ItemBattleRecordList>();

		var playerCharacterList = CharacterManager.it.GetPlayerCharacters(true);

		foreach (var character in playerCharacterList)
		{
			var record = VGameManager.it.battleRecord.GetCharacterRecord(character.charID);
			recordList.Add(record);

			var item = Instantiate(itemBattleRecord, listItemParent);
			item.SetData(character, record);
			itemBattleRecordList.Add(item);
		}
	}

	public void UpdateDamage()
	{
		IdleNumber totalDamage = new IdleNumber(0, 0);

		for (int i = 0; i < recordList.Count; i++)
		{
			var record = recordList[i];
			IdleNumber characterDamage = record.TotalDamage();
			totalDamage += characterDamage;
		}

		for (int i = 0; i < itemBattleRecordList.Count; i++)
		{
			var item = itemBattleRecordList[i];
			item.UpdateDamage(totalDamage);
		}
	}
}
