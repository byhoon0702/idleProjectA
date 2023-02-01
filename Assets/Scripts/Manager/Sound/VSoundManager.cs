using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSoundManager : MonoBehaviour
{
	public static VSoundManager it => instance;
	private static VSoundManager instance;

	private BgmPlayer bgmPlayer;



	private void Awake()
	{
		instance = this;

		GameObject go = new GameObject("BGM");
		go.transform.SetParent(transform);
		bgmPlayer = go.AddComponent<BgmPlayer>();
		bgmPlayer.Initialize();
	}

	public void PlayBgm(string _bgmKey)
	{
		bgmPlayer.Play(_bgmKey);
	}

	public void PlayEffect(string _effectKey)
	{
		SoundEffectPlayer effectPlayer = GetEffectPlayer();

		effectPlayer.Play(_effectKey);
	}

	private SoundEffectPlayer GetEffectPlayer()
	{
		var effectPlayers = GetComponentsInChildren<SoundEffectPlayer>(true);
		foreach(var player in effectPlayers)
		{
			if(player.gameObject.activeInHierarchy == false)
			{
				return player;
			}
		}

		GameObject effectPlayer = new GameObject("EffectPlayer");
		effectPlayer.transform.SetParent(transform);

		return effectPlayer.AddComponent<SoundEffectPlayer>();
	}
}
