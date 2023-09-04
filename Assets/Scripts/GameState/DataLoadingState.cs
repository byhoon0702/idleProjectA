using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DataLoadingState : RootState
{
	public override void Init()
	{

	}
	public override async Task<IntroFSM> OnEnter()
	{
		elapsedTime = 0;

		Intro.it.SetActiveProgressBar(true);
		Intro.it.SetProgressBar(0, "Data Loading");

		LoadConfig();
		DataManager.LoadAllJson();

		return this;


	}
	public override IntroFSM RunNextState(float time)
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
			Intro.it.ShowTermsAgreementPopup();
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
