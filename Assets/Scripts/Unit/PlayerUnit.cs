using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerUnit : Unit
{
	public PlayerUnitInfo info
	{
		get;
		private set;
	}


	[SerializeField] private PlayableDirector hyperDirector;
	private float hpRecoveryRemainTime;
	public override string CharName => info.data.name;
	public override ControlSide ControlSide => ControlSide.PLAYER;
	public override UnitType UnitType => UnitType.Player;

	public override HitInfo HitInfo
	{
		get
		{

			float chance = Random.Range(0.001f, 100f);
			bool checkCritical = chance <= info.stats.GetValue(StatsType.Crits_Chance);
			CriticalType criticalType = CriticalType.Normal;
			IdleNumber damage = AttackPower;
			if (checkCritical)
			{
				criticalType = CriticalType.Critical;

				chance = Random.Range(0.001f, 100f);
				damage = AttackPower * (info.stats.GetValue(StatsType.Crits_Damage) / 100f);
				checkCritical = chance <= info.stats.GetValue(StatsType.Super_Crits_Chance);
				if (checkCritical)
				{
					criticalType = CriticalType.CriticalX2;
					damage = AttackPower * (info.stats.GetValue(StatsType.Crits_Damage) / 100f) * (info.stats.GetValue(StatsType.Super_Crits_Damage) / 100f);
				}
			}
			HitInfo hit = new HitInfo(damage, criticalType);

			return hit;
		}
	}
	public override IdleNumber AttackPower => info.AttackPower();
	public override float AttackSpeed => info.AttackSpeed();



	public override CriticalType RandomCriticalType => info.IsCritical();
	public override float CriticalDamageMultifly => info.CriticalDamageMultifly();
	public override float CriticalX2DamageMultifly => info.CriticalX2DamageMultifly();


	public override IdleNumber Hp { get => info.hp; set => info.hp = value; }
	public override IdleNumber MaxHp => info.maxHp;
	public override float MoveSpeed => info.MoveSpeed();
	public override Vector3 MoveDirection => Vector3.right;
	public override float AttackTime => info.attackTime;

	public int hyperPhase;

	private FxSpriteEffectAuto trainingLevelupEffect;

	private GameObject hyperModel;



	public void HPRecoveryUpdate(float _dt)
	{
		hpRecoveryRemainTime += _dt;
		if (hpRecoveryRemainTime >= GameManager.Config.PLAYER_HP_RECOVERY_CYCLE)
		{
			hpRecoveryRemainTime -= GameManager.Config.PLAYER_HP_RECOVERY_CYCLE;
			if (Hp < MaxHp)
			{
				var recoveryValue = info.HPRecovery();
				if (recoveryValue > 0)
				{
					Heal(new HealInfo(AttackerType.Player, recoveryValue));
				}
			}
		}
	}

	public override void Init()
	{
		base.Init();
	}

	public void AttackHyper()
	{
		if (target == null)
		{
			return;
		}
		HeadingToTarget();
		Vector3 hitPos = transform.position + headingDirection;
	}


	public override void Attack()
	{
		if (target == null)
		{
			return;
		}
		HeadingToTarget();

		skillModule.DefaultAttack(HitInfo);
		attackCount++;

		hyperModule?.StackHyperGauge();

	}

	public void PlayHyperFinishTimeline(TimelineAsset timelineAsset)
	{
		if (timelineAsset == null)
		{
			return;
		}

		SceneCamera.VirtualZoomCam.Follow = UnitManager.it.Player.transform;

		hyperDirector.playableAsset = timelineAsset;
		var tracks = (hyperDirector.playableAsset as TimelineAsset).GetOutputTracks();
		foreach (var track in tracks)
		{
			if (track.name.Equals("HyperCharacter"))
			{
				hyperDirector.SetGenericBinding(track, UnitManager.it.Player.unitAnimation.animator);
			}
			if (track.name.Equals("Cinemachine Zoom Track"))
			{
				hyperDirector.SetGenericBinding(track, SceneCamera.CinemachineBrain);
				if (track.hasClips)
				{
					var clips = track.GetClips().GetEnumerator();
					while (clips.MoveNext())
					{
						var current = clips.Current;
						var cinemachineShot = (current.asset as CinemachineShot);
						if (cinemachineShot == null)
						{
							continue;
						}
						if (current.displayName.Equals("CM vcam default"))
						{
							hyperDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, SceneCamera.DefaultVirtualCam);

						}
						if (current.displayName.Equals("CM vcam zoom"))
						{
							hyperDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, SceneCamera.VirtualZoomCam);

						}
					}
				}
			}
			if (track.name.Equals("MapDark"))
			{
				hyperDirector.SetGenericBinding(track, SceneCamera.it.BlackOutCurtain.GetComponent<Animator>());
			}
		}


		hyperDirector.Play();
	}

	public void ChangeNormalUnit(RuntimeData.AdvancementInfo _info)
	{
		if (unitMode is UnitNormalMode)
		{

			if (_info.Level == 0)
			{
				unitFacial?.ChangeFacial(0);
				return;
			}
			unitFacial?.ChangeFacial(_info.CostumeIndex);
		}
	}

	public void Spawn(UnitData _data, int _level = 1)
	{
		if (_data == null)
		{
			VLog.ScheduleLogError("No Unit Data");
		}

		info = new PlayerUnitInfo(this, _data);
		rigidbody2D = GetComponent<Rigidbody2D>();

		Init();

		instaneStats = Instantiate(stats);

		InitMode("B/Player", info);

		InactiveHyperEffect();

		headingDirection = Vector3.right;

		GameUIManager.it.ShowUnitGauge(this);

		SceneCamera.it.SetTarget(transform);
		InitSkills();
	}

	void InitSkills()
	{
		skillModule.Init(this, info.defaultSkillTid);
		skillModule.ChangeSkillSet(GameManager.UserDB.skillContainer.skillSlot);

		skillModule.ResetSkills();
	}

	public void NormalSpawn()
	{
		unitCostume = unitAnimation.GetComponent<NormalUnitCostume>();
		if (unitCostume != null)
		{
			unitCostume.Init();
			ChangeCostume();
		}
	}

	public bool TriggerHyperFinishSkill(SkillSlot skillSlot)
	{
		if (skillSlot == null)
		{
			Debug.LogError("스킬 슬롯이 비어있음");
			return false;
		}
		if (target == null)
		{
			FindTarget(0, true);
		}


		IdleNumber totalAttackPower = AttackPower;
		IdleNumber skillvalue = skillSlot.item.skillAbility.Value;
		IdleNumber skillBuffvalue = GameManager.UserDB.GetValue(StatsType.Skill_Damage);

		totalAttackPower = (totalAttackPower * (skillvalue + (skillvalue * skillBuffvalue))) / 100f;

		HitInfo info = new HitInfo(AttackPower);
		info = new HitInfo(totalAttackPower);

		skillModule.RegisterUsingSkill(skillSlot, info);
		//skillSlot.Use();
		//DialogueManager.it.CreateSkillBubble(skillSlot.item.Name, this);
		//unitAnimation.PlayAnimation("skill1");
		return true;
	}

	public override bool TriggerSkill(SkillSlot skillSlot)
	{
		if (target == null)
		{
			FindTarget(0, true);
		}

		IdleNumber totalAttackPower = AttackPower;
		IdleNumber skillvalue = skillSlot.item.skillAbility.Value;
		IdleNumber skillBuffvalue = GameManager.UserDB.GetValue(StatsType.Skill_Damage);

		totalAttackPower = (totalAttackPower * (skillvalue + (skillvalue * skillBuffvalue))) / 100f;

		HitInfo info = new HitInfo(AttackPower);
		info = new HitInfo(totalAttackPower);

		if (skillSlot.item.Instant)
		{
			skillModule.ActivateSkill(skillSlot, info);
		}
		else
		{
			skillModule.RegisterUsingSkill(skillSlot, info);
		}

		if (skillSlot.item.IsSkillState)
		{
			ChangeState(StateType.SKILL, true);
		}

		skillSlot.Use();
		DialogueManager.it.CreateSkillBubble(skillSlot.item.Name, this);

		unitAnimation.PlayAnimation(skillSlot.item.itemObject.animationClip);


		return true;
	}

	public bool ignoreAnimationEndEvent = true;
	public override void EndHyper()
	{
		hyperModule.EndHyper();
	}

	public override void UseSkill()
	{
		skillModule.ActivateSkill();
	}

	public void EquipWeapon()
	{
		var weaponSlot = GameManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON);
		if (weaponSlot.item == null || weaponSlot.item.Tid == 0)
		{
			return;
		}

		GameManager.UserDB.costumeContainer[CostumeType.WEAPON].Clear();
		unitCostume.EquipeWeapon(GameManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).item);
	}

	public override void ChangeCostume()
	{
		if (currentMode is UnitHyperMode)
		{
			return;
		}
		var headInfo = GameManager.UserDB.costumeContainer[CostumeType.HEAD].item;
		var bodyInfo = GameManager.UserDB.costumeContainer[CostumeType.BODY].item;
		var weaponInfo = GameManager.UserDB.costumeContainer[CostumeType.WEAPON].item;

		unitCostume.ChangeCostume(headInfo, bodyInfo, weaponInfo);


		if (weaponInfo == null)
		{
			unitCostume.EquipeWeapon(GameManager.UserDB.equipContainer.GetSlot(EquipType.WEAPON).item);
		}
	}

	public override void OnMove(float delta)
	{
		if (target == null)
		{
			return;
		}

		HeadingToTarget();

		rigidbody2D.MovePosition(transform.position + headingDirection * MoveSpeed * delta);
	}


	public override void Dash(float time)
	{

		if (target == null)
		{
			return;
		}

		HeadingToTarget();

		rigidbody2D.MovePosition(transform.position + headingDirection * MoveSpeed * 2f * time);
	}

	/// <summary>
	/// 하이퍼 모드 활성화 가능여부
	/// 해당 캐릭터가 사용할 수 있는 상태인지만 체크하면 됨
	/// </summary>
	public bool IsActiveHyperMode()
	{
		return true;
	}

	/// <summary>
	/// 하이퍼 모드 활성화
	/// </summary>
	public override void ActiveHyperEffect()
	{

		ChangeState(StateType.IDLE, true);

		unitHyperMode?.OnModeEnter(currentState);
		currentMode = unitHyperMode;

		hyperDirector.playableAsset = hyperModule.hyperChangeTimeline;
		var tracks = (hyperDirector.playableAsset as TimelineAsset).GetOutputTracks();

		foreach (var track in tracks)
		{
			if (track.name.Equals("NormalModeDissolve"))
			{
				hyperDirector.SetGenericBinding(track, unitMode.ModelAnimation);
			}
			if (track.name.Equals("HyperModeDissolve"))
			{
				hyperDirector.SetGenericBinding(track, unitHyperMode.ModelAnimation);
			}
			if (track.name.Equals("MapDark"))
			{
				hyperDirector.SetGenericBinding(track, SceneCamera.it.BlackOutCurtain.GetComponent<Animator>());
			}
		}

		hyperDirector.Play();


		GameManager.GameStop = true;
		VLog.SkillLog($"[{NameAndId}] 하이퍼 모드 발동");
	}

	public void EnterHyperMode()
	{
		unitMode?.OnModeExit();
		GameManager.GameStop = false;
	}

	public void ExitNormalMode()
	{
		unitMode?.OnModeExit();
	}


	protected override void Update()
	{
		base.Update();

		if (IsAlive() == false)
		{

			hyperDirector.Stop();
		}

		HPRecoveryUpdate(Time.deltaTime);
	}
	/// <summary>
	/// 하이퍼 모드 비활성화
	/// </summary>
	public override void InactiveHyperEffect()
	{
		unitMode?.OnModeEnter(currentState);
		unitHyperMode?.OnModeExit();

		currentMode = unitMode;
		//unitAnimation.InactiveHyperEffect();

		attackEffectObject = null;
		hitEffectObject = null;
		skillModule.Init(this, info.defaultSkillTid);
		skillModule.ChangeSkillSet(GameManager.UserDB.skillContainer.skillSlot);

		UIController.it.HyperSkill.SetHyperMode(false);

		VLog.SkillLog($"[{NameAndId}] 하이퍼 모드 발동종료");
	}

	/// <summary>
	/// 하이퍼 브레이크 발동이 가능한 상태인지
	/// </summary>
	public bool UseableHyperBreak()
	{
		return target != null;
	}

	/// <summary>
	/// 하이퍼 브레이크 발동
	/// </summary>
	public void StartHyperBreak()
	{
		VLog.SkillLog($"[{NameAndId}] 하이퍼 브레이크");

		FinishHyperBreak(); // <- 애니메이션 끝나는 시점에 호출하게 바꿔주세요
	}

	/// <summary>
	/// 하이퍼 브레이크 스킬종료
	/// </summary>
	public void FinishHyperBreak()
	{
		VLog.SkillLog($"[{NameAndId}] 하이퍼 브레이크 종료");
	}

	public override void PlayAnimation(StateType type)
	{
		base.PlayAnimation(type);
	}

	public override void Hit(HitInfo _hitInfo)
	{
		if (_hitInfo.TotalAttackPower == 0)
		{
			return;
		}
		if (GameManager.GameStop)
		{
			return;
		}
		if (Hp > 0)
		{
			hitCount++;

			Vector3 reverse = headingDirection;
			reverse.x = HeadPosition.x + (0.7f * -currentDir);
			reverse.y = HeadPosition.y + 0.6f;
			reverse.z = HeadPosition.z;
			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, HeadPosition, reverse, TextType.PLAYER_HIT);
			ShakeUnit();
		}
		Hp -= _hitInfo.TotalAttackPower;

		GameManager.it.battleRecord.RecordAttackPower(_hitInfo);

		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}

		var go = Instantiate(hitEffect).GetComponent<UVAnimation>();
		go.transform.position = position;
		go.transform.localScale = Vector3.one;
		go.transform.rotation = unitAnimation.transform.rotation;
		go.Play(null);

		currentMode?.OnHit(_hitInfo);
	}

	Transform levelupTrans;
	public void PlayLevelupEffect(StatsType ability)
	{
		if (levelupTrans == null)
		{
			GameObject go = (GameObject)Instantiate(Resources.Load("LevelUpEffect"), transform);
			go.transform.localPosition = Vector3.zero;

			levelupTrans = go.transform;
			levelupTrans.localEulerAngles = new Vector3(-60, 0, 0);
		}
		levelupTrans.gameObject.SetActive(false);
		levelupTrans.gameObject.SetActive(true);

		//체력 버프의 경우 증가한 만큼 현재 체력에 더해준다
		if (ability == StatsType.Hp || ability == StatsType.Hp_Buff)
		{
			info.UpdateMaxHP(this);
		}
	}

	public void UpdateMaxHp()
	{
		info.UpdateMaxHP(this);
	}

	public override void RandomTarget(float _time, bool _ignoreSearchDelay)
	{
		base.RandomTarget(_time, _ignoreSearchDelay);

		var list = UnitManager.it.GetEnemies();
		int count = list.Count;

		List<HittableUnit> unitlist = new List<HittableUnit>();
		for (int i = 0; i < count; i++)
		{
			if (list[i].IsAlive())
			{
				unitlist.Add(list[i]);
			}
		}

		int index = UnityEngine.Random.Range(0, unitlist.Count);
		HittableUnit newTarget = unitlist[index];

		if (TargetAddable(newTarget))
		{
			SetTarget(newTarget);
		}
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


	public override void FindTarget(float _time, bool _ignoreSearchDelay, float range)
	{
		base.FindTarget(_time, _ignoreSearchDelay, range);

		if (searchInterval > GameManager.Config.TARGET_SEARCH_DELAY || _ignoreSearchDelay)
		{
			searchInterval = 0;

			// 새로운 타겟을 찾음
			HittableUnit newTarget = FindNewTarget(UnitManager.it.GetEnemies(), range);
			if (TargetAddable(newTarget))
			{
				SetTarget(newTarget);
			}
		}
	}



	public void ResetUnit()
	{
		Hp = MaxHp;
	}



	public override void Debuff(List<StatInfo> debufflist)
	{

	}
}
