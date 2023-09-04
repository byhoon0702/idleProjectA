using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffInfo : AffectedInfo
{
	public string key { get; private set; }
	public long tid;
	public float duration;
	public RuntimeData.AbilityInfo info;

	public DebuffInfo(LayerMask layerMask, long tid, float duration, RuntimeData.AbilityInfo ability) : base(layerMask)
	{
		this.tid = tid;
		this.duration = duration;
		info = ability.Clone();
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
