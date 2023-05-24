using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class UnitFacial : MonoBehaviour
{
	[SerializeField] private SpriteResolver[] resolvers;
	public void ChangeFacial(int index)
	{
		for (int i = 0; i < resolvers.Length; i++)
		{
			resolvers[i].SetCategoryAndLabel(resolvers[i].GetCategory(), index.ToString());
		}
	}
}
