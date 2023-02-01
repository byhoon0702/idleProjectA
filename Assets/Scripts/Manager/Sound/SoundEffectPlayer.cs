using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
	public string soundKey;
	private AudioSource audioSource;




	private void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void Play(string _soundKey)
	{
		soundKey = _soundKey;

		var soundData = DataManager.it.Get<SoundEffectDataSheet>().Get(_soundKey);
		if(soundData == null)
		{
			VLog.SoundLogError($"Effect 못찾음. SoundEffectDataSheet. key: {_soundKey}");
			ReturnSound();
			return;
		}

		var clip = Resources.Load<AudioClip>($"EffectSound/{soundData.resource}");
		if (clip == null)
		{
			VLog.SoundLogError($"Effect 리소스 못찾음. SoundEffectDataSheet. key: {_soundKey}, res: {soundData.resource}");
			ReturnSound();
			return;
		}

		gameObject.SetActive(true);

		audioSource.clip = clip;
		audioSource.volume = GameSetting.it.property.fxVolume * 0.01f;
		audioSource.Play();

		VLog.SoundLog($"{soundData.description} 재생. key: {_soundKey}, res: {soundData.resource}");

		StartCoroutine(CheckEnd());
	}

	private IEnumerator CheckEnd()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
			if(audioSource.isPlaying == false)
			{
				ReturnSound();
			}
		}
	}
	
	private void ReturnSound()
	{
		gameObject.SetActive(false);
	}

	public void Stop()
	{
		audioSource.Stop();
		ReturnSound();
	}
}
