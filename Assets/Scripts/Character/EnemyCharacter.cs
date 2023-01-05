using UnityEngine;

public class EnemyCharacter : Character
{
	public override void Spawn(CharacterData data)
	{
		rawData = data;
		info = new CharacterInfo(this, data, ControlSide.ENEMY);

		SetCharacterClass();

		if (characterView == null)
		{
			var model = Instantiate(Resources.Load($"B/{data.resource}")) as GameObject;
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one * 0.003f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			characterView = model;
		}
		Init();
	}

	public override void Move(float delta)
	{
		transform.Translate(Vector3.left * info.MoveSpeed() * delta);
	}

	public override void Hit(Character _attacker, IdleNumber _damage, Color _color, float _criticalMul = 1)
	{
		IdleNumber totalDamage = _damage * _criticalMul;
		bool isCriticalAttack = _criticalMul > 1;

		if (info.data.hp > 0)
		{
			SceneCamera.it.ShakeCamera();
			GameUIManager.it.ShowFloatingText(totalDamage.ToString(), _color, characterAnimation.CenterPivot.position, isCriticalAttack);
		}
		info.data.hp -= totalDamage;

		GameManager.it.battleRecord.RecordDamage(_attacker.charID, totalDamage, isCriticalAttack);

	}

	public override void Heal(Character _attacker, IdleNumber _damage, Color _color)
	{
		base.Heal(_attacker, _damage, _color);
		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = info.data.hp + _damage;

			if (rawData.hp.GetValue() < newHP.GetValue())
			{
				newHP = rawData.hp;
			}

			IdleNumber addHP = newHP - info.data.hp;
			info.data.hp += addHP;
			GameManager.it.battleRecord.RecordHeal(_attacker.charID, addHP);
			GameUIManager.it.ShowFloatingText(_damage.ToString(), _color, characterAnimation.CenterPivot.position, false);
		}
	}
}
