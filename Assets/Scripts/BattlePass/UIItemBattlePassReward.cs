using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class UIItemBattlePassReward : MonoBehaviour
{

	[SerializeField] protected UIItemReward uiReward;
	public abstract void SetData();

}
