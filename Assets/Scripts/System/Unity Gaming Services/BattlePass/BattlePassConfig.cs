namespace BattlePass
{
	public struct BattlePassConfig
	{
		public RewardDetail[] rewardsFree;
		public RewardDetail[] rewardsPremium;
		public int seasonXpPerTier;
		public int battlePassTierCount;
		public string eventName;
	}
}

public struct RewardDetail
{
	public long tid;
	public string quantity;

}
