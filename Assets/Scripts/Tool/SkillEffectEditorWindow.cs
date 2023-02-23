using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SkillEffectEditorWindow : EditorWindow
{
	private SkillEffectEditorPanel target;
	private string path;
	private string skillName;
	private string description;
	private int pageIndex = 0;
	private long newTid = 0;

	private SkillEffectObject skillEffectObject;
	private SerializedObject serializedObject;
	private SkillEffectData currentData;

	private Vector2 scrollPos;
	private string[] pageNames = new string[]
	{
		"편집",
		"생성"
	};


	public static SkillEffectEditorWindow CreateWindow(SkillEffectEditorPanel _projectileEditorPanel)
	{
		if (Application.isPlaying == false)
		{
			return null;
		}

		var window = EditorWindow.GetWindow<SkillEffectEditorWindow>();
		window.Init(_projectileEditorPanel);
		window.Show();

		return window;
	}
	public void Init(SkillEffectEditorPanel _projectileEditorPanel)
	{
		target = _projectileEditorPanel;

	}

	bool isLoop;

	private void OnGUI()
	{
		if (Application.isPlaying == false)
		{
			this.Close();
			return;
		}

		pageIndex = GUILayout.Toolbar(pageIndex, pageNames);
		path = EditorGUILayout.TextField("경로", path);
		if (serializedObject != null)
		{
			if (skillEffectObject == null)
			{
				serializedObject = null;
			}
		}

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		if (pageIndex == 0)
		{
			if (UnitEditor.it.viewPlayer != null)
			{
				for (int i = 0; i < target.layerStates.Keys.Count; i++)
				{
					target.selectedAniIndex[i] = EditorGUILayout.Popup($"{i}번 레이어", target.selectedAniIndex[i], target.layerStates[i]);
				}

				isLoop = GUILayout.Toggle(isLoop, "애니메이션 루프");
				UnitEditor.it.SetAttackAnimationLoop(isLoop);

				if (GUILayout.Button("애니메이션 재생"))
				{
					for (int i = 0; i < target.selectedAniIndex.Length; i++)
					{
						PlayAnimation(i, target.layerStates[i][target.selectedAniIndex[i]]);
					}
				}
			}

			if (skillEffectObject == null)
			{
				target.selectedDataIndex = EditorGUILayout.IntPopup("Data List", target.selectedDataIndex, target.dataList.ToArray(), null);

				if (GUILayout.Button("씬에 복사"))
				{
					currentData = target.GetDataByIndex(target.selectedDataIndex);
					CreateNewEffectObject(currentData);
				}
			}
			else
			{

				if (UnitEditor.it.viewPlayer != null)
				{
					if (GUILayout.Button("캐릭터에 스킬 부여"))
					{
						CreateNewEffectObject(currentData);
					}
				}
				if (GUILayout.Button("스킬 시연"))
				{
					Vector3 pos = new Vector3(-3, 0, 0);
					Vector3 targetPos = new Vector3(3, 0, 0);
					if (UnitEditor.it.viewPlayer != null)
					{
						pos = UnitEditor.it.viewPlayer.unitAnimation.CenterPivot.position;
						targetPos = new Vector3(pos.x + 3, pos.y, 0);
					}

					skillEffectObject.OnSkillStartEditor(UnitEditor.it.viewPlayer, pos, targetPos);

				}
				if (GUILayout.Button("씬 오브젝트 삭제"))
				{
					DestroyCreatedProjectile();
				}

				if (GUILayout.Button("JSon 저장"))
				{
					target.CreateNewData(skillEffectObject.data);
					target.Save();
				}
				if (serializedObject != null)
				{
					serializedObject.Update();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("mData"));
					serializedObject.ApplyModifiedProperties();
				}

			}
		}
		else
		{
			newTid = EditorGUILayout.LongField("Tid ", newTid);
			skillName = EditorGUILayout.TextField("이름", skillName);
			description = EditorGUILayout.TextField("설명", description);

			if (GUILayout.Button("씬에 복사"))
			{

				SkillEffectData data = new SkillEffectData();
				data.tid = newTid;
				data.name = skillName;
				data.description = description;
				currentData = data;
				CreateNewEffectObject(data);

				pageIndex = 0;
			}
		}

		EditorGUILayout.EndScrollView();
	}

	private void CreateNewEffectObject(SkillEffectData data)
	{
		DestroyCreatedProjectile();
		GameObject temp = new GameObject("SkillEffectObject");

		skillEffectObject = temp.AddComponent<SkillEffectObject>();
		serializedObject = new SerializedObject(skillEffectObject);
		skillEffectObject.SetData(currentData);
		if (UnitEditor.it.viewPlayer != null)
		{
			UnitEditor.it.viewPlayer.SetSkillEffectObject(skillEffectObject);
		}
	}

	private void PlayAnimation(int layer, string aniStateName)
	{
		target.PlayAnimation(layer, aniStateName);
	}

	private void DestroyCreatedProjectile()
	{

		if (skillEffectObject == null)
		{
			return;
		}

		Destroy(skillEffectObject.gameObject);
		skillEffectObject = null;
	}

	private void OnDestroy()
	{
		System.GC.Collect();
	}
}
