using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Pool;

public class SCUITweenPathMove : MonoBehaviour
{
	[SerializeField] private RectTransform m_object_from;
	[SerializeField] private RectTransform[] m_object_path_array;
	[SerializeField] private RectTransform m_object_to;
	[SerializeField] public RectTransform m_tween_object;
	[SerializeField] public float m_start_delay = 0f;
	[SerializeField] public float m_duration = 1f;
	[SerializeField] private AnimationCurve m_animation_curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField] private AnimationCurve m_scale_curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField] private int m_loop_counter = -1;
	[SerializeField] private LoopType m_loop_type = LoopType.Restart;
	[SerializeField] private bool m_play_on_awake = true;
	[SerializeField] private bool m_is_loop = false;
	[SerializeField] public bool m_is_copyed = false;
	[SerializeField] public bool m_is_array_object = false;
	[SerializeField] public GameObject[] m_array_target_object;
	[SerializeField] public bool m_is_scale_tween = false;
	[SerializeField] public int m_copy_count = 1;
	[SerializeField] public float m_start_interval = 0.01f;
	[SerializeField] public float m_size_random = 1f;
	[SerializeField] private float m_start_random = 1f;
	[SerializeField] private float m_path_random_range = 1f;
	[SerializeField] private float m_target_random_range = 1f;
	[SerializeField] private Camera m_target_camera;

	private Vector2 m_origin_position;
	private RectTransform[] m_copyed_object_group;
	public Sequence doTweenSequence;

	private IObjectPool<SCUITweenPathMove> managedPool;

	private void OnDestroy()
	{
		DOTween.Kill(this);
	}

	private void OnDisable()
	{
		DOTween.Kill(this);
	}

	public void OnEnable()
	{

	}
	public void OnEndTween()
	{
		managedPool.Release(this);
	}
	public Tween OnMultiCopy()
	{
		StartCoroutine(StartTweeningMulti());
		return doTweenSequence;
	}

	public void Set(IObjectPool<SCUITweenPathMove> _managedPool)
	{
		this.managedPool = _managedPool;
	}

	public bool Init()
	{
		if (m_is_copyed || m_is_array_object)
		{
			if (m_copy_count <= 0)
			{
				Debug.Log("TWeen copy count must bigger then 0");
				return false;
			}
		}
		if (m_duration <= 0)
		{
			Debug.Log("TWeen duration must longer then 0");
			return false;
		}
		m_copyed_object_group = new RectTransform[m_copy_count + 1];
		doTweenSequence.Kill();
		StopCoroutine(StartTweeningMulti());

		return true;
	}
	public void Run(Vector2 _startPos)
	{
		m_object_from.anchoredPosition = _startPos;

		if (Init() == false) return;
		if (m_is_copyed || m_is_array_object)
		{
			StartCoroutine(StartTweeningMulti(OnEndTween));
		}
		else
		{
			DOTween.Play(PlayTween(m_tween_object, OnEndTween));
		}
	}

	private IEnumerator StartTweeningMulti()
	{
		yield return new WaitForSeconds(m_start_delay);

		RectTransform m_object_no_component = m_tween_object;
		SCUITweenPathMove m_tween_component = m_object_no_component.gameObject.GetComponent<SCUITweenPathMove>();
		if (m_tween_component != null)
		{
			m_tween_component.m_play_on_awake = false;
			m_object_no_component = Instantiate(m_tween_object);
		}
		if (m_is_array_object)
		{
			m_copy_count = m_array_target_object.Length;

			for (int i = 0; i < m_copy_count; i++)
			{
				m_array_target_object[i].transform.localScale = m_array_target_object[i].transform.localScale * Random.Range(1 - m_size_random, 1 + m_size_random);

				DOTween.Play(PlayTween(m_array_target_object[i].GetComponent<RectTransform>()));
				if (m_is_scale_tween)
				{
					int myLoops = 0;
					if (m_is_loop) myLoops = m_loop_counter;
					m_array_target_object[i].transform.localScale = new Vector3(0, 0, 0);
					Vector3 m_targetScale = new Vector3(1, 1, 1);
					m_array_target_object[i].transform.DOScale(m_targetScale, m_duration).SetEase(m_scale_curve).SetLoops(myLoops, m_loop_type).SetDelay(0);
				}
				yield return new WaitForSeconds(m_start_delay);
			}
		}
		else
		{
			for (int i = 0; i < m_copy_count; i++)
			{
				m_copyed_object_group[i] = Instantiate(m_object_no_component, this.transform.parent);
				m_copyed_object_group[i].name = "clone_" + i;
				m_copyed_object_group[i].transform.localScale = m_copyed_object_group[i].transform.localScale * Random.Range(1 - m_size_random, 1 + m_size_random);

				DOTween.Play(PlayTween(m_copyed_object_group[i]));
				if (m_is_scale_tween)
				{
					int myLoops = 0;
					if (m_is_loop) myLoops = m_loop_counter;
					m_copyed_object_group[i].transform.localScale = new Vector3(0, 0, 0);
					Vector3 m_targetScale = new Vector3(1, 1, 1);
					m_copyed_object_group[i].transform.DOScale(m_targetScale, m_duration).SetEase(m_scale_curve).SetLoops(myLoops, m_loop_type).SetDelay(0);
				}
				yield return new WaitForSeconds(m_start_interval);
			}
		}
		yield return new WaitForSeconds(m_start_delay);

	}

	private IEnumerator StartTweeningMulti(TweenCallback actionOnComplete = null)
	{
		yield return new WaitForSeconds(m_start_delay);

		RectTransform m_object_no_component = m_tween_object;
		SCUITweenPathMove m_tween_component = m_object_no_component.gameObject.GetComponent<SCUITweenPathMove>();
		if (m_tween_component != null)
		{
			m_tween_component.m_play_on_awake = false;
			m_object_no_component = Instantiate(m_tween_object);
		}
		if (m_is_array_object)
		{
			m_copy_count = m_array_target_object.Length;

			for (int i = 0; i < m_copy_count; i++)
			{
				m_array_target_object[i].transform.localScale = m_array_target_object[i].transform.localScale * Random.Range(1 - m_size_random, 1 + m_size_random);

				DOTween.Play(PlayTween(m_array_target_object[i].GetComponent<RectTransform>()));
				if (m_is_scale_tween)
				{
					int myLoops = 0;
					if (m_is_loop) myLoops = m_loop_counter;
					m_array_target_object[i].transform.localScale = new Vector3(0, 0, 0);
					Vector3 m_targetScale = new Vector3(1, 1, 1);
					m_array_target_object[i].transform.DOScale(m_targetScale, m_duration).SetEase(m_scale_curve).SetLoops(myLoops, m_loop_type).SetDelay(0);
				}
				yield return new WaitForSeconds(m_start_interval);
			}
			yield return new WaitForSeconds(m_start_interval * m_array_target_object.Length + m_duration);
			actionOnComplete();
		}
		else
		{
			for (int i = 0; i < m_copy_count; i++)
			{
				m_copyed_object_group[i] = Instantiate(m_object_no_component, this.transform.parent);
				m_copyed_object_group[i].name = "clone_" + i;
				m_copyed_object_group[i].transform.localScale = m_copyed_object_group[i].transform.localScale * Random.Range(1 - m_size_random, 1 + m_size_random);

				DOTween.Play(PlayTween(m_copyed_object_group[i]));
				if (m_is_scale_tween)
				{
					int myLoops = 0;
					if (m_is_loop) myLoops = m_loop_counter;
					m_copyed_object_group[i].transform.localScale = new Vector3(0, 0, 0);
					Vector3 m_targetScale = new Vector3(1, 1, 1);
					m_copyed_object_group[i].transform.DOScale(m_targetScale, m_duration).SetEase(m_scale_curve).SetLoops(myLoops, m_loop_type).SetDelay(0);
				}
				yield return new WaitForSeconds(m_start_interval);
			}
		}
		yield return new WaitForSeconds(m_start_interval * m_array_target_object.Length + m_duration);
		actionOnComplete();
	}
	public Tween PlayTween(RectTransform m_object_to_move)
	{
		if (m_object_to_move == null)
			m_object_to_move = m_tween_object;

		if (m_object_to_move.gameObject.activeSelf == false) m_object_to_move.gameObject.SetActive(true);

		/*		m_origin_position = ConvertAnchordPositionToCanvasPosition(m_object_from, m_object_from.transform.parent.GetComponent<RectTransform>());
				m_origin_position = ConvertRectTransformToWorldTransform(m_object_from.GetComponent<RectTransform>());*/
		m_origin_position = m_object_from.position;
		m_object_to_move.position = m_origin_position;
		Vector3 m_temp_path_vector;

		if (m_object_from.name != m_object_to_move.name)
		{
			m_temp_path_vector = ConvertAnchordPositionToCanvasPosition(m_object_from, m_object_from.transform.parent.GetComponent<RectTransform>());
			m_temp_path_vector = m_origin_position;
			if (m_start_random != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_start_random), Random.Range(0, m_start_random), Random.Range(0, m_start_random));
			}
			m_object_to_move.anchoredPosition = m_temp_path_vector;
			//m_object_to_move.position = m_origin_position;
		}

		Vector3[] myPath = new Vector3[m_object_path_array.Length + 1];

		int counter;

		if (m_object_path_array.Length > 0)
		{
			for (counter = 0; counter < m_object_path_array.Length; counter++)
			{
				m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_path_array[counter]);
				if (m_path_random_range != 0)
				{
					m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_path_random_range), Random.Range(0, m_path_random_range), Random.Range(0, m_path_random_range));
				}
				myPath[counter] = m_temp_path_vector;
			}
			m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_to);
			if (m_target_random_range != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range));
			}
			myPath[counter] = m_temp_path_vector;
		}
		else
		{
			m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_to);
			if (m_target_random_range != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range));
			}
			myPath[0] = m_temp_path_vector;
		}

		int myLoops = 0;
		if (m_is_loop) myLoops = m_loop_counter;
		Tween returnTween;
		if (m_is_copyed)
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(0).SetRelative(false).SetAutoKill(true).OnComplete(() =>
				{
					DOVirtual.DelayedCall(0, () =>
					{
						DestroyImmediate(m_object_to_move.gameObject);
					});
				});
		}
		else if (m_is_array_object)
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(0).SetRelative(false).SetAutoKill(true).OnComplete(() =>
				{
					DOVirtual.DelayedCall(0, () =>
					{
						m_object_to_move.gameObject.SetActive(false);
					}
					);
				});
		}
		else
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(m_start_delay).SetRelative(false).SetAutoKill(true);
		}

		return returnTween;
	}
	public Tween PlayTween(RectTransform m_object_to_move, TweenCallback actionOnComplete = null)
	{
		if (m_object_to_move == null)
			m_object_to_move = m_tween_object;

		if (m_object_to_move.gameObject.activeSelf == false) m_object_to_move.gameObject.SetActive(true);

		/*		m_origin_position = ConvertAnchordPositionToCanvasPosition(m_object_from, m_object_from.transform.parent.GetComponent<RectTransform>());
				m_origin_position = ConvertRectTransformToWorldTransform(m_object_from.GetComponent<RectTransform>());*/
		m_origin_position = m_object_from.position;
		m_object_to_move.position = m_origin_position;
		Vector3 m_temp_path_vector;

		if (m_object_from.name != m_object_to_move.name)
		{
			m_temp_path_vector = ConvertAnchordPositionToCanvasPosition(m_object_from, m_object_from.transform.parent.GetComponent<RectTransform>());
			m_temp_path_vector = m_origin_position;
			if (m_start_random != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_start_random), Random.Range(0, m_start_random), Random.Range(0, m_start_random));
			}
			m_object_to_move.anchoredPosition = m_temp_path_vector;
			//m_object_to_move.position = m_origin_position;
		}

		Vector3[] myPath = new Vector3[m_object_path_array.Length + 1];

		int counter;

		if (m_object_path_array.Length > 0)
		{
			for (counter = 0; counter < m_object_path_array.Length; counter++)
			{
				m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_path_array[counter]);
				if (m_path_random_range != 0)
				{
					m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_path_random_range), Random.Range(0, m_path_random_range), Random.Range(0, m_path_random_range));
				}
				myPath[counter] = m_temp_path_vector;
			}
			m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_to);
			if (m_target_random_range != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range));
			}
			myPath[counter] = m_temp_path_vector;
		}
		else
		{
			m_temp_path_vector = ConvertRectTransformToWorldTransform(m_object_to);
			if (m_target_random_range != 0)
			{
				m_temp_path_vector = m_temp_path_vector + new Vector3(Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range), Random.Range(0, m_target_random_range));
			}
			myPath[0] = m_temp_path_vector;
		}

		int myLoops = 0;
		if (m_is_loop) myLoops = m_loop_counter;
		Tween returnTween;
		if (m_is_copyed)
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(0).SetRelative(false).SetAutoKill(true).OnComplete(() =>
				{
					DOVirtual.DelayedCall(0, () =>
					{
						if (actionOnComplete != null) actionOnComplete();
						DestroyImmediate(m_object_to_move.gameObject);
					});
				});
		}
		else if (m_is_array_object)
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(0).SetRelative(false).SetAutoKill(true).OnComplete(() =>
				{
					DOVirtual.DelayedCall(0, () =>
					{
						if (actionOnComplete != null) actionOnComplete();
						m_object_to_move.gameObject.SetActive(false);
					}
					);
				});
		}
		else
		{
			returnTween = m_object_to_move.transform.DOPath(myPath, m_duration, PathType.CatmullRom, PathMode.Ignore)
				.SetEase(m_animation_curve).SetLoops(myLoops, m_loop_type).SetDelay(m_start_delay).SetRelative(false).SetAutoKill(true);
		}

		return returnTween;
	}
	public Vector2 ConvertAnchordPositionToCanvasPosition(RectTransform m_rect_transform, RectTransform m_parent_transform)
	{
		//if (m_target_camera == null) m_target_camera = Camera.main;
		if (m_target_camera == null) return new Vector2(-1, -1);
		Vector3[] corners = new Vector3[4];
		m_rect_transform.GetWorldCorners(corners);
		Vector3 CPos = m_target_camera.WorldToScreenPoint(corners[0]);
		Vector2 ReturnPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent_transform, CPos, m_target_camera, out ReturnPoint);
		return ReturnPoint;
	}
	public Vector3 ConvertRectTransformToWorldTransform(RectTransform m_rect_transform)
	{
		Vector3[] corners = new Vector3[4];
		m_rect_transform.GetWorldCorners(corners);
		return corners[0];
	}
}
