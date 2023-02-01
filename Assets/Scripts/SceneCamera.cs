using DG.Tweening;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
	public float shakeAmount = 0.3f;
	private static SceneCamera instance;
	public static SceneCamera it => instance;
	public float speed;


	public Transform target;
	public MapController mapController;
	public Camera sceneCamera => m_camera;
	[SerializeField] private Camera m_camera;
	protected Transform m_camera_transform;

	protected Vector3 originPos;

	public bool canCameraMove = false;
	protected Vector3 offset;
	protected bool fixedCamera = false;
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
	//private void Start()
	//{
	//	offset = m_camera_transform.position;
	//	offset.x = m_camera_transform.position.x - target.position.x;
	//}
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
		PlayerCharacter player = GameObject.FindObjectOfType<PlayerCharacter>();

		if (player == null)
		{
			//메인 캐릭터 못찾음
			return;
		}

		target = player.transform;
		offset = m_camera_transform.position;
		offset.x = m_camera_transform.position.x - target.position.x;
	}
	public Vector2 GetCameraEdgePosition(Vector2 pivot)
	{
		return m_camera.ViewportToWorldPoint(new Vector3(pivot.x, pivot.y, m_camera.nearClipPlane));
	}

	// Update is called once per frame
	void Update()
	{
		Animator ani; 
		if (fixedCamera)
		{
			return;
		}
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
		if (target == null)
		{
			return;
		}

		Vector3 targetPos = m_camera_transform.position;

		targetPos.x = offset.x + target.position.x;

		Vector3 camPos = m_camera_transform.position;

		Vector3 lerp = Vector3.Lerp(camPos, targetPos, Time.deltaTime * speed);

		Vector3 toward = Vector3.MoveTowards(camPos, lerp, 0.2f);
		toward.y = m_camera_transform.position.y;
		toward.z = m_camera_transform.position.z;


		if (toward.x < 0)
		{
			return;
		}
		if (mapController != null)
		{
			mapController.Scroll(new Vector2(toward.x - camPos.x, 0));
		}

		m_camera_transform.SetPositionAndRotation(toward, m_camera_transform.rotation);
	}
}
