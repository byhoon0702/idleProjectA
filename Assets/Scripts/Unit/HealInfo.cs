using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealInfo : AffectedInfo
{
	public AttackerType healer;
	public IdleNumber healRecovery = new IdleNumber();

	public bool IsPlayerHeal => healer == AttackerType.Player || healer == AttackerType.Pet;
	public Color color => Color.green;


	public HealInfo(AttackerType _healer, IdleNumber _healRecovery)
	{
		healer = _healer;
		healRecovery = _healRecovery;
	}
}
