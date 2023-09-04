using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Services.PushNotifications;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{


	//public static NotificationManager Instance;

	private const string ChannelId = "Offline_Reward";
	private void Awake()
	{
		//Instance = this;
	}

	public void Init()
	{
		//string token = await PushNotificationsService.Instance.RegisterForPushNotificationsAsync();
		//Debug.Log($"The push notification token is {token}");

		//AndroidNotificationCenter.OnNotificationReceived += _OnNotificationReceived;
		//CheckNotificationIntentData();
		//RegisterNotificationChannel();
	}

	private void RegisterNotificationChannel()
	{
		var c = new AndroidNotificationChannel()
		{
			Id = ChannelId,
			Name = "Default Channel",
			Importance = Importance.Default,
			Description = "Generic notification",
		};
		AndroidNotificationCenter.RegisterNotificationChannel(c);

	}

	public void SendNotification()
	{
		var notification = new AndroidNotification();
		notification.Title = "";
		notification.Text = "";
		notification.FireTime = TimeManager.Instance.Now.AddHours(12);
		AndroidNotificationCenter.SendNotification(notification, ChannelId);
	}
	private void _OnNotificationReceived(AndroidNotificationIntentData data)
	{

	}

	public void AddEvent()
	{
		//PushNotificationsService.Instance.OnNotificationReceived += OnNotification;
	}

	public void OnNotification(Dictionary<string, object> d)
	{
		Debug.Log("Received Notification");
	}


	private void CheckNotificationIntentData()
	{
		var intentData = AndroidNotificationCenter.GetLastNotificationIntent();
		if (intentData != null)
		{
			var id = intentData.Id;
		}
	}
}
