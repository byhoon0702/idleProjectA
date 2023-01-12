using UnityEngine;

using asd = System.Int32;

public class PlayerCharacter : Character
{
	public int side;
	Vector3 moveDirection;
	RaycastHit raycastHit;
	public override void Spawn(CharacterData _data)
	{
		rawData = _data;
		info = new CharacterInfo(this, _data, ControlSide.PLAYER);
		side = 1;
		SetCharacterClass();

		if (characterView == null)
		{
			var model = UnitModelPoolManager.it.GetModel(("B/" + this.info.data.resource));
			model.transform.SetParent(transform);
			model.transform.localPosition = Vector3.zero;
			model.transform.localScale = Vector3.one * 0.003f;
			var cam = SceneCamera.it.sceneCamera;
			model.transform.LookAt(model.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
			characterView = model;

			gameObject.AddComponent<SphereCollider>();
			gameObject.tag = "Player";
			gameObject.name = info.charNameAndCharId;
		}
		Init();
	}
	float elapsedTime = 0;
	private Vector3 SimpleCrowdAI(float _deltatime)
	{
		moveDirection = Vector3.right * side;
		//if (elapsedTime < 0.1f)
		//{
		//	elapsedTime += _deltatime;
		//	return moveDirection;
		//}
		//elapsedTime = 0;
		//오른쪽
		//if (Physics.Raycast(transform.position, Vector3.right * side, out raycastHit, 1))
		//{
		//	if (raycastHit.transform.tag == transform.tag)
		//	{
		//		moveDirection = Vector3.zero;
		//	}
		//}
		////왼쪽
		//if (Physics.Raycast(transform.position, Vector3.left * side, out raycastHit, 1))
		//{
		//	if (raycastHit.transform.tag == transform.tag)
		//	{

		//	}
		//}
		////Z+
		//if (Physics.Raycast(transform.position, Vector3.forward, out raycastHit, 1))
		//{
		//	if (raycastHit.transform.tag == transform.tag)
		//	{
		//		moveDirection += Vector3.back;
		//	}
		//}
		////Z-
		//if (Physics.Raycast(transform.position, Vector3.back, out raycastHit, 1))
		//{
		//	if (raycastHit.transform.tag == transform.tag)
		//	{
		//		moveDirection += Vector3.forward;
		//	}
		//}

		return moveDirection;
	}


	public override void Move(float _delta)
	{

		moveDirection = SimpleCrowdAI(_delta);

		transform.Translate(moveDirection * info.MoveSpeed() * _delta);
	}

	public override void Hit(Character _attacker, IdleNumber _attackPower, string _attackName, Color _color, float _criticalChanceMul = 1)
	{
		IdleNumber totalAttackPower = _attackPower * _criticalChanceMul * _attacker.info.DamageMul();
		bool isCriticalAttack = _criticalChanceMul > 1;

		if (info.data.hp > 0)
		{
			GameUIManager.it.ShowFloatingText(totalAttackPower.ToString(), _color, characterAnimation.CenterPivot.position, isCriticalAttack, isPlayer: true);

		}
		info.data.hp -= totalAttackPower;

		VGameManager.it.battleRecord.RecordAttackPower(_attacker.charID, charID, _attackName, totalAttackPower, isCriticalAttack);
	}

	public override void Heal(Character _attacker, IdleNumber _attackPower, string _healName, Color _color)
	{
		//base.Heal(_attacker, _attackPower, _healName, _color);
		if (currentState != StateType.DEATH)
		{
			IdleNumber newHP = info.data.hp + _attackPower;

			if (rawData.hp.GetValue() < newHP.GetValue())
			{
				newHP = rawData.hp;
			}

			IdleNumber addHP = newHP - info.data.hp;
			info.data.hp += addHP;
			VGameManager.it.battleRecord.RecordHeal(_attacker.charID, charID, _healName, addHP);
			GameUIManager.it.ShowFloatingText(_attackPower.ToString(), _color, characterAnimation.CenterPivot.position, false, isPlayer: true);
		}
	}
}
