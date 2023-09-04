using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealInfo : AffectedInfo
{
	public Unit healer;
	public IdleNumber healRecovery = new IdleNumber();

	public bool IsPlayerHeal => healer is PlayerUnit || healer is Pet;
	public Color color => Color.green;


	public HealInfo(LayerMask layerMask, Unit _healer, IdleNumber _healRecovery) : base(layerMask)
	{
		healer = _healer;
		healRecovery = _healRecovery;


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
