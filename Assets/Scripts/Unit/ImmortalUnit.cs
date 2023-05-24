using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalUnit : EnemyUnit
{
	public override bool IsAlive()
	{
		return true;
	}

	public override void CheckDeathState()
	{
		//base.CheckDeathState();
	}

	public override void Hit(HitInfo _hitInfo)
	{
		//base.Hit(_hitInfo);

		if (_hitInfo.ShakeCamera)
		{
			SceneCamera.it.ShakeCamera();
		}

		GameUIManager.it.ShowFloatingText(_hitInfo.TotalAttackPower, CenterPosition, CenterPosition, _hitInfo.criticalType);
		ShakeUnit();


		// HP가 0이 되면 다음레벨로 리스폰
		IdleNumber attackPower = _hitInfo.TotalAttackPower;
		while (Hp <= attackPower)
		{
			attackPower = attackPower - Hp;
			var unitData = info.data;
			info.unitLevel = info.unitLevel + 1;

			info = new EnemyUnitInfo(this, unitData, StageManager.it.CurrentStage);

		}
		Hp -= attackPower;

		UIController.it.UiStageInfo.SetBossHpGauge(Mathf.Clamp01((float)(Hp / MaxHp)));


		GameManager.it.battleRecord.RecordAttackPower(_hitInfo);
		if (ControlSide == ControlSide.ENEMY)
		{
			UIController.it.UiStageInfo.RefreshDPSCount();
		}
		if (_hitInfo.hitSound.IsNullOrWhiteSpace() == false)
		{
			VSoundManager.it.PlayEffect(_hitInfo.hitSound);
		}
	}
}
