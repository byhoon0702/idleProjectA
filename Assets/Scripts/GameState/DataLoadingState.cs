using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadingState : RootState
{
	public override void OnEnter()
	{
		DataManager.it.LoadAllJson();
		LoadConfig();
		LoadSkillMeta();
		VGameManager.it.mapController.Reset();
		GameUIManager.it.mainUIObject.SetActive(true);
		GameUIManager.it.ReleaseAllPool();
		elapsedTime = 0;

		UserInfo.LoadUserData();

		UIController.it.Init();
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		if (elapsedTime > 1)
		{
			VGameManager.it.ChangeState(GameState.LOADING);

		}
	}

	private void LoadConfig()
	{
		VGameManager.it.config = ScriptableObject.CreateInstance<ConfigMeta>();
		TextAsset textAsset = Resources.Load<TextAsset>($"Json/{ConfigMeta.fileName.Replace(".json", "")}");

		if (textAsset == null)
		{
			VLog.LogError("Config 로드 실패");
			return;
		}

		JsonUtility.FromJsonOverwrite(textAsset.text, VGameManager.it.config);
	}

	private void LoadSkillMeta()
	{
		VGameManager.it.skillMeta = new SkillMeta();
		VGameManager.it.skillMeta.LoadData();
	}
}
