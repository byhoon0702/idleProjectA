using UnityEngine;
using UnityEngine.UI;
using System;
public class UIPopupAgreement : MonoBehaviour
{
	[SerializeField] Intro intro;
	[SerializeField] Toggle toggleServiceTerms;
	[SerializeField] Toggle togglePrivateTerms;
	[SerializeField] Toggle toggleAll;

	public const string k_Terms = "TermsAgreement";
	Action _onClose;
	public void Open(Action onClose)
	{
		if (PlayerPrefs.HasKey(k_Terms))
		{
			int term = PlayerPrefs.GetInt(k_Terms, 0);
			if (term == 1)
			{
				onClose.Invoke();
				return;
			}
		}

		_onClose = onClose;
		gameObject.SetActive(true);
	}
	public void OnClickServiceTermsPage()
	{
		Application.OpenURL("https://url.kr/jlcx1i");
	}

	public void OnClickPrivateTermsPage()
	{

		Application.OpenURL("https://url.kr/2nvswo");
	}

	public void OnServiceTermValueChaned(bool isOn)
	{
		toggleAll.SetIsOnWithoutNotify(toggleServiceTerms.isOn && togglePrivateTerms.isOn);
	}
	public void OnPrivateTermValueChaned(bool isOn)
	{
		toggleAll.SetIsOnWithoutNotify(toggleServiceTerms.isOn && togglePrivateTerms.isOn);
	}

	public void OnAllTermsValueChaned(bool isOn)
	{
		toggleServiceTerms.isOn = isOn;
		togglePrivateTerms.isOn = isOn;
	}

	public void OnClickAgreement()
	{
		if (togglePrivateTerms.isOn == false || toggleServiceTerms.isOn == false)
		{
			PopAlert.Create("알림", "약관 동의 필수");
			//ToastUI.Instance.Enqueue("약관 동의 필수");
			return;
		}
		gameObject.SetActive(false);
		PlayerPrefs.SetInt(k_Terms, 1);
		PlayerPrefs.Save();
	}
}
