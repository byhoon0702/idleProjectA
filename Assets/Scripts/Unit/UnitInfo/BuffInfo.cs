using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo : AffectedInfo
{
	public string key { get; private set; }
	public long tid;
	public float duration;
	public RuntimeData.AbilityInfo info;

	public BuffInfo(LayerMask layerMask, long tid, float duration, RuntimeData.AbilityInfo ability) : base(layerMask)
	{
		this.tid = tid;
		this.duration = duration;
		info = ability.Clone();
		key = $"buff_{tid}";

		if (LayerMask == LayerMask.NameToLayer("Enemy"))
		{
			targetLayer = LayerMask.NameToLayer("Enemy");
		}
		else
		{
			targetLayer = LayerMask.NameToLayer("Player");
		}

	}

}
