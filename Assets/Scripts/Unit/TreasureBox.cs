using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : HittableUnit
{
	public UnitData info;
	public override IdleNumber AttackPower => (IdleNumber)0;


	public override ControlSide ControlSide => ControlSide.ENEMY;
	public override UnitType UnitType => UnitType.TreasureBox;
	public override HitInfo HitInfo
	{
		get
		{
			return new HitInfo(gameObject.layer, AttackPower);
		}
	}
	public override float AttackSpeed => 0;

	public override IdleNumber Hp { get; set; }

	private IdleNumber maxHp;
	public override IdleNumber MaxHp => maxHp;

	public override Vector3 CenterPosition => position;


	public void Spawn(UnitData _spawnInfo, RuntimeData.StageInfo _stageInfo)
	{
		info = _spawnInfo;

		if (model == null)
		{
			var model = Instantiate(Resources.Load<GameObject>($"B/TreasureBox"));
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one;
			//maxHp = _stageInfo.UnitHP(false);
			Hp = maxHp;

			if (SceneCamera.it != null)
			{
				Camera sceneCam = SceneCamera.it.sceneCamera;
				model.transform.LookAt(model.transform.position + sceneCam.transform.rotation * Vector3.forward, sceneCam.transform.rotation * Vector3.up);
			}

			base.model = model.gameObject;
			model.gameObject.tag = "Enemy";
			unitAnimation = model.GetComponent<UnitAnimation>();
			unitAnimation.Init();
			unitAnimation.SetMaskInteraction();
			GameUIManager.it.ShowUnitGauge(this);
		}
	}

	public override bool IsAlive()
	{
		return Hp > 0;
	}

	private void LateUpdate()
	{
		if (isRewardable && IsAlive() == false)
		{
			isRewardable = false;
			//StageManager.it.CheckKillRewards(UnitType, transform);
		}
	}

	protected override void OnHit(HitInfo hitInfo)
	{

	}

	protected override IEnumerator OnHitRoutine(HitInfo hitInfo)
	{
		yield return null;
	}
	public override void Hit(HitInfo _hitInfo, RuntimeData.SkillInfo _skillInfo)
	{
		if (Hp > 0)
		{
			if (_hitInfo.ShakeCamera)
			{
				SceneCamera.it.ShakeCamera();
			}
			GameObject go = Instantiate(hitEffect);
			go.transform.position = CenterPosition;
			go.transform.localScale = Vector3.one;

			TextType textType = TextType.ENEMY_HIT;

			if (_hitInfo.criticalType == CriticalType.CriticalX2)
			{
				textType = TextType.CRITICAL_X2;
			}
			else if (_hitInfo.criticalType == CriticalType.Critical)
			{
				textType = TextType.CRITICAL;
			}

			GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, CenterPosition, CenterPosition, textType);
			unitAnimation?.PlayDamageWhite();
		}

		Hp -= _hitInfo.TotalAttackPower;

		GameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		//if (ControlSide == ControlSide.ENEMY)
		//{
		//	UIController.it.UiStageInfo.RefreshDPSCount();
		//}
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			SoundManager.Instance.PlayEffect(_hitInfo.hitSound);
		}
	}

	//public override void SetAttack(SkillEffectData data, SkillInfoObject infoObject = null, SkillData _skillData = null)
	//{
	//	//throw new System.NotImplementedException();
	//}



}
