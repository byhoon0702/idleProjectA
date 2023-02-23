using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
	// Start is called before the first frame update
	public List<int> originalOrderList = new List<int>();
	public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
	void Start()
	{
		GetComponentsInChildren<SpriteRenderer>(true, renderers);

		for (int i = 0; i < renderers.Count; i++)
		{
			originalOrderList.Add(renderers[i].sortingOrder);
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < renderers.Count; i++)
		{
			renderers[i].sortingOrder = originalOrderList[i] - (int)((transform.localPosition.z * 100) * 3);
		}
	}
}
