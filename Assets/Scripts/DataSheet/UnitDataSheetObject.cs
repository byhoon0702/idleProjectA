using UnityEngine;

public interface IData
{
	object GetData();
}
[System.Serializable]
public class UnitDataSheetObject : BaseDataSheetObject
{
	[SerializeField]
	public UnitDataSheet dataSheet;

}
