using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RuntimeData
{
	public class HyperClassInfo : ModifyInfo
	{

		public HyperClassInfo(RuntimeData.AbilityInfo ability)
		{
			BaseValue = ability.Value;
		}



	}
}
