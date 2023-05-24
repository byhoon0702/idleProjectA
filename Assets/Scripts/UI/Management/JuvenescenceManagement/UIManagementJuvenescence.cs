using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JuvenescencePage
{
	Juvenescence,
	Hyper,
}

public class UIManagementJuvenescence : MonoBehaviour
{
	[SerializeField] private UIPageHyper uiPageHyper;
	[SerializeField] private UIPageJuvenescence uIPageJuvenescence;

	[SerializeField] private Toggle toggleHyper;
	[SerializeField] private Toggle toggleJuvenescence;

	private JuvenescencePage page;
	// Start is called before the first frame update
	public void Awake()
	{
		uiPageHyper.Init(this);
		uIPageJuvenescence.Init(this);
	}
	private void Start()
	{
		if (page == JuvenescencePage.Juvenescence)
		{
			toggleJuvenescence.isOn = true;
		}
		else
		{
			toggleHyper.isOn = true;
		}

	}
	public void SetPage(JuvenescencePage _page)
	{
		page = _page;
	}

	public void OnToggleJuvenescence(bool isOn)
	{
		uIPageJuvenescence.gameObject.SetActive(isOn);
	}

	public void OnToggleHyper(bool isOn)
	{
		uiPageHyper.gameObject.SetActive(isOn);
	}
}
