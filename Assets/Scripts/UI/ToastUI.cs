using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastUI : MonoBehaviour
{
	public static ToastUI it { get; private set; }
	[SerializeField] private ItemToast prefab;
	[SerializeField] private RectTransform itemRoot;


	private void Awake()
	{
		it = this;
	}

	public void Create(string text)
	{
		var toast = Instantiate(prefab, itemRoot);
		toast.transform.SetAsFirstSibling();
		toast.OnUpdate(text);
	}
}
