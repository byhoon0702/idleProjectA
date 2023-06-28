﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RuntimeData
{
	[System.Serializable]
	public class AdvancementInfo : BaseInfo
	{
		[SerializeField] private int level;
		public int Level => level;
		[SerializeField] private int costumeIndex;
		public int CostumeIndex => costumeIndex;

		public string Resource
		{
			get
			{
				if (advancementInfo == null)
				{
					return rawData.resource;
				}
				return advancementInfo.resource;
			}

		}

		public UnitAdvancementInfo advancementInfo { get; private set; }
		public UnitData rawData { get; private set; }
		public override void Load<T>(T info)
		{
			AdvancementInfo _info = info as AdvancementInfo;
			level = _info.level;
			costumeIndex = _info.costumeIndex;
			UpdateInfo();
		}
		public override void SetRawData<T>(T data) where T : class
		{
			rawData = data as UnitData;
			UpdateInfo();
		}

		public void UpdateInfo(Unit owner = null)
		{
			int level = this.level;
			advancementInfo = rawData.upgradeInfoList.Find(x => x.level == level);


			GameManager.UserDB.advancementContainer.RemoveModifiers(GameManager.UserDB.UserStats);
			GameManager.UserDB.advancementContainer.AddModifiers(GameManager.UserDB.UserStats);
		}

		public void LevelUp(Unit owner, UnitAdvancementInfo _info)
		{
			level = _info.level;

			UpdateInfo(owner);
		}

		public void ChangeCostume(Unit owner, UnitAdvancementInfo _info)
		{
			costumeIndex = _info.level;
			if (owner != null && owner is PlayerUnit)
			{
				var player = (owner as PlayerUnit);
				player.ChangeNormalUnit(this);
			}

		}

	}
}
