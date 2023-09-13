using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIAdBuffInfoObject : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textTime;
	[SerializeField] private StatsType _type;
	public void Init()
	{
		PlatformManager.UserDB.buffContainer.UpdateAdBuff += OnUpdate;
		gameObject.SetActive(false);
	}

	public void OnDestroy()
	{
		if (PlatformManager.Instance != null)
		{
			PlatformManager.UserDB.buffContainer.UpdateAdBuff -= OnUpdate;
		}
	}

	public void OnUpdate(RuntimeData.AdBuffInfo info)
	{
		if (_type != info.Ability.type)
		{
			return;
		}
		if (PlatformManager.UserDB.inventory.GetPersistent(InventoryContainer.AdFreeTid).unlock)
		{
			textTime.text = $"무제한";
			gameObject.SetActive(true);
			return;
		}

		System.TimeSpan ts = info.EndTime - TimeManager.Instance.UtcNow;
		if (ts.TotalSeconds <= 0)
		{
			gameObject.SetActive(false);
			return;
		}
		if (gameObject.activeInHierarchy == false)
		{
			gameObject.SetActive(true);
		}

		if (ts.Minutes > 0)
		{
			textTime.text = $"{(int)ts.TotalMinutes}M";
		}
		else
		{
			textTime.text = $"{(int)ts.TotalSeconds}S";
		}
	}
}
