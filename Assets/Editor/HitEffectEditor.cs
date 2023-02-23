using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.DOTweenEditor;

[CustomEditor(typeof(HitEffect))]
public class HitEffectEditor : Editor
{
	HitEffect hitEffect;
	private void OnEnable()
	{
		hitEffect = target as HitEffect;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("테스트"))
		{
			DOTweenEditorPreview.Stop(true, true);
			hitEffect.Set(null);
			//hitEffect.Play();
			DOTweenEditorPreview.PrepareTweenForPreview(hitEffect.PlayEditor());
			DOTweenEditorPreview.Start();

			List<ParticleSystem> particleList = new List<ParticleSystem>(hitEffect.GetComponents<ParticleSystem>());
			particleList.AddRange(hitEffect.GetComponentsInChildren<ParticleSystem>());

			foreach (var particle in particleList)
			{
				particle.Play();
			}
		}
	}
}
