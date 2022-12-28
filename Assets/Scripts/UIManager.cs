using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
	private static UIManager instance;
	public static UIManager it => instance;

	public Canvas mainCanvas;
	public FloatingText resource;

	public List<FloatingText> tmpFloatings;

	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		Debug.Log($"{Screen.width} {Screen.height}");
		for (int i = 0; i < 10; i++)
		{
			CreateFloatingText();
		}
	}
	FloatingText CreateFloatingText()
	{
		FloatingText tmp = Instantiate(resource);
		tmp.gameObject.SetActive(false);
		tmp.transform.SetParent(mainCanvas.transform);
		tmpFloatings.Add(tmp);
		return tmp;
	}

	public void ShowFloatingText(string text, Color color, Vector3 position)
	{
		Vector3 pos = SceneCamera.it.camera.WorldToScreenPoint(position);

		float halfX = Screen.width / 2;
		float halfY = Screen.height / 2;

		Vector2 uipos = new Vector2(pos.x - halfX, pos.y - halfY);
		for (int i = 0; i < tmpFloatings.Count; i++)
		{
			if (tmpFloatings[i].gameObject.activeInHierarchy == false)
			{
				tmpFloatings[i].Show(text, color, uipos);
				return;
			}
		}

		var floatingtext = CreateFloatingText();
		floatingtext.Show(text, color, uipos);

	}

}
