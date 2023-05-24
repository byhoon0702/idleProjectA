using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class NewSkill : ScriptableObject
{
	[SerializeField] private long tid;
	public long Tid => tid;
	public Sprite Icon;

	[SerializeField] private LayerMask layer;
	public LayerMask Layer => layer;

	[SerializeField] private TargetingType targetingType;
	public TargetingType TargetingType => targetingType;


	public string ui_Description;


	[SerializeField] private GameObject obj;

	[SerializeField] private StateType allowedUnitState = StateType.NONE;
	public StateType AllowedUnitState => allowedUnitState;
	public bool self;

#if UNITY_EDITOR
	public void SetEditorData(SkillData data)
	{
		ui_Description = data.ui_Description;
		tid = data.tid;
	}
#endif

	public bool Trigger(Unit _caster, RuntimeData.SkillInfo _info, HitInfo hitInfo)
	{
		if (_caster == null || _caster.target == null)
		{
			return false;
		}
		if (allowedUnitState != StateType.NONE)
		{
			if (allowedUnitState.HasFlag(_caster.currentState) == false)
			{
				return false;
			}
		}

		if (_info.skillAbility.Value > 0)
		{
			hitInfo.TotalAttackPower *= _info.Value / 100f;
		}

		ShowEffect(_caster, self);
		_caster.StartCoroutine(Attack(_caster, _info, hitInfo));
		return true;
	}
	private void ShowEffect(Unit caster, bool isSelf)
	{
		if (obj == null)
		{
			return;
		}
		GameObject go = Instantiate(obj);
		if (isSelf)
		{
			go.transform.SetParent(caster.transform);
		}

		go.transform.position = isSelf ? caster.position : caster.target.position;
	}

	private IEnumerator Attack(Unit caster, RuntimeData.SkillInfo info, HitInfo hitInfo)
	{
		int attackCount = 0;
		if (targetingType == TargetingType.NONTARGETING)
		{
			Vector3 pos = caster.target.position;
			while (attackCount < info.HitCount)
			{
				var colliders = Physics2D.OverlapCircleAll(pos, info.HitRange, layer);

				for (int i = 0; i < colliders.Length; i++)
				{
					Unit target = colliders[i].GetComponent<Unit>();
					target.Hit(hitInfo);

					if (target.IsAlive() == false)
					{
						caster.killCount++;
					}
				}

				attackCount++;
				yield return new WaitForSeconds(info.Interval);
			}
		}
		else
		{
			if (caster.target == null)
			{
				yield break;
			}
			if (Vector3.Distance(caster.target.position, caster.position) <= info.HitRange)
			{
				while (attackCount < info.HitCount)
				{
					if (caster.target != null)
					{
						caster.target.Hit(hitInfo);

						if (caster.target.IsAlive() == false)
						{
							caster.killCount++;
						}
					}
					attackCount++;
					yield return new WaitForSeconds(info.Interval);
				}

			}
		}
		yield return null;
	}
}
