using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffInfo : AffectedInfo
{
	public string key { get; private set; }
	public long tid;
	public float duration;
	public StatsType type;
	public IdleNumber power;

	public DebuffInfo(LayerMask layerMask, long tid, float duration, StatsType type, IdleNumber power) : base(layerMask)
	{
		this.tid = tid;
		this.duration = duration;
		this.type = type;
		this.power = power;
		key = $"debuff_{tid}";


		if (LayerMask == LayerMask.NameToLayer("Enemy"))
		{
			targetLayer = LayerMask.NameToLayer("Player");
		}
		else
		{
			targetLayer = LayerMask.NameToLayer("Enemy");
		}
	}
}
