using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillDictionary
{
	public static Dictionary<string, object[]> dic = new Dictionary<string, object[]>() 
	{
		{ typeof(Serina_sk1).ToString(), new object[] { new Serina_sk1Data(new AttackSpeedUpConditionData(0.14f, 5), new MoveSpeedUpConditionData(0.21f, 5), 5.21f, 10) } } ,
		{ typeof(Landrock_sk1).ToString(), new object[] { new Landrock_sk1Data(new KnockbackConditionData(0.5f, 1), new ReducedDefenseConditionData(0.05f, 4), 3, 1.12f, 5) } },
		{ typeof(Mirfiana_sk1).ToString(), new object[] { new Mirfiana_sk1Data(new StunConditionData(2), 11.42f, 8) } },
		{ typeof(Haru_sk1).ToString(), new object[] { new Haru_sk1Data(new PoisonConditionData(0.4f, 5), 3.39f, 0.5f, 7) } },
		{ typeof(Gilius_sk1).ToString(), new object[] { new Gilius_sk1Data(new AttackPowerUpConditionData(0.084f, 6), new CriticalUpConditionData(0.035f, 6), 5.46f, 10) } }
	};
}
