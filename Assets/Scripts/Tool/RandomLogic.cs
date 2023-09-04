using System;
using Unity.VisualScripting;

public static class RandomLogic
{
	public const int maxChance = 10000;
	public static Random Reward { get; } = new Random(21);
	public static Random RewardBox { get; } = new Random(31);
	public static Random Critical { get; } = new Random(10);

	public static Random Gacha { get; } = new Random(100);

	public static int Get(this Random r, int min = 0, int max = maxChance)
	{
		return r.Next(min, max);
	}

}
