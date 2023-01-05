using UnityEngine;
using System.Collections.Generic;

public class Demo : MonoBehaviour
{
	public GameObject m_SceneRoot1, m_SceneRoot2;
	public ScreenTransition m_ST;
	[Range(0.001f, 0.01f)] public float m_TransitionSpeed = 0.003f;
	public Material[] m_Mats;
	[Header("Internal")]
	public string m_Effect;
	public int m_CurrIndex = 0;
	int m_PrevIndex = 0;
	float m_CurrTransitionProgress = 0.001f;
	int m_PlayState = 0;   // 0 -> increase, 1 -> decrease, 2 -> wait
	int m_NextPlayState = 0;
	int m_WaitFrame = 0;
	RenderTexture m_RT1, m_RT2;
	Camera m_RTCam;

    void Start ()
	{
		m_RT1 = RenderTexture.GetTemporary (Screen.width, Screen.height, 24);
		m_RT2 = RenderTexture.GetTemporary (Screen.width, Screen.height, 24);
		InvokeRepeating ("UpdateTransition", 0f, 0.01f);
		m_ST.m_Mat = m_Mats[m_CurrIndex];
		m_Effect = string.Format("{0}:{1}", m_CurrIndex, m_ST.m_Mat.name);
	}
	void Update ()
    {
		Camera camCurr = Camera.main;
		//camCurr.transform.LookAt (m_SceneRoot1.transform);
		//camCurr.transform.Translate (Vector3.right * Time.deltaTime);
		
		if (m_RTCam == null)
		{
			GameObject go = new GameObject ("RTCamera", typeof(Camera));
			m_RTCam = go.GetComponent<Camera>();
			go.transform.parent = camCurr.transform;
		}
		m_RTCam.CopyFrom (camCurr);
		m_RTCam.enabled = false;

		if (Time.frameCount % 2 == 0)
		{
			m_SceneRoot1.SetActive (true);
			m_SceneRoot2.SetActive (false);
			m_RTCam.targetTexture = m_RT1;
		}
		else
		{
			m_SceneRoot1.SetActive (false);
			m_SceneRoot2.SetActive (true);
			m_RTCam.targetTexture = m_RT2;
		}
		m_RTCam.Render ();
		m_ST.PrevTex = m_RT1;
		m_ST.NextTex = m_RT2;
		
		// if user click SelectionGrid change the current transition type, we should handle this situation
		if (m_CurrIndex != m_PrevIndex)
		{
			m_ST.m_Mat = m_Mats[m_CurrIndex];
			m_PrevIndex = m_CurrIndex;
		}
    }
	void UpdateTransition ()
	{
		if (m_PlayState == 0)   // increase
		{
			m_CurrTransitionProgress += m_TransitionSpeed;
			m_CurrTransitionProgress = Mathf.Clamp01 (m_CurrTransitionProgress);
			m_ST.m_Progress = m_CurrTransitionProgress;
			if (m_CurrTransitionProgress.Equals (1f))
			{
				m_PlayState = 2;
				m_WaitFrame = 30;
				m_NextPlayState = 1;
				GoNextTransitionEffect ();
			}
		}
		else if (m_PlayState == 1)   // decrease
		{
			m_CurrTransitionProgress += -m_TransitionSpeed;
			m_CurrTransitionProgress = Mathf.Clamp01 (m_CurrTransitionProgress);
			m_ST.m_Progress = m_CurrTransitionProgress;
			if (m_CurrTransitionProgress.Equals (0f))
			{
				m_PlayState = 2;
				m_WaitFrame = 30;
				m_NextPlayState = 0;
				GoNextTransitionEffect ();
			}
		}
		else   // wait
		{
			--m_WaitFrame;
			m_WaitFrame = m_WaitFrame < 0 ? 0 : m_WaitFrame;
			if (m_WaitFrame == 0)
				m_PlayState = m_NextPlayState;
		}
	}
	void GoNextTransitionEffect ()
	{
		++m_CurrIndex;
		if (m_CurrIndex == m_Mats.Length)
			m_CurrIndex = 0;
		m_PrevIndex = m_CurrIndex;
		m_ST.m_Mat = m_Mats[m_CurrIndex];
		m_Effect = string.Format("{0}:{1}", m_CurrIndex, m_ST.m_Mat.name);
	}
	void OnGUI ()
	{
		int w = 250;
		GUI.Box (new Rect (Screen.width / 2 - w / 2, 10, w, 25), "Screen Transition Shader Demo");
	}
}