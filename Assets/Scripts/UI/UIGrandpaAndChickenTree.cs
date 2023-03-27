using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Drawing;
using TMPro;

public class UIGrandpaAndChickenTree : MonoBehaviour
{
	[SerializeField] private UIChickenCubeItem prefabObject;
	[SerializeField] private RectTransform cubeParentTransform;
	[SerializeField] private Button closeButton;

	[SerializeField] private ToggleGroup toggleGroup;

	[SerializeField] private Toggle pickaxeToggle;
	[SerializeField] private Toggle bombToggle;
	[SerializeField] private Toggle drillToggle;

	[SerializeField] private TextMeshProUGUI pickaxeText;
	[SerializeField] private TextMeshProUGUI bombText;
	[SerializeField] private TextMeshProUGUI drillText;

	[SerializeField] private GameObject specialImage2x2;
	[SerializeField] private GameObject specialImage3x2;

	private float EMPTY_CUBE_PERCENT = 10;
	private float CRACKED_CUBE_PERCENT = 70;
	private float SOLID_CUBE_PERCENT = 20;

	private IObjectPool<UIChickenCubeItem> cubePool;
	private UIChickenCubeItem selectedCube = null;

	private Dictionary<Point, UIChickenCubeItem> map = new Dictionary<Point, UIChickenCubeItem>();
	private List<UIChickenCubeItem> refreshedCubeList = new List<UIChickenCubeItem>();

	public Dictionary<Point, UIChickenCubeItem> Map => map;

	public DiggingTool currentTool = DiggingTool.Pickaxe;

	private bool isInitialized = false;

	private int SpecialY = 0;

	public int FocusY
	{
		get; private set;
	}

	public enum DiggingTool
	{
		None,
		Pickaxe,
		Bomb,
		Drill
	}

	private void Awake()
	{
		cubePool = new ObjectPool<UIChickenCubeItem>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(OnCloseButton);

		SetToggles();
	}

	private void OnCloseButton()
	{
		UIController.it.InactivateAllBottomToggle();
		gameObject.SetActive(false);
	}

	private void SetToggles()
	{
		drillToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (Inventory.it.FindItemByHashTag("drill").Count == 0)
			{
				currentTool = DiggingTool.None;
				drillToggle.isOn = false;
			}
			if (_isOn == true)
			{
				currentTool = DiggingTool.Drill;
			}
			else
			{
				currentTool = DiggingTool.None;
			}
		});

		pickaxeToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (Inventory.it.FindItemByHashTag("pickaxe").Count == 0)
			{
				currentTool = DiggingTool.None;
				pickaxeToggle.isOn = false;
			}
			if (_isOn == true)
			{
				currentTool = DiggingTool.Pickaxe;
			}
			else
			{
				currentTool = DiggingTool.None;
			}
		});

		bombToggle.onValueChanged.AddListener((_isOn) =>
		{
			if (Inventory.it.FindItemByHashTag("bomb").Count == 0)
			{
				currentTool = DiggingTool.None;
				bombToggle.isOn = false;
			}
			if (_isOn == true)
			{
				currentTool = DiggingTool.Bomb;
			}
			else
			{
				currentTool = DiggingTool.None;
			}
		});
	}

	private void UpdateItemCount()
	{
		IdleNumber pickaxeCount = Inventory.it.FindItemByHashTag("pickaxe").Count;
		IdleNumber bombCount = Inventory.it.FindItemByHashTag("bomb").Count;
		IdleNumber drillCount = Inventory.it.FindItemByHashTag("drill").Count;

		if (pickaxeCount == 0)
		{
			pickaxeToggle.isOn = false;
		}
		if (bombCount == 0)
		{
			bombToggle.isOn = false;
		}
		if (drillCount == 0)
		{
			drillToggle.isOn = false;
		}

		pickaxeText.text = $"곡괭이\nX{Inventory.it.FindItemByHashTag("pickaxe").Count.ToString()}";
		bombText.text = $"폭탄\nX{Inventory.it.FindItemByHashTag("bomb").Count.ToString()}";
		drillText.text = $"드릴\nX{Inventory.it.FindItemByHashTag("drill").Count.ToString()}";
	}

	public void OnUpdate()
	{
		if (isInitialized == false)
		{
			InitializeTree();
			isInitialized = true;
		}
		UpdateItemCount();
	}

	private void InitializeTree()
	{
		for (int y = 0; y < 7; y++)
		{
			MakeCubes(y, true);
		}
		for (int y = 7; y < 9; y++)
		{
			MakeCubes(y, false);
		}

		FocusY = 6;
		RefreshAllCubeShade();
	}

	private void MakeCubes(int _y, bool _isInit = false)
	{
		for (int x = 0; x < 6; x++)
		{
			var point = new Point(x, _y);

			// SpecialCube로 미리 생성된 경우가 있음
			if (map.ContainsKey(point) == true)
			{
				continue;
			}

			var type = SetRandomType();
			var cube = cubePool.Get();

			// 첫 생성시 빈 타일은 무조건 밝혀져 있도록 한다.
			if (type == ChickenCubeType.NONE && _isInit)
			{
				cube.SetShadeOn(false);
			}
			else
			{
				cube.SetShadeOn(true);
			}

			cube.SetData(this, point, type);
			map.Add(point, cube);
		}

		if (_isInit == false)
		{
			if (UnityEngine.Random.Range(0, 100) < 10)
			{
				// 아직 스페셜 보상이 남아있음.
				if (SpecialY >= FocusY - 6)
				{
					return;
				}

				if (UnityEngine.Random.Range(0, 100) < 50)
				{
					int randomX = UnityEngine.Random.Range(0, 5);
					Point point = new Point(randomX, _y);
					Make2x2SpecialCubes(point);
				}
				else
				{
					int randomX = UnityEngine.Random.Range(0, 4);
					Point point = new Point(randomX, _y);
					Make3x2SpecialCubes(point);
				}
			}
		}
	}

	private void Make2x2SpecialCubes(Point _point)
	{
		// dltmdduq1118
		// _point 위치의 타일에 bg 이미지 로드해야함. 어떤 이미지로 처리할것인지 확인 후 처리.

		var firstCube = GetCube(_point);
		specialImage2x2.transform.SetParent(cubeParentTransform);
		specialImage2x2.transform.SetAsFirstSibling();
		specialImage2x2.transform.localPosition = firstCube.transform.localPosition;
		specialImage2x2.gameObject.SetActive(true);

		for (int x = _point.X; x < _point.X + 2; x++)
		{
			for (int y = _point.Y; y < _point.Y + 2; y++)
			{
				Point point = new Point(x, y);
				map.TryGetValue(point, out var cube);
				if (cube == null)
				{
					cube = cubePool.Get();
					map.Add(point, cube);
				}
				cube.SetData(this, point, ChickenCubeType.NONE);
				if (point.Y == _point.Y + 1)
				{
					// dltmdduq1118
					// RewardItem 지정. 치킨큐브? 아이템 확정되면 해당 아이템으로 추가.
					// cube.SetRewardItem(0);
				}
			}
		}
		SpecialY = _point.Y + 1;
	}

	private void Make3x2SpecialCubes(Point _point)
	{
		// dltmdduq1118
		// _point 위치의 타일에 bg 이미지 로드해야함. 어떤 이미지로 처리할것인지 확인 후 처리.

		var firstCube = GetCube(_point);
		specialImage3x2.transform.SetParent(cubeParentTransform);
		specialImage3x2.transform.SetAsFirstSibling();
		specialImage3x2.transform.localPosition = firstCube.transform.localPosition;
		specialImage3x2.gameObject.SetActive(true);

		for (int x = _point.X; x < _point.X + 3; x++)
		{
			for (int y = _point.Y; y < _point.Y + 2; y++)
			{
				Point point = new Point(x, y);
				map.TryGetValue(point, out var cube);
				if (cube == null)
				{
					cube = cubePool.Get();
					map.Add(point, cube);
				}
				cube.SetData(this, point, ChickenCubeType.NONE);
				if (point.Y == _point.Y + 1 && point.X == _point.X + 1)
				{
					// dltmdduq1118
					// RewardItem 지정. 치킨큐브? 아이템 확정되면 해당 아이템으로 추가.
					// cube.SetRewardItem(0);
				}
			}
		}
		SpecialY = _point.Y + 1;
	}

	private void Focus(int _y)
	{
		FocusY = _y;
		cubeParentTransform.anchoredPosition = new Vector3(0, 150 * (FocusY - 6), 0);

		var keysToRemove = new List<Point>();
		foreach (var item in map)
		{
			var cube = item.Value;
			if (cube.Y < FocusY - 6)
			{
				keysToRemove.Add(item.Key);
				cube.Release();
			}
		}

		foreach (var key in keysToRemove)
		{
			map.Remove(key);
		}
	}

	private void RefreshAllCubeShade()
	{
		foreach (var cube in map.Values)
		{
			if (cube.CubeType == ChickenCubeType.NONE)
			{
				RefreshCubeShade(cube);
			}
		}
		foreach (var cube in map.Values)
		{
			if (cube.CubeType != ChickenCubeType.NONE)
			{
				RefreshCubeShade(cube);
			}
		}
		refreshedCubeList.Clear();
	}

	public void RefreshCubeShade(Point _point)
	{
		map.TryGetValue(_point, out var cube);
		if (cube != null)
		{
			RefreshCubeShade(cube);
		}
	}

	public void RefreshCubeShade(UIChickenCubeItem _cube)
	{
		if (_cube != null)
		{
			if (refreshedCubeList.Contains(_cube) == false)
			{
				if (_cube.RefreshShade() == true)
				{
					refreshedCubeList.Add(_cube);

					Point point = new Point(_cube.X, _cube.Y);
					RefreshCubeShade(new Point(point.X - 1, point.Y));
					RefreshCubeShade(new Point(point.X + 1, point.Y));
					RefreshCubeShade(new Point(point.X, point.Y - 1));
					RefreshCubeShade(new Point(point.X, point.Y + 1));
				}
			}
		}
	}

	private ChickenCubeType SetRandomType()
	{
		float totalPercent = EMPTY_CUBE_PERCENT + CRACKED_CUBE_PERCENT + SOLID_CUBE_PERCENT;
		var randomValue = UnityEngine.Random.Range(0, totalPercent);

		if (randomValue < EMPTY_CUBE_PERCENT)
		{
			return ChickenCubeType.NONE;
		}
		else if (randomValue < EMPTY_CUBE_PERCENT + CRACKED_CUBE_PERCENT)
		{
			return ChickenCubeType.CRACKED;
		}
		else
		{
			return ChickenCubeType.SOLID;
		}
	}

	private void SelectCubesByBomb(UIChickenCubeItem _selectedCube)
	{
		for (int x = -2; x <= 2; x++)
		{
			for (int y = -2; y <= 2; y++)
			{
				if (Mathf.Abs(x) + Mathf.Abs(y) > 2)
				{
					continue;
				}

				var point = new Point(_selectedCube.X + x, _selectedCube.Y + y);
				if (map.TryGetValue(point, out var cube) == true)
				{
					cube.Select(true);
				}
			}
		}
	}

	private void SelectCubesByDrill(UIChickenCubeItem _selectedCube)
	{
		for (int y = 0; y < 7; y++)
		{
			var point = new Point(_selectedCube.X, FocusY - 6 + y);
			if (map.TryGetValue(point, out var cube) == true)
			{
				cube.Select(true);
			}
		}

		{
			var point = new Point(_selectedCube.X - 1, FocusY);
			if (map.TryGetValue(point, out var cube) == true)
			{
				cube.Select(true);
			}
		}

		{
			var point = new Point(_selectedCube.X + 1, FocusY);
			if (map.TryGetValue(point, out var cube) == true)
			{
				cube.Select(true);
			}
		}
	}

	public void OnPointerDown(Vector2 _pos)
	{
		if (currentTool == DiggingTool.None)
		{
			return;
		}

		selectedCube = GetCube(_pos);
		if (selectedCube == null)
		{
			return;
		}

		bool isEnabled = selectedCube.Select(false, currentTool);

		if (isEnabled)
		{
			switch (currentTool)
			{
				case DiggingTool.Pickaxe:
					break;
				case DiggingTool.Bomb:
					SelectCubesByBomb(selectedCube);
					break;
				case DiggingTool.Drill:
					SelectCubesByDrill(selectedCube);
					break;
			}
		}
	}

	public void OnPointerMove(Vector2 _pos)
	{
		if (currentTool == DiggingTool.None)
		{
			return;
		}

		var currentCube = GetCube(_pos);
		if (currentCube == null)
		{
			return;
		}
		if (currentCube != selectedCube)
		{
			DeselectAllCube();
			selectedCube = currentCube;
			OnPointerDown(_pos);
		}
	}

	public void OnPointerUp(Vector2 _pos)
	{
		if (currentTool == DiggingTool.None)
		{
			return;
		}

		if (currentTool == DiggingTool.Pickaxe)
		{
			if (selectedCube != null && selectedCube.IsShaded == false && selectedCube.CubeType != ChickenCubeType.NONE)
			{
				// 부숴졌으면
				if (selectedCube.Hit(1) == true)
				{
					if (selectedCube.Y == FocusY)
					{
						ScrollCube(selectedCube);
					}
				}
				if (Inventory.it.ConsumeItem("pickaxe", new IdleNumber(1)).Fail())
				{
					pickaxeToggle.isOn = false;
				}
				UpdateItemCount();
			}
		}
		else if (currentTool == DiggingTool.Bomb)
		{
			var selectedCubes = FindSelectedCubes();
			if (selectedCubes.Count != 0)
			{
				UIChickenCubeItem focusCube = null;

				foreach (var cube in selectedCubes)
				{
					cube.Hit(2);
					cube.SetShadeOn(false);
					if (cube.X == selectedCube.X && cube.Y == FocusY)
					{
						focusCube = cube;
					}
				}
				if (focusCube != null)
				{
					ScrollCube(focusCube);
				}
				if (Inventory.it.ConsumeItem("bomb", new IdleNumber(1)).Fail())
				{
					bombToggle.isOn = false;
				}
				UpdateItemCount();
			}
		}
		else if (currentTool == DiggingTool.Drill)
		{
			var selectedCubes = FindSelectedCubes();
			if (selectedCubes.Count != 0)
			{
				UIChickenCubeItem focusCube = null;

				foreach (var cube in selectedCubes)
				{
					cube.Hit(2);
					cube.SetShadeOn(false);
					if (cube.X == selectedCube.X && cube.Y == FocusY)
					{
						focusCube = cube;
					}
				}
				if (focusCube != null)
				{
					ScrollCube(focusCube);
				}
				if (Inventory.it.ConsumeItem("drill", new IdleNumber(1)).Fail())
				{
					drillToggle.isOn = false;
				}
				UpdateItemCount();
			}
		}
		DeselectAllCube();
		RefreshAllCubeShade();
	}

	private List<UIChickenCubeItem> FindSelectedCubes()
	{
		List<UIChickenCubeItem> result = new List<UIChickenCubeItem>();

		foreach (var cube in map.Values)
		{
			if (cube.IsSelected == true)
			{
				result.Add(cube);
			}
		}

		return result;
	}

	private void ScrollCube(UIChickenCubeItem _selectedCube)
	{
		var focusedCube = _selectedCube;

		while (true)
		{
			MakeCubes(FocusY + 3);
			Focus(FocusY + 1);

			var nextCube = GetCube(new Point(focusedCube.X, focusedCube.Y + 1));

			if (nextCube.CubeType != ChickenCubeType.NONE)
			{
				break;
			}
			else
			{
				focusedCube = nextCube;
			}
		}
	}

	public void OnPointerCancel()
	{
		DeselectAllCube();
		selectedCube = null;
	}

	private void DeselectAllCube()
	{
		foreach (var cube in map.Values)
		{
			cube.Deselect();
		}
	}

	private UIChickenCubeItem GetCube(Vector2 _pos)
	{
		Point point = new Point((int)_pos.x / 150, -(int)_pos.y / 150);
		point.Y += FocusY - 6;
		return GetCube(point);
	}

	private UIChickenCubeItem GetCube(Point _point)
	{
		map.TryGetValue(_point, out var cube);
		return cube;
	}

	#region Pool
	private UIChickenCubeItem OnCreateObject()
	{
		UIChickenCubeItem cube = Instantiate(prefabObject, cubeParentTransform);
		cube.SetManagedPool(cubePool);
		return cube;
	}

	private void OnGetObject(UIChickenCubeItem _object)
	{
		_object.gameObject.SetActive(true);
	}

	private void OnReleaseObject(UIChickenCubeItem _object)
	{
		_object.gameObject.SetActive(false);
	}

	private void OnDestroyObject(UIChickenCubeItem _object)
	{
		Destroy(_object.gameObject);
	}
	#endregion
}
