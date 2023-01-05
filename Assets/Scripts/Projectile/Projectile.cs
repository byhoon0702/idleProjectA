using UnityEngine;

public class Projectile : MonoBehaviour
{
	//추후 변경될 변수
	public SpriteRenderer projectileView;
	public IdleNumber damage;
	public Character attacker;
	public Character targetCharacter;
	protected Vector3 targetPos;

	protected Vector3 lookRotation;
	protected float gravity = 9.8f;
	protected float distance;


	protected float duration;
	protected float vx;
	protected float vy;
	protected float elapsetimd = 0;
	protected bool dontmove = false;
	public virtual void Spawn(Transform origin, Character _attacker, Character _targetCharacter, IdleNumber _damage)
	{
		transform.position = origin.position;
		attacker = _attacker;
		targetCharacter = _targetCharacter;

		damage = _damage;
		targetPos = _targetCharacter.characterAnimation.CenterPivot.position;
		//Vector2 dir = targetPos - origin.position;
		//var dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//lookRotation = new Vector3(0, 0, dirAngle);
		//transform.eulerAngles = lookRotation;
	}
	public virtual void Spawn(Vector3 origin, Vector3 targetPos, IdleNumber damage)
	{
		transform.position = origin;
		this.damage = damage;
		dontmove = false;
		//Vector2 dir = targetPos - origin;
		//var dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//lookRotation = new Vector3(0, 0, dirAngle);
		//transform.eulerAngles = lookRotation;
	}

	private void Start()
	{

	}

	public virtual void ReachedDestination()
	{
		if (targetCharacter == null)
		{
			return;
		}

		if (targetCharacter.IsAlive() == false)
		{
			Destroy(gameObject);
			return;
		}

		SkillUtility.SimpleDamage(attacker, targetCharacter, damage, Color.white);

		Destroy(gameObject);
	}

	// Update is called once per frame
}
