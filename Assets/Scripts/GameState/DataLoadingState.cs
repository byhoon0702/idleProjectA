using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadingState : RootState
{
	public override void OnEnter()
	{
		DataManager.it.LoadAllJson();
		LoadConfig();
		LoadSkillMeta();
		UnitGlobal.it.skillModule.InitSkill(UserInfo.skillSlots);
		VGameManager.it.mapController.Reset();
		GameUIManager.it.mainUIObject.SetActive(true);
		GameUIManager.it.ReleaseAllPool();
		elapsedTime = 0;

		UserInfo.LoadUserData();


		var instantItems = new List<InstantItem>();
		var goldData = DataManager.it.Get<ItemDataSheet>().GetByHashTag("gold");
		var goldItem = ItemCreator.MakeInstantItem(goldData);
		goldItem.count = new IdleNumber(9000);
		instantItems.Add(goldItem);

		var diaData = DataManager.it.Get<ItemDataSheet>().GetByHashTag("dia");
		var diaItem = ItemCreator.MakeInstantItem(diaData);
		diaItem.count = new IdleNumber(3000);
		instantItems.Add(diaItem);

		var charData = DataManager.it.Get<ItemDataSheet>().GetByHashTag("defaultunit");
		var charItem = ItemCreator.MakeInstantItem(charData);
		charItem.count = new IdleNumber(1);
		instantItems.Add(charItem);

		//var heartData = DataManager.it.Get<ItemDataSheet>().Get(5);
		//var heartItem = ItemCreator.MakeInstantItem(heartData);
		//heartItem.count = new IdleNumber(5);
		//instantItems.Add(heartItem);

		//var bossData = DataManager.it.Get<ItemDataSheet>().Get(6);
		//var bossItem = ItemCreator.MakeInstantItem(bossData);
		//bossItem.count = new IdleNumber(2);
		//instantItems.Add(bossItem);

		var unitLvMoney = DataManager.it.Get<ItemDataSheet>().Get(8);
		var unitLvItem = ItemCreator.MakeInstantItem(unitLvMoney);
		unitLvItem.count = new IdleNumber(100);
		instantItems.Add(unitLvItem);

		var defaultWpData = DataManager.it.Get<ItemDataSheet>().Get(9);
		var defauitititem = ItemCreator.MakeInstantItem(defaultWpData);
		defauitititem.count = new IdleNumber(1);
		instantItems.Add(defauitititem);

		var rareWpData = DataManager.it.Get<ItemDataSheet>().Get(10);
		var rareititem = ItemCreator.MakeInstantItem(rareWpData);
		rareititem.count = new IdleNumber(1);
		instantItems.Add(rareititem);

		// 돌던지기
		var skData = DataManager.it.Get<ItemDataSheet>().Get(15);
		var skitem = ItemCreator.MakeInstantItem(skData);
		skitem.count = new IdleNumber(1);
		instantItems.Add(skitem);


		// 공격유물
		var atkData = DataManager.it.Get<ItemDataSheet>().Get(16);
		var atkitem = ItemCreator.MakeInstantItem(atkData);
		atkitem.count = new IdleNumber(0);
		instantItems.Add(atkitem);

		// 체력유물
		var hpData = DataManager.it.Get<ItemDataSheet>().Get(17);
		var hpitem = ItemCreator.MakeInstantItem(hpData);
		hpitem.count = new IdleNumber(0);
		instantItems.Add(hpitem);



		Inventory.it.Initialize(instantItems);
		UIController.it.Init();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.LOADING);
		}
	}

	private void LoadConfig()
	{
		TextAsset textAsset = Resources.Load<TextAsset>($"Json/{ConfigMeta.fileName.Replace(".json", "")}");

		if (textAsset == null)
		{
			VLog.LogError("Config 로드 실패");
			return;
		}

		JsonUtility.FromJsonOverwrite(textAsset.text, VGameManager.it.config);
	}

	private void LoadSkillMeta()
	{
		VGameManager.it.skillMeta = new SkillMeta();
		VGameManager.it.skillMeta.LoadData();
	}
}
