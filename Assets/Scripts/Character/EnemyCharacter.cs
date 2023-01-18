using System.Buffers;
using UnityEngine;

public class EnemyCharacter : Character
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
			model.transform.localScale = Vector3.one * 0.005f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			characterView = model;
			gameObject.AddComponent<SphereCollider>();
			gameObject.tag = "Enemy";
			gameObject.name = info.charNameAndCharId;
		}
		Init();
		targeting = Targeting.OPPONENT;
	}

	public override void Move(float delta)
	{
		transform.Translate(Vector3.left * info.MoveSpeed() * delta);
	}

	public override void Hit(Character _attacker, IdleNumber _attackPower, string _attackName, Color _color, float _criticalChanceMul = 1)
	{
		IdleNumber totalAttackPower = _attackPower * _criticalChanceMul * _attacker.info.DamageMul();
		bool isCriticalAttack = _criticalChanceMul > 1;

		if (info.hp > 0)
		{
			SceneCamera.it.ShakeCamera();
			GameUIManager.it.ShowFloatingText(totalAttackPower.ToString(), _color, characterAnimation.CenterPivot.position, isCriticalAttack);
		}
		info.hp -= totalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_attacker.charID, charID, _attackName, totalAttackPower, isCriticalAttack);

	}

	public override void Heal(Character _attacker, IdleNumber _attackPower, string _healName, Color _color)
	{
		//base.Heal(_attacker, _attackPower, _color);
		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = info.hp + _attackPower;

			IdleNumber rawHP = info.rawHp;
			if (rawHP < newHP)
			{
				newHP = rawHP;
			}

			IdleNumber addHP = newHP - info.hp;
			info.hp += addHP;
			VGameManager.it.battleRecord.RecordHeal(_attacker.charID, charID, _healName, addHP);
			GameUIManager.it.ShowFloatingText(_attackPower.ToString(), _color, characterAnimation.CenterPivot.position, false);
		}
	}
}
