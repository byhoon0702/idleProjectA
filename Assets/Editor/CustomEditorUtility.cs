using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CustomEditorUtility
{
	[MenuItem("Custom Menu/로컬 세이브 삭제")]
	public static void DeleteSave()
	{
		PlayerPrefs.DeleteKey(UserDBSave.k_LocalSave);
		PlayerPrefs.DeleteKey("Login");
		PlayerPrefs.DeleteKey(UIPopupAgreement.k_Terms);
	}
}

