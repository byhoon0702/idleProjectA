
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class ResultCodeData : BaseData
{
	public string key;
	public PopAlertType alertType;
	public string title;
	public string content;
	public string okText;
	public string cancelText;

	public ResultCodeData Clone()
	{
		ResultCodeData resultCodeData = new ResultCodeData();

		resultCodeData.key = key;
		resultCodeData.alertType = alertType;
		resultCodeData.title = title;
		resultCodeData.content = content;

		return resultCodeData;
	}
}

[Serializable]
public class ResultCodeDataSheet : DataSheetBase<ResultCodeData>
{

	public override ResultCodeData Get(string _key)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].key == _key)
			{
				return infos[i];
			}
		}

		return null;
	}
}
