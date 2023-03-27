using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDungeonListBossPin : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private WaveType waveType;

	private UIDungeonListBossMap owner;
	public GameStageInfo StageInfo { get; private set; }


	private void Awake()
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(OnClick);
	}

	public void OnUpdate(UIDungeonListBossMap _owner)
	{
		owner = _owner;
		StageInfo = StageManager.it.metaGameStage.GetStage(waveType, UserInfo.stage.RecentStageLv(waveType));
		if (StageInfo == null)
		{
			PopAlert.Create(new VResult().SetFail(VResultCode.INVALID_DATA, $""));
		}
	}

	private void OnClick()
	{
		owner.SelectStage(StageInfo);
	}
}
