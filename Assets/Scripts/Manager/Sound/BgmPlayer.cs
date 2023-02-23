using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
	private AudioSource audioSource;
	private SoundBgmData currBgmData;


	public void Initialize()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		UpdateBgmVolume(GameSetting.it.property.bgmVolume);
	}

	public void UpdateBgmVolume(float _vol)
	{
		if (audioSource != null && audioSource.enabled)
		{
			audioSource.volume = GameSetting.it.property.bgmVolume * 0.01f;
		}
	}

	public void Play(string _soundKey)
	{
		var bgmData = DataManager.Get<SoundBgmDataSheet>().Get(_soundKey);
		if (bgmData == null)
		{
			VLog.SoundLogError($"BGM 못찾음. key: {_soundKey}");
			return;
		}

		if (currBgmData != null && bgmData.tid == currBgmData.tid)
		{
			// 동일하면 무시
			VLog.SoundLog($"동일한 사운드 재생시도. key: {bgmData.key}, res: {bgmData.resource}");
			return;
		}

		Stop();

		var clip = Resources.Load<AudioClip>($"BgmSound/{bgmData.resource}");
		if (clip == null)
		{
			VLog.SoundLogError($"BGM 리소스 못찾음. key: {bgmData.key}, res: {bgmData.resource}");
			return;
		}


		audioSource.clip = clip;
		audioSource.Play();
		currBgmData = bgmData;

		VLog.SoundLog($"{bgmData.description} 재생. key: {bgmData.key}, res: {bgmData.resource}");
	}

	public void Stop()
	{
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
		currBgmData = null;
	}
}
