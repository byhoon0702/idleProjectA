using UnityEngine;

public interface IData
{
	object GetData();
}
[System.Serializable]
public class UnitDataSheetObject : ScriptableObject
{
	[SerializeField]
	public UnitDataSheet dataSheet;


}
