using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadingState : RootState
{
	public override void Init()
	{

	}
	public override FSM OnEnter()
	{
		elapsedTime = 0;

		Intro.it.SetActiveProgressBar(true);
		Intro.it.SetProgressBar(0, "Data Loading");

		LoadConfig();
		DataManager.LoadAllJson();
		return this;


	}
	public override FSM RunNextState(float time)
	{
		return this;
	}

	public override void OnExit()
	{

	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
		Intro.it.SetProgressBar(Mathf.Clamp01(elapsedTime / 0.1f), "Data Loading");
		if (elapsedTime > 0.1f)
		{
			Intro.it.SetActiveProgressBar(false);
			Intro.it.ChangeState(IntroState_e.LOGIN);
		}
	}

	private void LoadConfig()
	{
		TextAsset textAsset = Resources.Load<TextAsset>($"Data/Json/{ConfigMeta.fileName.Replace(".json", "")}");

		if (textAsset == null)
		{
			VLog.LogError("Config 로드 실패");
			return;
		}

		//JsonUtility.FromJsonOverwrite(textAsset.text, ConfigMeta.it);
	}
}
