using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 데이터 슬롯
public abstract class DataSlot
{

	public abstract void AddModifiers(UserDB userDB);
	public abstract void RemoveModifiers(UserDB userDB);
}
