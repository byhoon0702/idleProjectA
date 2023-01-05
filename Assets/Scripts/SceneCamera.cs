using DG.Tweening;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
	public float shakeAmount = 0.3f;
	private static SceneCamera instance;
	public static SceneCamera it => instance;
	public float speed;

	public Camera sceneCamera => m_camera;
	[SerializeField] private Camera m_camera;
	protected Transform m_camera_transform;

	protected PlayerCharacter[] players;
	protected Vector3 originPos;

	private bool canCameraMove = false;
	void Awake()
	{
		instance = this;

		m_camera_transform = transform;
		originPos = transform.position;
	}
	public void ResetToStart()
	{
		StopCameraMove();
		transform.position = originPos;
	}

	public void ActivateCameraMove()
	{
		canCameraMove = true;
	}
	public void StopCameraMove()
	{
		canCameraMove = false;
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		return sceneCamera.WorldToScreenPoint(position);
	}
	public void FindPlayers()
	{
		players = GameObject.FindObjectsOfType<PlayerCharacter>();
	}
	public Vector2 GetCameraEdgePosition(Vector2 pivot)
	{
		return m_camera.ViewportToWorldPoint(new Vector3(pivot.x, pivot.y, m_camera.nearClipPlane));
	}

	// Update is called once per frame
	void Update()
	{
		if (canCameraMove == false)
		{
			return;
		}
		CameraMove();
	}
	public void ShakeCamera()
	{
		m_camera.transform.localPosition = Random.insideUnitCircle * shakeAmount;
		m_camera.transform.DOLocalMove(Vector3.zero, 0.1f);
	}

	void CameraMove()
	{
		if (players == null || players.Length == 0)
		{
			return;
		}
		Vector3 near = transform.position;
		float distance = 1f;
		for (int i = 0; i < players.Length; i++)
		{
			var player = players[i];
			var diff = transform.position.x - player.transform.position.x;

			if (diff < distance)
			{
				transform.Translate(Vector3.right * player.info.data.moveSpeed * Time.deltaTime);
				break;
			}
		}
	}
}
