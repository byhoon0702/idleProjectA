using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

	private BgmPlayer bgmPlayer;

	private void Awake()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}

		DontDestroyOnLoad(gameObject);
	}

	public void Init()
	{
		if (bgmPlayer == null)
		{
			GameObject go = new GameObject("BGM");
			go.transform.SetParent(transform);
			bgmPlayer = go.AddComponent<BgmPlayer>();
			bgmPlayer.Initialize();
		}
		muteall = false;
	}

	public void PlayBgm(AudioClip clip)
	{
		if (GameSetting.Instance.Bgm == false || muteall)
		{
			return;
		}
		if (clip == null)
		{
			return;
		}

		bgmPlayer.Play(clip);
	}
	public void PlayBgm(string _bgmKey)
	{
		if (GameSetting.Instance.Bgm == false || muteall)
		{
			return;
		}

		bgmPlayer.Play(_bgmKey);
	}

	public void PlayEffect(AudioClip clip)
	{
		if (GameSetting.Instance.Fx == false || muteall)
		{
			return;
		}
		if (clip == null)
		{
			return;
		}

		SoundEffectPlayer effectPlayer = GetEffectPlayer();

		effectPlayer.Play(clip);
	}
	public void PlayEffect(string _effectKey)
	{
		if (GameSetting.Instance.Fx == false || muteall)
		{
			return;
		}

		SoundEffectPlayer effectPlayer = GetEffectPlayer();

		effectPlayer.Play(_effectKey);
	}

	private SoundEffectPlayer GetEffectPlayer()
	{
		var effectPlayers = GetComponentsInChildren<SoundEffectPlayer>(true);
		foreach (var player in effectPlayers)
		{
			if (player.gameObject.activeInHierarchy == false)
			{
				return player;
			}
		}

		GameObject effectPlayer = new GameObject("EffectPlayer");
		effectPlayer.transform.SetParent(transform);

		return effectPlayer.AddComponent<SoundEffectPlayer>();
	}
	bool muteall;
	public void MuteAll()
	{
		bgmPlayer.Mute();
		var effectPlayers = GetComponentsInChildren<SoundEffectPlayer>(true);
		foreach (var player in effectPlayers)
		{
			player.enabled = false;
		}
		muteall = true;
	}
	public void UnmuteAll()
	{
		muteall = false;
		bgmPlayer.Unmute();
		var effectPlayers = GetComponentsInChildren<SoundEffectPlayer>(true);
		foreach (var player in effectPlayers)
		{
			player.enabled = true;
		}
	}
}
