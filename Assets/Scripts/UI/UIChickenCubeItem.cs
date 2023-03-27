using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Drawing;
using UnityEngine.EventSystems;

public enum ChickenCubeType
{
	NONE,
	CRACKED,
	SOLID,
}

public class UIChickenCubeItem : MonoBehaviour
{
	[SerializeField] private GameObject crackedImage;
	[SerializeField] private GameObject solidImage;

	[SerializeField] private Image rewardItemIcon;
	[SerializeField] private Image shadeImage;

	[SerializeField] private Image selectEnabledImage;
	[SerializeField] private Image selectDisabledImage;

	[SerializeField] private Image specialCubeImage;

	private Point point;
	private ChickenCubeType type;
	private int hp;
	private ItemData rewardItem;

	private int cubeWidth = 150;
	private int cubeHeight = 150;

	private UIGrandpaAndChickenTree owner;

	private IObjectPool<UIChickenCubeItem> managedPool;

	public ChickenCubeType CubeType => type;

	public bool IsShaded
	{
		get
		{
			return shadeImage.gameObject.activeSelf;
		}
	}

	public bool IsSelected
	{
		get
		{
			return selectEnabledImage.gameObject.activeSelf;
		}
	}

	public int X => point.X;
	public int Y => point.Y;

	public void SetManagedPool(IObjectPool<UIChickenCubeItem> _managedPool)
	{
		managedPool = _managedPool;
	}

	public void SetData(UIGrandpaAndChickenTree _onwer, Point _point, ChickenCubeType _type, int _hp = 0, long _rewardItemTid = 0)
	{
		owner = _onwer;
		point = _point;
		type = _type;

		SetHP(_hp);
		SetPosition();
		SetCubeImage();
		CreateRewardItem(_rewardItemTid);
		ShowRewardIcon();
	}

	public void SetRewardItem(long _rewardItemTid)
	{
		CreateRewardItem(_rewardItemTid);
		ShowRewardIcon();
	}


	private void SetHP(int _hp = 0)
	{
		if (_hp != 0)
		{
			hp = _hp;
		}
		else
		{
			if (type == ChickenCubeType.CRACKED)
			{
				hp = 1;
			}
			if (type == ChickenCubeType.SOLID)
			{
				hp = 2;
			}
		}
	}

	private void SetPosition()
	{
		transform.localPosition = new Vector3(cubeWidth * point.X, -cubeHeight * point.Y);
	}

	private void SetCubeImage()
	{
		crackedImage.gameObject.SetActive(type == ChickenCubeType.CRACKED);
		solidImage.gameObject.SetActive(type == ChickenCubeType.SOLID);
	}

	private void CreateRewardItem(long _rewardItemTid)
	{
		if (type == ChickenCubeType.SOLID)
		{
			return;
		}

		if (_rewardItemTid != 0)
		{
			// 수동으로 보상지정
			return;
		}


		// 확률로 리워드 생성.
	}

	private void ShowRewardIcon()
	{
		// 리워드가 생성됐다면 Image에 아이콘 로드.
	}

	public void SetShadeOn(bool _isOn)
	{
		shadeImage.gameObject.SetActive(_isOn);
	}

	// Shade Off 되었을때 true를 반환
	public bool RefreshShade()
	{
		// 이미 밝혀진 타일이 다시 어두워질 일은 없음.
		if (IsShaded == false)
		{
			return false;
		}

		var map = owner.Map;

		bool isCubeExist1 = map.ContainsKey(new Point(point.X - 1, point.Y))
			&& map[new Point(point.X - 1, point.Y)].type == ChickenCubeType.NONE
			&& map[new Point(point.X - 1, point.Y)].IsShaded == false;

		bool isCubeExist2 = map.ContainsKey(new Point(point.X + 1, point.Y))
			&& map[new Point(point.X + 1, point.Y)].type == ChickenCubeType.NONE
			&& map[new Point(point.X + 1, point.Y)].IsShaded == false;

		bool isCubeExist3 = map.ContainsKey(new Point(point.X, point.Y + 1))
			&& point.Y <= owner.FocusY
			&& map[new Point(point.X, point.Y + 1)].type == ChickenCubeType.NONE
			&& map[new Point(point.X, point.Y + 1)].IsShaded == false;

		bool isCubeExist4 = map.ContainsKey(new Point(point.X, point.Y - 1))
			&& map[new Point(point.X, point.Y - 1)].type == ChickenCubeType.NONE
			&& map[new Point(point.X, point.Y - 1)].IsShaded == false;

		bool isShadeOff = isCubeExist1 || isCubeExist2 || isCubeExist3 || isCubeExist4;
		bool result = isShadeOff == true && IsShaded == true;

		shadeImage.gameObject.SetActive(isShadeOff == false);

		return result;
	}

	public bool Hit(int _damage)
	{
		hp -= _damage;

		if (hp <= 0)
		{
			GetReward();
			type = ChickenCubeType.NONE;
			SetCubeImage();
			return true;
		}

		return false;
		// dltmdduq1118
		// 남은 hp에 따른 UI 변화 or 이미지 변화가 있는지 확인
	}

	private void GetReward()
	{
		if (rewardItem == null)
		{
			return;
		}

	}

	public bool Select(bool _selectForce = false, UIGrandpaAndChickenTree.DiggingTool _tool = UIGrandpaAndChickenTree.DiggingTool.Pickaxe)
	{
		if (IsSelected == true)
		{
			return true;
		}

		if (_selectForce == true)
		{
			selectEnabledImage.gameObject.SetActive(true);
			selectDisabledImage.gameObject.SetActive(false);
			return true;
		}

		bool isEnabled = false;
		if (_tool == UIGrandpaAndChickenTree.DiggingTool.Pickaxe)
		{
			isEnabled = IsShaded == false && type != ChickenCubeType.NONE;
		}
		else
		{
			isEnabled = IsShaded == false && type == ChickenCubeType.NONE;
		}

		selectEnabledImage.gameObject.SetActive(isEnabled == true);
		selectDisabledImage.gameObject.SetActive(isEnabled == false);

		return isEnabled;
	}

	public void Deselect()
	{
		selectEnabledImage.gameObject.SetActive(false);
		selectDisabledImage.gameObject.SetActive(false);
	}

	public void Release()
	{
		managedPool?.Release(this);
	}
}
