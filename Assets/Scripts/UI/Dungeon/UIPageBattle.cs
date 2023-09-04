using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPageBattle : MonoBehaviour
{
	protected UIManagementBattle parent;
	public abstract void OnUpdate(UIManagementBattle _parent);

}
