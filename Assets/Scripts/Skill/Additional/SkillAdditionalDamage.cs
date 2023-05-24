using System;
using UnityEngine;

[Serializable]
public class AdditionalDamageInfo
{
	public bool useCasterInfo;
	public Sprite icon;
	public float duration;
	public int damageCount;
	public string fourArithmetics;
	//public Stats[] calculator;
	public GameObject particle;
}

/// <summary>
/// 추가 데미지
/// </summary>
[CreateAssetMenu(fileName = "Skill Addtional Damage Effect", menuName = "Skill/Status Effect/Additional Damage")]
[Serializable]
public class SkillAdditionalDamage : ScriptableObject
{
	//선딜레이를 가지는 추가 데미지
	public AdditionalDamageInfo info;

	public void ApplyEffect(HittableUnit caster, HittableUnit target)
	{
		IdleNumber power = new IdleNumber(0);
		if (info.useCasterInfo)
		{
			power = caster.AttackPower;
		}
		else
		{
			power = target.AttackPower;
		}
		HitInfo hitinfo = new HitInfo(caster, power);
		target.AdditionalDamage(info, hitinfo);
	}
}
