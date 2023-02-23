using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using h_story_group_meta;
using log4net.Util;
using UnityEngine;
using UnityEngine.Rendering.UI;

[CreateAssetMenu(fileName = "Attack Skill", menuName = "ScriptableObject/Skill/Attack", order = 2)]
public class AttackAbilitySO : BaseAbilitySO
{
	public override void OnSet(SkillEffectObject skillEffectObj, IdleNumber power)
	{
		base.OnSet(skillEffectObj, power);

		if (skillEffectObj.caster != null)
		{
			int count = 1;
			if (skillEffectObj.baseAbilitySO.infoList != null && skillEffectObj.baseAbilitySO.infoList.Count > 0)
			{
				count = skillEffectObj.baseAbilitySO.infoList.Count;
			}
			var hitinfo = SkillUtility.CreateHitInfo(skillEffectObj.caster, false, power / count, skillEffectObj.caster.defaultAttackSoundPath);
			hitinfo.attackerType = AttackerType.Player;
			skillEffectObj.SetAffectedInfo(hitinfo);
			skillEffectObj.transform.position = skillEffectObj.caster.target.position;
		}
		else
		{
			int count = 1;
			if (skillEffectObj.baseAbilitySO.infoList != null && skillEffectObj.baseAbilitySO.infoList.Count > 0)
			{
				count = skillEffectObj.baseAbilitySO.infoList.Count;
			}
			var hitinfo = new HitInfo(AttackerType.Player, power / count);
			hitinfo.attackerType = AttackerType.Player;
			skillEffectObj.SetAffectedInfo(hitinfo);
		}
	}

	public override void OnUpdate(SkillEffectObject skillEffectObj, float time)
	{
		skillEffectActionSO?.OnUpdate(skillEffectObj, time);
	}

	public override void OnTrigger(SkillEffectObject skillEffectObj)
	{

	}

	public override void OnTrigger(SkillEffectInfo info, Unit targets, AffectedInfo hitinfo)
	{
		if (info.damageCount > 1)
		{
			MultiHit(info, targets, (HitInfo)hitinfo);
		}
		else
		{
			if (targets.IsAlive())
			{
				targets.Hit((HitInfo)hitinfo);
			}
		}

	}

	async void MultiHit(SkillEffectInfo info, UnitBase targets, HitInfo hitInfo)
	{

		int damageCount = info.damageCount > 0 ? info.damageCount : 1;
		IdleNumber result = hitInfo.TotalAttackPower / damageCount;

		HitInfo newHitInfo = new HitInfo(hitInfo.attackerType, result);

		for (int i = 0; i < damageCount; i++)
		{
			targets.Hit(hitInfo);
			await Task.Delay((int)(1000 * info.damageInterval));
		}
	}
}
