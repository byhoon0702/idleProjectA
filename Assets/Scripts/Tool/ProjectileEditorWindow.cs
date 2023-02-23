using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;

public class ProjectileEditorWindow : EditorWindow
{
	private ProjectileEditorPanel target;
	private string path;
	private string projectileName;
	private string description;
	private int pageIndex = 0;
	private long newTid = 0;
	private GameObject baseProjectileObject;

	private SkillActionBehavior behavior;

	private SkillObject projectileForEdit;
	private GameObject hitEffect;

	private string[] pageNames = new string[]
	{
		"편집",
		"생성"
	};


	public static ProjectileEditorWindow CreateWindow(ProjectileEditorPanel _projectileEditorPanel)
	{
		if (Application.isPlaying == false)
		{
			return null;
		}

		var window = EditorWindow.GetWindow<ProjectileEditorWindow>();
		window.Init(_projectileEditorPanel);
		window.Show();

		return window;
	}
	public void Init(ProjectileEditorPanel _projectileEditorPanel)
	{
		target = _projectileEditorPanel;
	}


	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			this.Close();
			return;
		}

		pageIndex = GUILayout.Toolbar(pageIndex, pageNames);
		path = EditorGUILayout.TextField("경로", path);
		if (pageIndex == 0)
		{
			if (projectileForEdit == null)
			{
				target.selectedDataIndex = EditorGUILayout.IntPopup("Data List", target.selectedDataIndex, target.dataList.ToArray(), null);
				target.selectedObjectIndex = EditorGUILayout.IntPopup("Object List", target.selectedObjectIndex, target.projectileObjectList.ToArray(), null);
				target.selectedHitEffectIndex = EditorGUILayout.IntPopup("Hit Effect List", target.selectedHitEffectIndex, target.hitEffectList.ToArray(), null);
				target.selectedBehaviorIndex = EditorGUILayout.IntPopup("Behavior List", target.selectedBehaviorIndex, target.behaviorList.ToArray(), null);
			}
			else
			{
				//projectileForEdit.behavior = (SkillActionBehavior)EditorGUILayout.ObjectField(projectileForEdit.behavior, typeof(SkillActionBehavior), false);
				//projectileForEdit.speed = EditorGUILayout.FloatField("속도", projectileForEdit.speed);
				//if (projectileForEdit.projectileType == ProjectileType.PARABOLA)
				//{
				//	projectileForEdit.angle = EditorGUILayout.FloatField("각도", projectileForEdit.angle);
				//}

				if (GUILayout.Button("씬 오브젝트 삭제"))
				{
					DestroyCreatedProjectile();
				}

				if (GUILayout.Button("실행"))
				{
					SetProjectile(projectileForEdit);
				}
			}

			if (GUILayout.Button("프리팹 불러오기"))
			{

				DestroyCreatedProjectile();

				var obj = AssetDatabase.LoadMainAssetAtPath(target.projectilePathList[target.selectedObjectIndex]);

				GameObject temp = (GameObject)PrefabUtility.InstantiatePrefab(obj);

				projectileForEdit = temp.GetComponent<SkillObject>();
				if (projectileForEdit == null)
				{
					projectileForEdit = temp.AddComponent<SkillObject>();
				}
				SetProjectile(projectileForEdit);
			}

			if (GUILayout.Button("프리팹 저장"))
			{
				SavePrefab(projectileForEdit);
			}
		}
		else
		{
			newTid = EditorGUILayout.LongField("Tid ", newTid);
			projectileName = EditorGUILayout.TextField("이름", projectileName);
			description = EditorGUILayout.TextField("설명", description);

			behavior = (SkillActionBehavior)EditorGUILayout.ObjectField(behavior, typeof(SkillActionBehavior), false);
			baseProjectileObject = (GameObject)EditorGUILayout.ObjectField("참조 오브젝트", baseProjectileObject, typeof(GameObject), true);

			hitEffect = (GameObject)EditorGUILayout.ObjectField("히트 오브젝트", hitEffect, typeof(GameObject), true);

			if (GUILayout.Button("씬에 복사"))
			{
				if (baseProjectileObject == null)
				{
					return;
				}

				DestroyCreatedProjectile();

				GameObject temp = Instantiate(baseProjectileObject);
				temp.name = projectileName;

				projectileForEdit = temp.GetComponent<SkillObject>();
				if (projectileForEdit == null)
				{
					projectileForEdit = temp.AddComponent<SkillObject>();
				}

				//projectileForEdit.behavior = behavior;
				SetProjectile(projectileForEdit);
				//target.editorToolUI.projectileForEdit = projectileForEdit;
			}
			if (projectileForEdit != null)
			{
				if (GUILayout.Button("프리팹 생성"))
				{
					CreatePrefab(projectileForEdit.gameObject, path);
				}
			}
			if (GUILayout.Button("JSON 데이터 저장"))
			{
				SaveData();
			}
		}
	}

	private void SetProjectile(SkillObject go)
	{

		go.name = go.name.Replace("(Clone)", "");
		go.transform.position = Vector3.zero;
		go.transform.localScale = Vector3.one;

		Vector3 pos = new Vector3(-2.5f, 0, 0);
		Vector3 targetpos = new Vector3(2.5f, 0, 0);
		if (UnitEditor.it.viewPlayer != null)
		{
			pos = UnitEditor.it.viewPlayer.unitAnimation.CenterPivot.position;
			targetpos = UnitEditor.it.viewPlayer.target.unitAnimation.CenterPivot.position;
		}

		UnitEditor.it.SetProjectile(projectileForEdit);
		//go.gameObject.SetActive(true);
		//go.Spawn(pos, targetpos, new HitInfo(AttackerType.Player, AttackType.RANGED, new IdleNumber()));
	}

	private void SavePrefab(SkillObject go)
	{
		string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);

		GameObject obj = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);

		if (obj != null)
		{
			var sourceProjectile = obj.GetComponent<SkillObject>();
			sourceProjectile = go;
			EditorUtility.SetDirty(sourceProjectile);
			PrefabUtility.SaveAsPrefabAssetAndConnect(go.gameObject, path, InteractionMode.AutomatedAction);
			return;
		}
	}

	private void CreatePrefab(GameObject go, string path)
	{
		string fullpath = $"Assets/AssetFolder/Resources/{path}/{go.name}.prefab";
		Object obj = AssetDatabase.LoadMainAssetAtPath(fullpath);
		if (obj != null)
		{
			Debug.LogWarning("같은 이름의 프리팹이 이미 있습니다. 다른 이름으로 만들어 주세요");
			return;
		}
		else
		{
			PrefabUtility.SaveAsPrefabAssetAndConnect(go, fullpath, InteractionMode.AutomatedAction, out bool success);
		}

	}
	private void SaveData()
	{
		string hitEffectName = "";
		if (hitEffect != null)
		{
			hitEffectName = hitEffect.name;
		}
		target.CreateNewProjectileData(newTid, description, projectileName, projectileName, hitEffectName);
		target.Save();
	}

	private void DestroyCreatedProjectile()
	{

		if (projectileForEdit == null)
		{
			return;
		}
		Destroy(projectileForEdit.gameObject);
		projectileForEdit = null;
	}

	private void OnDestroy()
	{
		System.GC.Collect();
	}

	private void CreateProjectile()
	{

	}
}
