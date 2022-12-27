using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SceneCamera : MonoBehaviour
{
	private static SceneCamera instance;
	public static SceneCamera it => instance;
	public float speed;

	protected Camera m_camera;
	protected Transform m_camera_transform;

	protected PlayerCharacter[] players;
	void Awake()
	{
		instance = this;
		m_camera = transform.GetComponent<Camera>();
		m_camera_transform = transform;
	}
	void Start()
	{
		players = GameObject.FindObjectsOfType<PlayerCharacter>();

		Debug.Log(m_camera.ViewportToWorldPoint(new Vector3(1, 0.5f, m_camera.nearClipPlane)));
	}

	public Vector2 GetCameraEdgePosition(Vector2 pivot)
	{
		return m_camera.ViewportToWorldPoint(new Vector3(pivot.x, pivot.y, m_camera.nearClipPlane));
	}

	// Update is called once per frame
	void Update()
	{

		//m_camera_transform.Translate(speed * Time.deltaTime);
		CameraMove();
	}

	void CameraMove()
	{
		if (players == null || players.Length == 0)
		{
			return;
		}
		Vector3 near = m_camera_transform.position;
		float distance = 1f;
		for (int i = 0; i < players.Length; i++)
		{
			var player = players[i];
			var diff = m_camera_transform.position.x - player.transform.position.x;

			if (diff < distance)
			{
				m_camera_transform.Translate(Vector3.right * speed * Time.deltaTime);
				break;
			}
		}

	}
}
