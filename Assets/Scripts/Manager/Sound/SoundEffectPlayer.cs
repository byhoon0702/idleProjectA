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
	private void Start()
	{
		GameSetting.Instance.FxChanged += OnFxChanged;
		OnFxChanged(GameSetting.Instance.Fx);
	}
	private void OnDestroy()
	{
		GameSetting.Instance.FxChanged -= OnFxChanged;
	}

	private void OnFxChanged(bool isOn)
	{
		audioSource.enabled = isOn;
	}

	public void Play(AudioClip clip)
	{
		gameObject.SetActive(true);
		audioSource.clip = clip;
		audioSource.Play();

		StartCoroutine(CheckEnd());
	}

	public void Play(string _soundKey)
	{
		soundKey = _soundKey;

		var soundData = DataManager.Get<SoundEffectDataSheet>().Get(_soundKey);
		if (soundData == null)
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
		audioSource.volume = 1;
		audioSource.Play();

		VLog.SoundLog($"{soundData.description} 재생. key: {_soundKey}, res: {soundData.resource}");

		StartCoroutine(CheckEnd());
	}

	private IEnumerator CheckEnd()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			if (audioSource.isPlaying == false)
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
