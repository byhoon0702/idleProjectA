using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
	public bool OnlyDeactivate;

	private List<ParticleSystem> particles;
	void OnEnable()
	{

		particles = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
		var mine = GetComponent<ParticleSystem>();
		if (mine != null)
		{
			particles.Add(mine);
		}

		StartCoroutine("CheckIfAlive");

	}

	IEnumerator CheckIfAlive()
	{
		bool check = false;
		while (true)
		{
			check = false;
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < particles.Count; i++)
			{
				if (particles[i].IsAlive(true))
				{
					check = true;
					break;
				}
			}

			if (check == false)
			{
				if (OnlyDeactivate)
				{

					this.gameObject.SetActive(false);

				}
				else
				{
					GameObject.Destroy(this.gameObject);
				}
				break;
			}
		}
	}
}
