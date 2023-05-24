using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class JuvenescenceContainer : BaseContainer
{
	public List<RuntimeData.JuvenescenceInfo> juvenescenceInfos;

	public List<RuntimeData.HyperClassInfo> hyperClassInfos;

	public override void FromJson(string json)
	{
		JuvenescenceContainer temp = CreateInstance<JuvenescenceContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < juvenescenceInfos.Count; i++)
		{
			if (i < temp.juvenescenceInfos.Count)
			{
				juvenescenceInfos[i].Load(temp.juvenescenceInfos[i]);
			}
		}
		for (int i = 0; i < hyperClassInfos.Count; i++)
		{
			if (i < temp.hyperClassInfos.Count)
			{
				hyperClassInfos[i].Load(temp.hyperClassInfos[i]);
			}
		}
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();
		SetStatListRawData(ref juvenescenceInfos, DataManager.Get<JuvenescenceDataSheet>().GetInfosClone());
		SetStatListRawData(ref hyperClassInfos, DataManager.Get<HyperDataSheet>().GetInfosClone());
	}

	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}


}
