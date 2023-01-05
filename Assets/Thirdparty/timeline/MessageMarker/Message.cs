using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public enum ParameterType
{
	Int,
	Float,
	String,
	Object,
	Event,
	Pointer,
	None
}

[Serializable]
public class Message : Marker, INotification, INotificationOptionProvider
{
	public string method;
	public bool retroactive;
	public bool emitOnce;

	public ParameterType parameterType;
	public int Int;
	public string String;
	public float Float;
	public UnityEvent Event;
	public ExposedReference<Object> Object;
	public PointerEventData Pointer;
	public string exposedName;
	PropertyName INotification.id
	{
		get
		{
			return new PropertyName(method);
		}
	}

	NotificationFlags INotificationOptionProvider.flags
	{
		get
		{
			return (retroactive ? NotificationFlags.Retroactive : default(NotificationFlags)) |
				(emitOnce ? NotificationFlags.TriggerOnce : default(NotificationFlags));
		}
	}
}
