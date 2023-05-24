using System.Collections;

using UnityEngine;
[CreateAssetMenu(fileName = "Dash", menuName = "Skill/Ability/Dash", order = 1)]
public class Dash : SkillAbility
{
	public float power;
	public bool toTarget;

	private float time;
	protected WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
	public override IEnumerator DO(Transform parent, Unit caster, Vector3 targetPos, AffectedInfo affectedInfo, System.Action onComplete)
	{
		float time = 0;
		GameObject go = null;
		if (effect != null)
		{
			go = Instantiate(effect);
			go.transform.SetParent(parent);
			go.transform.position = caster.CenterPosition;
			go.transform.forward = caster.headingDirection;
		}

		//caster.ChangeState(StateType.DASH, true);

		Vector2 relativePos = targetPos * 0.9f;
		if (caster.target != null)
		{
			relativePos = caster.target.position * 0.9f;
		}
		Vector2 dir = (relativePos - caster.rigidbody2D.position).normalized;
		float distance = (relativePos - caster.rigidbody2D.position).magnitude;
		float duration = distance / power;

		Vector2 position = caster.rigidbody2D.position;
		while (time < duration)
		{
			caster.rigidbody2D.position = Vector2.Lerp(position, relativePos, time / duration);
			caster.PlayAnimation("dash", time / duration);
			if (go != null)
			{
				go.transform.position = caster.rigidbody2D.position;
			}

			yield return waitForFixedUpdate;
			time += Time.deltaTime;
		}

		caster.ChangeState(StateType.IDLE, true);

		onComplete?.Invoke();
	}

}
