using System.Collections.Generic;
using UnityEngine;

public abstract class UIShopBase<T> : MonoBehaviour where T : RuntimeData.ShopInfo
{
	[SerializeField] protected GameObject itemPrefab;
	[SerializeField] protected Transform content;


	protected List<T> infoList;
	protected ShopType currentType;
	public ShopType CurrentType => currentType;
	public abstract void OnUpdate(ShopType type);
	public abstract void Refresh();

	protected abstract void SetGrid();
	public virtual void ShowBuyPopup(T info) { }
}
