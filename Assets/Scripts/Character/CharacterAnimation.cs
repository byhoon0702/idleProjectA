using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	public Animator animator;

	[SerializeField] private Transform centerPivot;
	[SerializeField] private Transform headPivot;

	public Transform CenterPivot => centerPivot;
	public Transform HeadPivot => headPivot;

	public void PlayAnimation(string aniName)
	{
		if (animator == null)
		{
			return;
		}
		//animator.Play(aniName, -1, 0);
		animator.SetTrigger(aniName);
	}

}
