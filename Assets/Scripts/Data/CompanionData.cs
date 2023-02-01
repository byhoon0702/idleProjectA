using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompanionData : BaseData
{
	public string name;
	public string resource;
	public long skillTid;
	public Grade grade;
	public int starlevel;
	public long attackPower;
	public float attackSpeed;
	public string attackSound;
	public string projectileResource;

	public CompanionData Clone()
	{
		CompanionData data = new CompanionData();
		data.name = name;
		data.resource = resource;
		data.skillTid = skillTid;
		data.grade = grade;
		data.starlevel = starlevel;
		data.attackPower = attackPower;
		data.attackSpeed = attackSpeed;
		data.attackSound = attackSound;
		data.projectileResource = projectileResource;
		return data;

	}
}
