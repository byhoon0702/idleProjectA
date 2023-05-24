using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

	private bool isFade = false;

	private float timer = 0f;
	private float perSec = 0f;
	public void Shake(float intensity, float time, bool isFade = false)
	{
		var perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		perlin.m_AmplitudeGain = intensity;
		this.timer = time;
		this.isFade = isFade;


		perSec = intensity / time;
	}
	// Update is called once per frame
	void LateUpdate()
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
			CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

			if (isFade)
			{
				cinemachineBasicMultiChannelPerlin.m_AmplitudeGain -= Time.deltaTime * perSec;
			}
			if (timer <= 0)
			{
				cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
				timer = 0f;
			}
		}

	}
}
