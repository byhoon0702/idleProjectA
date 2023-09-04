using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Container/Gacha")]
public class GachaContainer : BaseContainer
{
	[SerializeField] private List<RuntimeData.GachaInfo> gachaInfos;
	public List<RuntimeData.GachaInfo> GachaInfos => gachaInfos;

	public int[] gachaStarChance = new int[] { 45, 27, 18, 10 };
	public override void Dispose()
	{

	}
	public override void FromJson(string json)
	{
		GachaContainer temp = CreateInstance<GachaContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref gachaInfos, temp.gachaInfos);
		//gachaStarChance = temp.gachaStarChance;
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetListRawData(ref gachaInfos, DataManager.Get<GachaDataSheet>().GetInfosClone());
	}

	public override void UpdateData()
	{
		for (int i = 0; i < gachaInfos.Count; i++)
		{
			gachaInfos[i].UpdateData();
		}
	}
	public override void DailyResetData()
	{

	}
	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();

		var list = Resources.LoadAll<GachaObject>("RuntimeDatas/Gachas");
		AddDictionary(scriptableDictionary, list);
	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}


}
