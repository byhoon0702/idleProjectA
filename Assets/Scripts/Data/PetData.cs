using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PetData : BaseData
{
	public string name;
	public string resource;
	public long skillEffectTidNormal;
	public long skillTid;
	public Grade grade;
	public int starlevel;
	public long attackPower;
	public float attackSpeed;
	public string attackSound;
	public float moveSpeed;
	public string projectileResource;
	public PetCategory category;


	public PetData Clone()
	{
		PetData data = new PetData();
		data.name = name;
		data.resource = resource;
		data.skillTid = skillTid;
		data.grade = grade;
		data.starlevel = starlevel;
		data.attackPower = attackPower;
		data.attackSpeed = attackSpeed;
		data.attackSound = attackSound;
		data.projectileResource = projectileResource;
		data.moveSpeed = moveSpeed;
		data.skillEffectTidNormal = skillEffectTidNormal;
		return data;

	}
}
