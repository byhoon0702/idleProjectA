using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "Camera Shake Effect", menuName = "SkillScreenEffect/Camera Shake", order = 1)]
public class SkillCameraShakeEffect : SkillScreenEffect
{
	public float amount;
	public float duration;
	public bool isFade;

	private float impactTime = 0.2f;
	private float impactForce = 1f;
	private Vector3 defaultVelocity = new Vector3(0f, -1f, 0f);
	private AnimationCurve impulseCurve;

	private float listenerAmplitude = 1f;
	private float listenerFrequency = 1f;
	private float listenerDuration = 1f;

	public override void DoEffect(float multi = 1f)
	{
		if (SceneCamera.it != null)
		{
			SceneCamera.it.ShakeCamera(amount * multi, duration, isFade);
		}
	}
}
