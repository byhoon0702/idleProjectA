using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepFog : MonoBehaviour
{
	[SerializeField] private SpriteRenderer fogSprite;

	Color fogImageColor = new Color(1, 1, 1, 1);

	float speed = 1f;
	float alpha = 0.7f;

	public void Show(GameObject _unitFoot)
	{
		Vector3 fogPos = _unitFoot.transform.position;
		transform.position = new Vector3(fogPos.x - 0.4f, fogPos.y, fogPos.z);
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		alpha -= speed * deltaTime;
		fogSprite.color = new Color(1, 1, 1, alpha);

		if (alpha < 0)
		{
			Destroy(gameObject);
			return;
		}

		fogImageColor.a -= alpha;
	}
}
