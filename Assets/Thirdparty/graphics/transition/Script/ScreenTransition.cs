using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class ScreenTransition : MonoBehaviour
{
	public Material m_Mat;
	[Range(0, 1)] public float m_Progress = 0f;
	public Texture m_PrevTex = null;
	public Texture m_NextTex = null;

	public Texture PrevTex
	{
		get { return m_PrevTex; }
		set { m_PrevTex = value; }
	}
	public Texture NextTex
	{
		get { return m_NextTex; }
		set { m_NextTex = value; }
	}
	public Material Material
	{
		set { m_Mat = value; }
	}
	void Start ()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		m_Mat.SetTexture ("_PrevTex", m_PrevTex);
		m_Mat.SetFloat ("_Progress", m_Progress);
		Graphics.Blit (m_NextTex, destTexture, m_Mat);
	}
}