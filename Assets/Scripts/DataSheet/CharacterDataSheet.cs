[System.Serializable]
public class CharacterDataSheet : DataBase<CharacterData>
{
	public CharacterData GetData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}
		return null;
	}
}


