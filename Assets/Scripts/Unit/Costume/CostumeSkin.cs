using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;
using static UnityEngine.Rendering.DebugUI;

public class CostumeSkin : MonoBehaviour
{
	[SerializeField] private SpriteSkin[] skins;
	[SerializeField] private SpriteRenderer[] renderers;

	MaterialPropertyBlock tintPropertyBlock;
	MaterialPropertyBlock maskPropertyBlock;
	public void Init()
	{
		skins = transform.GetComponentsInChildren<SpriteSkin>();
		renderers = new SpriteRenderer[skins.Length];
		for (int i = 0; i < skins.Length; i++)
		{
			renderers[i] = skins[i].GetComponent<SpriteRenderer>();
		}
	}
	private void Start()
	{
		if (tintPropertyBlock == null)
		{
			tintPropertyBlock = new MaterialPropertyBlock();
		}
		if (maskPropertyBlock == null)
		{
			maskPropertyBlock = new MaterialPropertyBlock();
		}
	}

	public void SetRootBone(Transform rootBone)
	{
		System.Type type = typeof(SpriteSkin);
		for (int i = 0; i < skins.Length; i++)
		{
			FindBone(rootBone, skins[i].GetComponent<SpriteRenderer>().sprite, skins[i]);
		}
	}
	private void FindBone(Transform rootBone, Sprite sprite, SpriteSkin skin)
	{
		var bones = sprite.GetBones();
		List<Transform> boneTransforms = new List<Transform>();
		for (int i = 0; i < bones.Length; i++)
		{
			Transform bone = rootBone.FindDeepChild(bones[i].name);
			if (bone != null)
			{
				boneTransforms.Add(bone);
			}
		}
		System.Type type = typeof(SpriteSkin);
		BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

		var set_rootbone = type.GetProperty("rootBone", flags);


		set_rootbone.SetValue(skin, rootBone);

		System.Reflection.FieldInfo field = type.GetField("m_BoneTransforms", flags);
		var flag = type.GetMethod("CacheValidFlag", flags);
		var changed = type.GetMethod("OnBoneTransformChanged", flags);
		field.SetValue(skin, boneTransforms.ToArray());
		flag.Invoke(skin, null);
		changed.Invoke(skin, null);
	}
	public void SetMaterialTint(float value)
	{
		if (tintPropertyBlock == null)
		{
			tintPropertyBlock = new MaterialPropertyBlock();
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i] == null)
			{
				continue;
			}
			renderers[i].GetPropertyBlock(tintPropertyBlock);
			tintPropertyBlock.SetFloat("_TintValue", value);
			renderers[i].SetPropertyBlock(tintPropertyBlock);
		}
	}
	public void SetMaterialMask(Color color)
	{
		if (maskPropertyBlock == null)
		{
			maskPropertyBlock = new MaterialPropertyBlock();
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i] == null)
			{
				continue;
			}
			renderers[i].GetPropertyBlock(maskPropertyBlock);
			maskPropertyBlock.SetColor("_MaskColor", color);
			renderers[i].SetPropertyBlock(maskPropertyBlock);
		}
	}


}
