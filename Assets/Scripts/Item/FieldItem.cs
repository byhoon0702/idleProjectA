using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolingObject<T> : MonoBehaviour where T : UnityEngine.Object
{
	public IObjectPool<T> pool;

	public virtual void SetObjectPool(IObjectPool<T> _pool)
	{
		pool = _pool;
	}
	public virtual void Init()
	{

	}

	public virtual void Release()
	{
		pool.Release(this as T);
	}

}


public class FieldItem : PoolingObject<FieldItem>
{
	private enum FieldItemState
	{
		APPEAR,
		COLLECT,
	}

	[SerializeField] private GameObject[] itemObjects;
	[SerializeField]
	private float speed;
	FieldItemState state = FieldItemState.APPEAR;

	private Transform target;
	private Vector3 endPos;
	private float duration = 1f;

	Vector3 move;
	private float gravity = 9.8f;
	private float elapsedTime;

	public void Appear(int index, Sprite sprite, Vector3 startPos, Transform target)
	{
		Appear(index, startPos, target);

		itemObjects[3].GetComponent<SpriteRenderer>().sprite = sprite;
	}

	public void Appear(int index, Vector3 startPos, Transform target)
	{
		for (int i = 0; i < itemObjects.Length; i++)
		{
			itemObjects[i].SetActive(false);
		}

		itemObjects[index].SetActive(true);

		state = FieldItemState.APPEAR;
		transform.position = startPos;

		elapsedTime = 0;
		this.target = target;

		move = startPos;

		float x = Random.Range(-1f, 1f);
		endPos = startPos + new Vector3(x, 0, Random.Range(-0.5f, -1f));

		var distance = Vector3.Distance(endPos, startPos);

		var angle = 45;

		float velocity = speed;

		Vector3 normalizeDirection = (endPos - startPos).normalized;
		normalizeDirection.y = 0;

		move.x = velocity * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection.x;
		move.z = velocity * Mathf.Cos(angle * Mathf.Deg2Rad) * normalizeDirection.z;
		move.y = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);

		duration = distance / (new Vector3(move.x, 0, move.z).magnitude);
		stop = false;
	}

	public override void Release()
	{
		stop = true;
		pool.Release(this);
	}
	public void Collect()
	{
		state = FieldItemState.COLLECT;
		elapsedTime = 0;
	}

	bool stop = false;
	void Update()
	{
		if (stop)
		{
			return;
		}

		switch (state)
		{
			case FieldItemState.APPEAR:
				{
					if (elapsedTime < duration)
					{
						Vector3 cal = Vector3.zero;
						cal.x = move.x;
						cal.y = move.y - (gravity * elapsedTime);
						cal.z = move.z;
						transform.Translate(cal * Time.deltaTime, Space.Self);
					}
					if (elapsedTime > duration + 2)
					{
						state = FieldItemState.COLLECT;
					}
				}
				break;
			case FieldItemState.COLLECT:
				{
					if (target == null || transform == null)
					{
						Release();
						return;
					}
					if (Vector3.Distance(target.position, transform.position) > 0.1f)
					{
						Vector3 dir = (target.position - transform.position).normalized;
						transform.Translate(dir * Time.deltaTime * 10f);
					}
					else
					{
						Release();
					}
				}
				break;
		}

		elapsedTime += Time.deltaTime;
	}
}
