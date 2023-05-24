using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIManagementBaseInfo<T> : MonoBehaviour
{

	public abstract void OnUpdate(UIManagementEquip _parent, T _info);
}
