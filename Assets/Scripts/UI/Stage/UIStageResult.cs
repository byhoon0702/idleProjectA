using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIStageResult : UIBase
{
	protected UIStageClear parent;
	protected StageRule rule;
	public virtual void Init(UIStageClear _parent)
	{
		parent = _parent;
	}
	public virtual void Show(StageRule _rule)
	{
		rule = _rule;
	}

	protected override void OnClose()
	{

	}
}
