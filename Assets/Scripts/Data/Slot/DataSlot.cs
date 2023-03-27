using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 데이터 슬롯
public abstract class DataSlot
{
	public abstract void AddEquipValue(ref IdleNumber _value, Ability _type);
	public abstract void AddOwnedValue(ref IdleNumber _value, Ability _type);

}
