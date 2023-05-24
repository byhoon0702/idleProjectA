using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public delegate void OnAttack();

[RequireComponent(typeof(Animator))]
public class AnimationEventReceiver : MonoBehaviour
{
	public event OnAttack onAttack;

	public void AddEvent(OnAttack listener)
	{
		if (onAttack.IsRegistered(listener) == false)
		{
			onAttack += listener;
		}
	}
	public void RemoveEvent(OnAttack listener)
	{
		if (onAttack.IsRegistered(listener))
		{
			onAttack -= listener;
		}
	}
	public void Attack()
	{
		onAttack?.Invoke();
	}
}
