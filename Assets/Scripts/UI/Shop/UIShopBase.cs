using System.Collections.Generic;
using UnityEngine;

public abstract class UIShopBase : MonoBehaviour
{
	[SerializeField] protected GameObject itemPrefab;
	[SerializeField] protected Transform content;


	protected List<RuntimeData.ShopInfo> infoList;
	protected ShopType currentType;
	public ShopType CurrentType => currentType;
	public abstract void OnUpdate(ShopType type);
	public abstract void Refresh();

	protected abstract void SetGrid();
	public abstract void ShowBuyPopup(RuntimeData.ShopInfo info);
}
