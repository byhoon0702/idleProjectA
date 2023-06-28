using System;
using System.Collections.Generic;

[Serializable]
public class SoundEffectData : BaseData
{
	public string key;
	public string resource;
}

[Serializable]
public class SoundEffectDataSheet : DataSheetBase<SoundEffectData>
{


}
