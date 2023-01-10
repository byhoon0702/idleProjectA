using UnityEngine;

public interface IData
{
	object GetData();
}
[System.Serializable]
public class CharacterDataSheetObject : ScriptableObject
{
	[SerializeField]
	public CharacterDataSheet dataSheet;


}
