using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class Pet : Unit
{
	public PlayerUnit follow;
	public int index;
	public float speed = 5f;

	public new Rigidbody2D rigidbody;

	public override IdleNumber Hp { get; set; }

	public override IdleNumber MaxHp => (IdleNumber)100;

	public override UnitType UnitType => UnitType.Pet;

	public override ControlSide ControlSide => ControlSide.NO_CONTROL;

	public override IdleNumber AttackPower => (IdleNumber)0;

	public override HitInfo HitInfo => new HitInfo(gameObject.layer, (IdleNumber)0);

	public override float AttackSpeed => throw new System.NotImplementedException();

	public void Spawn(PetSlot petSlot, int _index, PlayerUnit _follow)
	{
		Init();
		index = _index;
		follow = _follow;
		GameObject go = Instantiate(petSlot.item.PetObject, transform);
		Vector3 pos = new Vector3(index * -0.8f, 0, 0);
		if (follow != null)
		{
			pos += follow.position;
		}
		transform.position = pos;
		transform.localScale = Vector3.one;

		Camera sceneCam = SceneCamera.it.sceneCamera;
		go.transform.LookAt(go.transform.position + sceneCam.transform.rotation * Vector3.forward, sceneCam.transform.rotation * Vector3.up);
		UnitAnimation petAnimation = go.GetComponent<UnitAnimation>();
		petAnimation.Init();
		petAnimation.SetMaskInteraction();
		unitAnimation = petAnimation;
		rigidbody = GetComponent<Rigidbody2D>();

		speed = Random.Range(1f, 3f);
		target = follow;
		Hp = MaxHp;

		skillModule.Init(this, 0);
		ChangeState(StateType.IDLE, true);
		PlatformManager.UserDB.skillContainer.EquipPetSkill(this, petSlot.index, petSlot.item.SkillTid);
		UIController.it.SkillGlobal.OnUpdate();
	}

	public override void OnMove(float delta)
	{
		Move();
	}


	public bool IsNextToFollow()
	{
		Vector3 targetPos = follow.transform.position + (follow.headingDirection * -1 * (index * 0.5f));

		Vector3 myPos = transform.position;
		return Vector3.Distance(targetPos, myPos) <= 0.05f;
	}
	public bool IsFollowAlive()
	{
		if (follow == null)
		{
			return false;
		}
		return follow.IsAlive();
	}
	void Move()
	{
		if (follow == null)
		{
			return;
		}
		Vector3 targetPos = follow.transform.position + (follow.headingDirection * -1 * (index * 0.5f));

		Vector3 myPos = transform.position;

		if (targetPos.x - myPos.x > 0.1f)
		{
			unitAnimation.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (targetPos.x - myPos.x < -0.1f)
		{
			unitAnimation.transform.localScale = new Vector3(-1, 1, 1);
		}

		rigidbody.MovePosition(myPos + (targetPos - myPos).normalized * Time.deltaTime * speed);
	}
	public override void FindTarget(float _time, bool _ignoreSearchDelay)
	{
		base.FindTarget(_time, _ignoreSearchDelay);

		if (searchInterval > GameManager.Config.TARGET_SEARCH_DELAY || _ignoreSearchDelay)
		{
			searchInterval = 0;

			// 새로운 타겟을 찾음
			HittableUnit newTarget = FindNewTarget(UnitManager.it.GetEnemies());
			if (TargetAddable(newTarget))
			{
				SetTarget(newTarget);
			}
		}
	}
	public override bool TriggerSkill(SkillSlot skillSlot)
	{
		target = null;
		if (skillSlot.item.rawData.skillType == SkillType.BUFF || skillSlot.item.rawData.skillType == SkillType.HEAL)
		{
			target = UnitManager.it.Player;
		}
		else
		{
			FindTarget(0, true);
		}

		if (target == null)
		{
			return false;
		}

		IdleNumber skillvalue = skillSlot.item.skillAbility.Value;

		HitInfo info = new HitInfo(gameObject.layer, skillvalue);

		if (skillSlot.item.rawData.animation.IsNullOrEmpty() == false)
		{
			unitAnimation.PlayAnimation(skillSlot.item.rawData.animation);
			if (skillSlot.item.Instant)
			{
				skillModule.ActivateSkill(skillSlot, info);
			}
			else
			{
				skillModule.RegisterUsingSkill(skillSlot, info);
			}
		}
		else
		{
			skillModule.ActivateSkill(skillSlot, info);
		}

		if (skillSlot.item.IsSkillState)
		{
			ChangeState(StateType.SKILL, true);
		}


		skillSlot.Use();
		//DialogueManager.it.CreateSkillBubble(skillSlot.item.Name, this);


		return true;
	}
	public override void ActiveHyperEffect()
	{

	}

	public override void InactiveHyperEffect()
	{

	}

	protected override void OnHit(HitInfo hitInfo)
	{

	}

	protected override IEnumerator OnHitRoutine(HitInfo hitInfo)
	{
		yield return null;
	}

	protected override void Update()
	{
		base.Update();
	}

}
