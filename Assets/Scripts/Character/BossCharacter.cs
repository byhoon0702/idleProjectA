using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : Unit
{
	public override void Spawn(UnitData data)
	{
		//rawData = data;
		info = new CharacterInfo(this, data, ControlSide.ENEMY);

		SetCharacterClass();

		if (characterView == null)
		{
			var model = UnitModelPoolManager.it.GetModel($"B/{data.resource}");
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one * 0.016f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			AnimationEventReceiver eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.GetComponent<AnimationEventReceiver>();
			if (eventReceiver == null)
			{
				eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.AddComponent<AnimationEventReceiver>();
			}
			eventReceiver.Init(this);

			characterView = model;
			gameObject.tag = "Enemy";
			gameObject.name = info.charNameAndCharId;
		}
		Init();
		GameUIManager.it.ShowCharacterGauge(this);
		targeting = Targeting.OPPONENT;
	}

	public override void Move(float delta)
	{
		transform.Translate(Vector3.left * info.MoveSpeed() * delta);
	}
}
