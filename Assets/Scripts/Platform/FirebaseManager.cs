using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
public class FirebaseManager : MonoBehaviour
{
	public FirebaseApp App { get; private set; }

	// Start is called before the first frame update
	public void Init()
	{
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
		{
			if (task.Result == DependencyStatus.Available)
			{
				App = FirebaseApp.DefaultInstance;
				Debug.Log("FirebaseInit");
			}
			else
			{
				Debug.LogError($"Firebase not launching {task.Result}");
			}

		});

	}

	public void StageLog(int stageNumber)
	{
		FirebaseAnalytics.LogEvent($"fb_stage_completed", "stage_number", stageNumber);
		//if (stageNumber == 1 || stageNumber % 50 == 0)
		//{
		//	FirebaseAnalytics.LogEvent($"fb_stage_{stageNumber}_completed");
		//}
	}

	public void Log(string eventName)
	{
		FirebaseAnalytics.LogEvent(eventName);
	}


}
