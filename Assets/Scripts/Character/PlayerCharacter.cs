using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerCharacter : Unit
{
	public int side;

	Vector3 moveDirection;
	RaycastHit raycastHit;

	List<Companion> companions = new List<Companion>();
	private float hpRecoveryRemainTime;



	protected override void LateUpdate()
	{
		base.LateUpdate();

		if(currentState != StateType.DEATH)
		{
			HPRecoveryUpdate(Time.deltaTime);
		}
	}

	private void HPRecoveryUpdate(float _dt)
	{
		//hpRecoveryRemainTime += _dt;
		//if(hpRecoveryRemainTime >= ConfigMeta.it.PLAYER_HP_RECOVERY_CYCLE)
		//{
		//	hpRecoveryRemainTime -= ConfigMeta.it.PLAYER_HP_RECOVERY_CYCLE;
		//	Heal(new HealInfo(AttackerType.Player, new IdleNumber()));
		//}
	}

	public override void Spawn(UnitData _data)
	{
		info = new CharacterInfo(this, _data, ControlSide.PLAYER);
		side = 1;
		SetCharacterClass();

		if (characterView == null)
		{
			var model = UnitModelPoolManager.it.GetModel(("B/" + this.info.data.resource));
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one * 0.008f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			AnimationEventReceiver eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.GetComponent<AnimationEventReceiver>();
			if (eventReceiver == null)
			{
				eventReceiver = model.GetComponent<UnitAnimation>().animator.gameObject.AddComponent<AnimationEventReceiver>();
			}
			eventReceiver.Init(this);


			characterView = model;


			if (_data.classTid == 4)
			{
				gameObject.tag = "Wall";
			}
			else
			{
				gameObject.tag = "Player";
			}


			gameObject.name = info.charNameAndCharId;
		}
		Init();
		GameUIManager.it.ShowCharacterGauge(this);
		companions.AddRange(CharacterManager.it.GetCompanions());
		targeting = Targeting.OPPONENT;

		if (UnitGlobal.it.hyperModule.IsHyperMode)
		{
			ActiveHyperEffect();
		}
	}

	private Vector3 SimpleCrowdAI(float _deltatime)
	{
		moveDirection = Vector3.right * side;

		return moveDirection;
	}

	public override void Move(float _delta)
	{
		moveDirection = SimpleCrowdAI(_delta);

		for (int i = 0; i < companions.Count; i++)
		{
			companions[i].transform.Translate(moveDirection * info.MoveSpeed() * _delta);
		}
		transform.Translate(moveDirection * info.MoveSpeed() * _delta);
	}

	public override void OnDefaultAttack()
	{
		base.OnDefaultAttack();
		UnitGlobal.it.hyperModule.AccumGauge();
	}

	public void ActiveHyperEffect()
	{
		characterAnimation.ActiveHyperEffect();
	}

	public void InactiveHyperEffect()
	{
		characterAnimation.InactiveHyperEffect();
	}
}
