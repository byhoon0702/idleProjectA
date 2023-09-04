using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppliedBuff
{
	public string key { get; private set; }
	public long tid { get; private set; }
	public float duration { get; private set; }
	public RuntimeData.AbilityInfo ability { get; private set; }

	public bool isFinish { get; private set; }


	public AppliedBuff(BuffInfo buff) : this(buff.tid, buff.duration, buff.info, buff.key)
	{

	}
	public AppliedBuff(DebuffInfo buff) : this(buff.tid, buff.duration, buff.info, buff.key)
	{

	}

	public AppliedBuff(long tid, float duration, RuntimeData.AbilityInfo ability, string key)
	{
		this.tid = tid;
		this.duration = duration;
		this.ability = ability;
		this.key = key;
		isFinish = false;
	}

	public void AddTime(float duration)
	{
		this.duration = duration;
		isFinish = false;
	}

	public void TimeSpan(float time)
	{
		duration -= time;
		if (duration <= 0)
		{
			isFinish = true;
		}

	}
}

public class BuffModule : MonoBehaviour
{
	public List<AppliedBuff> buffList = new List<AppliedBuff>();
	public List<AppliedBuff> debuffList = new List<AppliedBuff>();

	[SerializeField] private Unit owner;
	public void Init(Unit _owner)
	{
		owner = _owner;
	}

	public void AddBuff(BuffInfo info)
	{
		var obj = buffList.Find(x => x.tid == info.tid);
		if (obj == null)
		{
			var buff = new AppliedBuff(info);
			buffList.Add(new AppliedBuff(info));
			owner.AddBuff(buff);
		}
		else
		{
			obj.AddTime(info.duration);
		}
	}

	public void AddDebuff(DebuffInfo info)
	{
		var obj = debuffList.Find(x => x.tid == info.tid);
		if (obj == null)
		{
			var buff = new AppliedBuff(info);
			debuffList.Add(buff);
			owner.AddDebuff(buff);
		}
		else
		{
			obj.AddTime(info.duration);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (buffList.Count > 0)
		{
			for (int i = buffList.Count - 1; i >= 0; i--)
			{
				var debuff = buffList[i];
				debuff.TimeSpan(Time.deltaTime);
				if (debuff.isFinish)
				{
					owner.RemoveBuff(debuff);
					buffList.Remove(debuff);
				}
			}
		}

		if (debuffList.Count > 0)
		{
			for (int i = debuffList.Count - 1; i >= 0; i--)
			{
				var debuff = debuffList[i];
				debuff.TimeSpan(Time.deltaTime);
				if (debuff.isFinish)
				{
					owner.RemoveBuff(debuff);
					debuffList.Remove(debuff);
				}
			}
		}
	}
}
