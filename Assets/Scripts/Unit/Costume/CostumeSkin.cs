using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;


public class CostumeSkin : MonoBehaviour
{
	[SerializeField] private SpriteSkin[] skins;
	[SerializeField] private SpriteRenderer[] renderers;
	[SerializeField] private AnimatorOverrideController overrideController;
	public AnimatorOverrideController OverrideController => overrideController;


	MaterialPropertyBlock tintPropertyBlock;
	MaterialPropertyBlock maskPropertyBlock;
	MaterialPropertyBlock dissolvePropertyBlock;
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

		if (dissolvePropertyBlock == null)
		{
			dissolvePropertyBlock = new MaterialPropertyBlock();
		}
	}

	public void SetRootBone(Transform rootBone)
	{
		//System.Type type = typeof(SpriteSkin);
		for (int i = 0; i < skins.Length; i++)
		{
			if (skins[i] == null)
			{
				continue;
			}
			SpriteRenderer skinRenderer = skins[i].GetComponent<SpriteRenderer>();
			if (skinRenderer == null)
			{
				continue;
			}

			FindBone(rootBone, skinRenderer.sprite, skins[i]);
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

		var spriteRenderer = type.GetField("m_SpriteRenderer", flags).GetValue(skin);
		if (spriteRenderer == null)
		{
			return;
		}
		var set_rootbone = type.GetProperty("rootBone", flags);


		try
		{
			set_rootbone.SetValue(skin, rootBone);
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
		}


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
			tintPropertyBlock.SetFloat("_TintAmount", value);
			renderers[i].SetPropertyBlock(tintPropertyBlock);
		}
	}
	public void SetMaterialDissolve(float value)
	{
		if (dissolvePropertyBlock == null)
		{
			dissolvePropertyBlock = new MaterialPropertyBlock();
		}
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i] == null)
			{
				continue;
			}
			renderers[i].GetPropertyBlock(dissolvePropertyBlock);
			dissolvePropertyBlock.SetFloat("_DissolveAmount", value);
			renderers[i].SetPropertyBlock(dissolvePropertyBlock);
		}
	}
	public void SetMaterialMask(bool isGray)
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
			maskPropertyBlock.SetFloat("_Grayscale", isGray ? 1 : 0);
			renderers[i].SetPropertyBlock(maskPropertyBlock);
		}
	}


}
