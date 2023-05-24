using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VResultType
{
	Fail,
	Ok,
}

public class VResult
{
	public VResult Clone()
	{
		var result = new VResult();

		result.data = data;
		result.resultType = resultType;
		result.description = description;
		result.tidParams = tidParams;

		return result;
	}


	public ResultCodeData data;
	private VResultType resultType;
	public string description;
	private Int64[] tidParams;

	public string ContentsText
	{
		get
		{
			string[] stringParams = GetStringParams();

			if (stringParams.Length > 0)
			{
				try
				{
					return string.Format(data.content, stringParams);
				}
				catch
				{
					VLog.LogError($"Invalid Params. resultcode tid: {data.tid}");
					return data.content;
				}
			}
			else
			{
				return data.content;
			}
		}
	}

	public VResult()
	{
	}

	public VResult SetOk(VResultCode _resultCode = VResultCode._NONE, string _description = "", params Int64[] _tidParams)
	{
		data = DataManager.Get<ResultCodeDataSheet>().Get(_resultCode.ToString());
		resultType = VResultType.Ok;
		description = _description;
		tidParams = _tidParams;
		return this;
	}

	public VResult SetFail(VResultCode _resultCode, string _description = "", params Int64[] _tidParams)
	{
		data = DataManager.Get<ResultCodeDataSheet>().Get(_resultCode.ToString());
		resultType = VResultType.Fail;
		description = _description;
		tidParams = _tidParams;

		return this;
	}

	public bool Ok()
	{
		return resultType == VResultType.Ok;
	}

	public bool Fail()
	{
		return resultType == VResultType.Fail;
	}

	private string[] GetStringParams()
	{
		if (tidParams != null && tidParams.Length > 0)
		{
			string[] stringParams = new string[tidParams.Length];

			// 아이템 테이블에 있는지 검색
			//var itemDataSheet = DataManager.Get<ItemDataSheet>();

			//if (itemDataSheet == null)
			//{
			//	// 필요 데이터가 초기화전에 불린경우 예외처리 필요
			//	for (int i = 0; i < stringParams.Length; i++)
			//	{
			//		stringParams[i] = String.Empty;
			//	}
			//}
			//else
			//{
			//	for (Int32 i = 0; i < tidParams.Length; i++)
			//	{
			//		var itemData = itemDataSheet.Get(tidParams[i]);
			//		if (itemData != null)
			//		{
			//			stringParams[i] = itemData.name;
			//		}
			//		else
			//		{
			//			stringParams[i] = "";
			//		}
			//	}
			//}

			return stringParams;
		}
		else
		{
			return new string[0];
		}
	}

	public override string ToString()
	{
		return $"{data.key}({data.tid}) : {data.description}. / {description}";
	}
}
