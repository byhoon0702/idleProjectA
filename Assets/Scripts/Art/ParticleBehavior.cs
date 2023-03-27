using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleBehavior : MonoBehaviour
{
	public ParticleSystem particle;

	private Action onStop;
	public void Init(Action _onStop)
	{
		particle = GetComponent<ParticleSystem>();
		var main = particle.main;
		main.stopAction = ParticleSystemStopAction.Callback;

		onStop = _onStop;
	}

	public void SetSpeed(float speed)
	{
		var main = particle.main;
		main.simulationSpeed = speed;
	}
	public void Stop()
	{
		particle.Stop();
	}

	public void Play()
	{
		particle.Play();
	}

	private void OnParticleSystemStopped()
	{
		onStop?.Invoke();
	}
}
