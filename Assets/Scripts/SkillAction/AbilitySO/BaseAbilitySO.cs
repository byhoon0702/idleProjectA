using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAbilitySO : ScriptableObject
{
	public Sprite iconResource;
	public GameObject characterEffect;
	public List<Stats> affectStatList = new List<Stats>();

	public SkillEffectActionSO skillEffectActionSO;
	public List<TargetFilterBehaviorSO> targetFilterBehavior;
	public List<SkillEffectInfo> infoList = new List<SkillEffectInfo>();

	public abstract void OnUpdate(SkillEffectObject skillEffectObj, float time);
	public virtual void OnSet(SkillEffectObject skillEffectObj, IdleNumber power)
	{
		skillEffectObj.index = 0;
		if (characterEffect == null)
		{
			return;
		}
		GameObject go = (GameObject)Instantiate(characterEffect);

		Vector3 spawnPos = skillEffectObj.pos;
		if (skillEffectObj.caster != null)
		{
			spawnPos = skillEffectObj.caster.position;
		}

		go.transform.position = spawnPos;
		go.transform.localScale = Vector3.one * 0.7f;
		FxSpriteEffectAuto fxsprite = go.GetComponent<FxSpriteEffectAuto>();
		if (fxsprite != null)
		{
			fxsprite.Play(() => { Destroy(go); });
		}
	}
	public abstract void OnTrigger(SkillEffectObject skillEffectObj);

	public abstract void OnTrigger(SkillEffectInfo info, Unit targets, AffectedInfo hitinfo);

}
