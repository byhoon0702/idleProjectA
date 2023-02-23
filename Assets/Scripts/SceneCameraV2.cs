using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneCameraV2 : MonoBehaviour
{
	private static SceneCameraV2 instance;
	public static SceneCameraV2 it => instance;



	[SerializeField] private float shakeAmount = 0.3f;
	[SerializeField] private MapController mapController;
	public Camera sceneCamera => m_camera;
	[SerializeField] private Camera m_camera;
	protected Vector3 offset;
	private bool canCameraMove = false;
	protected Vector3 originPos;



	private void Awake()
	{
		instance = this;
		originPos = transform.position;
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if(UnitManager.it.Player == null)
		{
			return;
		}

		Vector2 offset = m_camera.ViewportToWorldPoint(new Vector2(0.5f, 0)) - m_camera.ViewportToWorldPoint(new Vector2(0.35f, 0));

		Vector3 targetPos = transform.position;

		targetPos.x = UnitManager.it.Player.position.x + offset.x;

		Vector3 camPos = transform.position;

		Vector3 lerp = targetPos;

		Vector3 toward = lerp;
		toward.y = transform.position.y;
		toward.z = transform.position.z;


		if (toward.x < 0)
		{
			return;
		}
		if (mapController != null)
		{
			mapController.Scroll(new Vector2(toward.x - camPos.x, 0));
		}

		transform.position = targetPos;
	}

	public void ShakeCamera()
	{
		m_camera.transform.localPosition = Random.insideUnitCircle * shakeAmount;
		m_camera.transform.DOLocalMove(Vector3.zero, 0.1f);
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
