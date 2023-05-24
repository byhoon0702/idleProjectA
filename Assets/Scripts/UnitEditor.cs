#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;


public class UnitEditor : MonoBehaviour
{
	private static UnitEditor instance;
	public static UnitEditor it => instance;

	public Camera editorCamera;
	public EditorToolUI editorToolUI;
	public EditorUnit prefab;
	public DummyUnit dummyprefab;
	public ConfigMeta configMeta;
	public Transform root2d;
	public EditorUnit viewPlayer
	{
		get; private set;
	}
	public DummyUnit clickedDummy
	{
		get; private set;
	}
	private List<DummyUnit> dummies = new List<DummyUnit>();

	//public SkillObject editedProjectile
	//{
	//	get;
	//	private set;
	//}

	private float animationSpeed;
	private float attackTime;
	UnityEditor.Animations.AnimatorController editorAnimatorController;

	public string projectileName;
	private bool isDummyShow = false;
	//private Dictionary<string, List<SkillObject>> projectilePool = new Dictionary<string, List<SkillObject>>();


	private Vector3 defaultPos = new Vector3(0, 3f, -10);
	private float size = 5;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		editorCamera.transform.position = new Vector3(defaultPos.x, defaultPos.y, defaultPos.z);
		DataManager.LoadAllJson();
		configMeta = ScriptableObject.CreateInstance<ConfigMeta>();
		editorToolUI.Init();
	}

	public void OnClickSpawn(UnitData data)
	{
		if (viewPlayer != null)
		{
			Destroy(viewPlayer.gameObject);
			viewPlayer = null;
		}
		EditorUnit player = Instantiate(prefab);
		player.transform.SetParent(root2d);
		if (isDummyShow)
		{
			player.transform.localPosition = new Vector3(-2.5f, 0, 0);
		}
		else
		{
			player.transform.localPosition = Vector3.zero;
		}

		player.Spawn(data);

		viewPlayer = player;
		var runtimeController = viewPlayer.unitAnimation.animator.runtimeAnimatorController;
		editorAnimatorController = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(UnityEditor.AssetDatabase.GetAssetPath(runtimeController));
		editorToolUI.SetUnit(player, editorAnimatorController);
		GetLayerStates();
	}

	public Dictionary<int, List<string>> layerStates = new Dictionary<int, List<string>>();

	public Dictionary<int, List<string>> GetLayerStates()
	{
		layerStates = new Dictionary<int, List<string>>();

		if (editorAnimatorController == null)
		{
			return layerStates;
		}
		for (int i = 0; i < editorAnimatorController.layers.Length; i++)
		{
			var layer = editorAnimatorController.layers[i];

			if (layerStates.ContainsKey(i) == false)
			{
				layerStates.Add(i, new List<string>());
			}
			foreach (var state in layer.stateMachine.states)
			{
				layerStates[i].Add(state.state.name);
			}
		}
		return layerStates;
	}

	public List<string> GetStates()
	{
		List<string> stateName = new List<string>();
		if (editorAnimatorController == null)
		{
			return stateName;
		}

		for (int i = 0; i < editorAnimatorController.layers.Length; i++)
		{
			var layer = editorAnimatorController.layers[i];

			foreach (var state in layer.stateMachine.states)
			{
				stateName.Add(state.state.name);
			}
		}
		return stateName;
	}

	public void OnClickSpawnPet(PetData data)
	{
		GameObject unit = new GameObject(data.name);
		unit.transform.SetParent(root2d);
		unit.transform.localPosition = Vector3.zero;
		EditorUnit player = unit.AddComponent<EditorUnit>();
		player.Spawn(data);

		viewPlayer = player;
		var runtimeController = viewPlayer.unitAnimation.animator.runtimeAnimatorController;
		editorAnimatorController = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(UnityEditor.AssetDatabase.GetAssetPath(runtimeController));

		editorToolUI.SetUnit(player, editorAnimatorController);
	}

	//public void SetProjectile(SkillObject _projectile)
	//{
	//	projectileName = _projectile.name;
	//	editedProjectile = _projectile;
	//}

	public void OnClickPlayAnimation(string animationName)
	{
		//if (viewPlayer == null)
		//{
		//	return;
		//}

		//string[] split = animationName.Split(':');

		//for (int i = 0; i < editorAnimatorController.layers.Length; i++)
		//{
		//	var layer = editorAnimatorController.layers[i];
		//	if (layer.name == split[0])
		//	{

		//		foreach (var state in layer.stateMachine.states)
		//		{
		//			if (state.state.name == split[1])
		//			{
		//				viewPlayer.PlayAnimation(state.state.name, state.state.motion as AnimationClip);
		//				return;
		//			}
		//		}
		//	}
		//}
	}

	public void OverrideAnimation(AnimatorOverrideController overrideController)
	{
		if (overrideController == null)
		{
			viewPlayer.ResetAnimator();
			return;
		}
		viewPlayer.OverrideAnimator(overrideController);
	}

	public void PlayerAnimation(int layerIndex, string statename)
	{

	}

	public void SetAttackAnimationLoop(bool isLoop)
	{
		viewPlayer.SetAttackAnimationLoop(isLoop);
	}


	public void SpawnTargetDummy(int count)
	{
		for (int i = 0; i < count; i++)
		{
			DummyUnit dummy = Instantiate(dummyprefab);
			dummy.transform.SetParent(root2d);
			dummy.transform.localPosition = new Vector3(2.5f + (1.5f * i), 0, 0);

			dummy.Spawn("Dummy");
			dummies.Add(dummy);
		}
	}

	//public void SpawnProjectile(HitInfo hitinfo, AttackData data, float speed, Vector3 mypos, Vector3 targetPos)
	//{
	//	string resource = data.objectResource;
	//	if (resource == null)
	//	{
	//		return;
	//	}
	//	SkillAttackObject projectile = null;

	//	if (projectilePool.ContainsKey(resource) == false)
	//	{
	//		projectilePool.Add(resource, new List<SkillAttackObject>());
	//	}
	//	var _projectileList = projectilePool[resource];
	//	for (int i = 0; i < _projectileList.Count; i++)
	//	{
	//		projectile = _projectileList[i];
	//		if (projectile.gameObject.activeSelf == false)
	//		{
	//			projectile.gameObject.SetActive(true);
	//			projectile.speed = speed;

	//			projectile = SetEditorProjectile(hitinfo, projectile, mypos, targetPos, data);
	//			return;
	//		}
	//	}

	//	GameObject go = (GameObject)Instantiate(Resources.Load($"Projectile/{resource}"));
	//	projectile = go.GetComponent<SkillAttackObject>();
	//	projectile.speed = speed;

	//	projectile = SetEditorProjectile(hitinfo, projectile, mypos, targetPos, data);

	//	projectilePool[resource].Add(projectile);
	//}

	//private SkillAttackObject SetEditorProjectile(HitInfo hitInfo, SkillAttackObject projectile, Vector3 pos, Vector3 targetPos, AttackData attackData)
	//{
	//	var target = FindTarget();
	//	if (target != null)
	//	{
	//		projectile.Spawn(pos, target, hitInfo, attackData);
	//	}
	//	else
	//	{
	//		projectile.Spawn(pos, targetPos, hitInfo, attackData);
	//	}

	//	return projectile;
	//}



	public void SpawnDummy(int count)
	{
		isDummyShow = count > 0;
		float diff = 6f / 10f;
		int relativeCount = count > 1 ? count - 1 : 0;
		editorCamera.transform.position = new Vector3(defaultPos.x + (relativeCount * diff), defaultPos.y, defaultPos.z);

		editorCamera.orthographicSize = size + (relativeCount * (diff / 2));

		for (int i = 0; i < dummies.Count; i++)
		{
			Destroy(dummies[i].gameObject);
		}
		dummies.Clear();

		SpawnTargetDummy(count);

		SetPlayerPos(isDummyShow);
	}


	public void SetPlayerPos(bool isChange)
	{
		if (viewPlayer == null)
		{
			return;
		}
		if (isChange)
		{
			viewPlayer.localPos = new Vector3(-2.5f, 0, 0);
		}
		else
		{
			viewPlayer.localPos = new Vector3(0, 0, 0);
		}
	}

	public void OnClickUnitLevelup()
	{

	}

	public void SetAnimationSpeed(float speed)
	{
		animationSpeed = speed;
	}


	public float GetAnimationSpeed()
	{
		return animationSpeed;
	}


	public void SetAttackTime(float time)
	{
		attackTime = time;
	}
	public float GetAttackTime()
	{
		return attackTime;
	}
	public DummyUnit FindTarget()
	{
		if (clickedDummy != null)
		{
			return clickedDummy;
		}
		else
		{
			if (dummies.Count > 0)
			{
				int index = Random.Range(0, dummies.Count);
				return dummies[index];
			}
			else
			{
				return null;
			}

		}
	}

	private void Update()
	{
		//if (viewPlayer != null)
		//{
		//	viewPlayer.characterAnimation.animator.speed = animationSpeed;
		//}

		if (Input.GetMouseButtonDown(0))
		{

			Ray ray = editorCamera.ScreenPointToRay(Input.mousePosition);

			for (int i = 0; i < dummies.Count; i++)
			{
				if (Vector3.Distance(ray.origin, dummies[i].CenterPosition) < 1f)
				{
					clickedDummy = dummies[i];
					break;
				}
			}
		}
	}
}
#endif
