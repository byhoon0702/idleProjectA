using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	public Animator animator;

	public void PlayAnimation(string aniName)
	{
		if (animator == null)
		{
			return;
		}
		animator.Play(aniName, -1, 0);
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
