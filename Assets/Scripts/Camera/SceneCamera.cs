using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SceneCamera : MonoBehaviour
{
	private static SceneCamera instance;
	public static SceneCamera it => instance;

	[SerializeField] private TimelineAsset defaultBossTimeline;
	[SerializeField] private PlayableDirector playableDirector;

	public static PlayableDirector PlayableDirector => it.playableDirector;
	[SerializeField] private SpriteRenderer blackOutCurtain;
	public SpriteRenderer BlackOutCurtain => blackOutCurtain;
	[SerializeField] private CinemachineVirtualCamera defaultVirtualCam;
	public static CinemachineVirtualCamera DefaultVirtualCam => it.defaultVirtualCam;
	[SerializeField] private CinemachineVirtualCamera virtualZoomCam;

	public static CinemachineVirtualCamera VirtualZoomCam => it.virtualZoomCam;

	[SerializeField] private CinemachineVirtualCamera bossVirtualCam;
	public static CinemachineVirtualCamera BossVirtualCam => it.bossVirtualCam;

	[SerializeField] private CinemachineBrain cinemachineBrain;
	public static CinemachineBrain CinemachineBrain => it.cinemachineBrain;

	public Camera sceneCamera => m_camera;
	[SerializeField] private Camera m_camera;

	private bool canCameraMove = false;

	protected Vector3 originPos;

	public CameraShake cameraShake;

	private void Awake()
	{
		instance = this;
		originPos = transform.position;
		BlackOutCurtain.color = new Color(0, 0, 0, 0);
	}

	public void SetTarget(Transform target)
	{
		CinemachineBrain.ActiveVirtualCamera.Follow = target;
	}

	public void ChangeViewPort(bool change)
	{
		if (change)
		{
			m_camera.rect = new Rect(0, 0.4f, 1, 1);
		}
		else
		{
			m_camera.rect = new Rect(0, 0, 1, 1);
		}
	}
	public void PlayBossEnter(TimelineAsset bossEnter)
	{
		TimelineAsset playAsset = bossEnter;
		if (playAsset == null)
		{
			playAsset = defaultBossTimeline;
		}
		PlayableDirector.playableAsset = playAsset;
		PlayableDirector.Play();
	}

	public void PlayTimeline()
	{
		PlayableDirector.Play();
	}

	private void LateUpdate()
	{
		if (UnitManager.it.Player == null)
		{
			return;
		}
		//CameraMove();
		//Vector3 offset = m_camera.ViewportToWorldPoint(new Vector2(0.5f, 0)) - m_camera.ViewportToWorldPoint(new Vector2(0.35f, 0));

		//Vector3 targetPos = transform.position;

		//targetPos.x = UnitManager.it.Player.position.x + offset.x;
		//targetPos.z = UnitManager.it.Player.position.z + offset.y;

		//Vector3 camPos = transform.position;

		//Vector3 lerp = targetPos;

		//Vector3 toward = lerp;
		//toward.y = transform.position.y;
		//toward.z = transform.position.z;


		//if (mapController != null)
		//{
		//	mapController.Scroll(new Vector2(toward.x - camPos.x, 0));
		//}

		//transform.position = targetPos;
	}

	public void ShakeCamera(float shakeAmount = 0.3f, float timer = 0.1f, bool isFade = false)
	{

		cameraShake.Shake(shakeAmount, timer, isFade);
	}

	public void ActivateCameraMove()
	{
		canCameraMove = true;
	}

	public void StopCameraMove()
	{
		canCameraMove = false;
	}

	public void ResetToStart()
	{
		StopCameraMove();
		transform.position = originPos;
	}

	public Vector2 WorldToScreenPoint(Vector3 _position)
	{
		return sceneCamera.WorldToScreenPoint(_position);
	}

	public Vector2 GetCameraEdgePosition(Vector2 _pivot)
	{
		return m_camera.ViewportToWorldPoint(new Vector3(_pivot.x, _pivot.y, m_camera.nearClipPlane));
	}

	public void FindPlayers()
	{

	}



}
