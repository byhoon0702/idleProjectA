using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InventoryEditor : EditorWindow
{
	public ItemType itemType;
	public long tid;
	public long count;
	public bool abilityFoldOut;
	public Vector2 scrollPos;


	[MenuItem("Custom Menu/Inventory")]
	public static void ShowEditor()
	{
		InventoryEditor window = GetWindow<InventoryEditor>();
		window.titleContent = new GUIContent(window.ToString());
		window.Show();
	}
	
	private void OnGUI()
	{
		if(Application.isPlaying == false)
		{
			return;
		}

		if(VGameManager.it.currentState <= GameState.DATALOADING)
		{
			return;
		}


		itemType = (ItemType)EditorGUILayout.EnumPopup(itemType);


		EditorGUILayout.BeginHorizontal();
		tid = EditorGUILayout.LongField("TID", tid);
		count = EditorGUILayout.LongField("Count", count);
		if(GUILayout.Button("Add", GUILayout.MaxWidth(40)))
		{
			Inventory.it.AddItem(tid, count);
		}
		EditorGUILayout.EndHorizontal();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		abilityFoldOut = EditorGUILayout.Foldout(abilityFoldOut, "Ability Info");
		if (abilityFoldOut)
		{
			GUILayout.Label("Total", "PreToolbar");
			foreach (var a in Inventory.it.abilityCalculator.abilityTotal)
			{
				GUILayout.Label($"[{a.Key}] {a.Value.ToString()}");
			}
		}
		
		

		GUILayout.Space(20);
		var infos = DataManager.it.Get<ItemDataSheet>().infos;
		if(itemType == ItemType.Money)
		{
			GUILayout.Label($"Refill Check: {ConfigMeta.it.REFILL_UPDATE_CYCLE - Inventory.it.refillCheckDeltaTime}");
		}

		foreach (var info in infos)
		{
			if (info.itemType != itemType)
			{
				continue;
			}

			var itembase = Inventory.it.FindItemByTid(info.tid);
			if (itembase == null)
			{
				continue;
			}

			GUILayout.Label(itembase.ToString());
			if (itembase is ItemMoney)
			{
				var itemMoney = itembase as ItemMoney;
				if (itemMoney.Refillable)
				{
					GUILayout.Label($"다음 리필: {itemMoney.refill.NextRefillDate - TimeManager.it.server_utc}");
				}
				if (itemMoney.Resetable)
				{
					GUILayout.Label($"다음 리셋: {itemMoney.reset.m_next_reset - TimeManager.it.server_utc}");
				}

			}

			EditorGUILayout.Space(10);
		}
		EditorGUILayout.EndScrollView();
	}

	private void Update()
	{
		Repaint();
	}
}
