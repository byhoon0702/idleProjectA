using System;
using System.Collections.Generic;

[Serializable]
public class GachaEntryData : BaseData
{
	public GachaEntryProbabilityInfo[] probabilities;
}

[Serializable]
public class GachaEntryProbabilityInfo
{
	public Grade grade;
	public double levelRatio_1;
	public double levelRatio_2;
	public double levelRatio_3;
	public double levelRatio_4;
	public double levelRatio_5;
	public double levelRatio_6;
	public double levelRatio_7;
	public double levelRatio_8;
	public double levelRatio_9;
	public double levelRatio_10;

	public double GetRatio(int _level)
	{
		switch (_level)
		{
			case 1:
				return levelRatio_1;
			case 2:
				return levelRatio_2;
			case 3:
				return levelRatio_3;
			case 4:
				return levelRatio_4;
			case 5:
				return levelRatio_5;
			case 6:
				return levelRatio_6;
			case 7:
				return levelRatio_7;
			case 8:
				return levelRatio_8;
			case 9:
				return levelRatio_9;
			case 10:
				return levelRatio_10;
		}

		return 0;
	}
}

[Serializable]
public class GachaEntryDataSheet : DataSheetBase<GachaEntryData>
{

}
