using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum ProjectileType
{
	GUIDED,
	STRAIGHT,
	PARABOLA,
	BEZIER
}

public class Projectile : MonoBehaviour
{
	[Header("===Common===")]
	public bool changeBehavior;
	//추후 변경될 변수
	public Transform projectileView;
	public ProjectileType projectileType;

	public float speed;
	public float angle;
	public float duration;
	public float vy;
	public Vector3 velocityXZ;
	[Header("===Common===")]

	[Space(1)]

	[Header("===Bezier===")]
	public Vector3[] points = new Vector3[4];
	public Vector3 size;
	[Header("===Bezier===")]

	protected IdleNumber attackPower;
	protected UnitBase attacker;
	protected UnitBase targetCharacter;
	protected ProjectileBehavior behavior;
	protected Vector3 targetPos;

	protected Vector3 lookRotation;
	protected float gravity = 9.8f;
	protected float distance;

	protected float vx;

	public float elapsedTime = 0;
	protected bool dontmove = false;

	protected Transform cachedTransform;

	public virtual void Spawn(Transform origin, UnitBase _attacker, UnitBase _targetCharacter, IdleNumber _attackPower)
	{
		if (_attacker is EnemyCharacter)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			transform.localScale = Vector3.one;
		}

		transform.position = origin.position;
		attacker = _attacker;
		targetCharacter = _targetCharacter;

		attackPower = _attackPower;
		targetPos = _targetCharacter.characterAnimation.CenterPivot.position;
		dontmove = false;
		cachedTransform = transform;

		if (changeBehavior)
		{
			int i = Random.Range(0, 2);
			if (i == 0)
			{
				behavior = ProjectileManager.it.projectileBehaviorGroup.Call(ProjectileType.STRAIGHT);
			}
			else
			{
				behavior = ProjectileManager.it.projectileBehaviorGroup.Call(ProjectileType.PARABOLA);
			}
		}
		else
		{
			behavior = ProjectileManager.it.projectileBehaviorGroup.Call(projectileType);
		}
		behavior.Init(this, origin.position, targetPos);
	}

	//public virtual void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber _attackPower)
	//{
	//	if (_attacker is EnemyCharacter)
	//	{
	//		transform.localScale = new Vector3(-1, 1, 1);
	//	}
	//	else
	//	{
	//		transform.localScale = Vector3.one;
	//	}
	//	transform.position = origin;
	//	attackPower = _attackPower;
	//	dontmove = false;
	//	cachedTransform = transform;
	//}

	protected bool CheckHitDistance()
	{
		if (cachedTransform == null)
		{
			return false;
		}
		float distance = Mathf.Abs(targetCharacter.position.x - cachedTransform.position.x);

		if (distance < 0.5f)
		{
			return true;
		}

		return false;
	}
	public virtual void ReachedDestination()
	{
		if (targetCharacter == null)
		{
			return;
		}

		if (targetCharacter.IsAlive() == false)
		{
			gameObject.SetActive(false);
			return;
		}

		Color color = Color.red;
		if (attacker is PlayerCharacter)
		{
			color = Color.white;
		}

		SkillUtility.SimpleAttack(attacker, targetCharacter, AttackType.RANGED, attackPower, color, _hitSound: attacker.defaultAttackSoundPath);

		gameObject.SetActive(false);

		dontmove = true;
		elapsedTime = 0;
	}

	public virtual void OnMove(Vector3 velocity)
	{

	}

	protected void Update()
	{
		if (dontmove)
		{
			return;
		}

		if (CheckHitDistance())
		{
			ReachedDestination();
			return;
		}
		if (elapsedTime < duration)
		{
			behavior.OnUpdate(this, elapsedTime, Time.deltaTime);
			//elapsedTime += Time.deltaTime;
		}

		else
		{
			ReachedDestination();
		}
	}

	protected float FourPointBezierCurve(float _p1, float _p2, float _p3, float p4, float t)
	{
		float a = Mathf.Pow((1 - t), 3) * _p1;
		float b = Mathf.Pow((1 - t), 2) * 3 * t * _p2;
		float c = Mathf.Pow(t, 2) * 3 * (1 - t) * _p3;
		float d = Mathf.Pow(t, 3) * p4;

		return a + b + c + d;
	}
}
