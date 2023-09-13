using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PopAlertDefault : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI contentText;
	[SerializeField] private TextMeshProUGUI resultCodeText;
	[SerializeField] private TextMeshProUGUI okButtonText;
	[SerializeField] private TextMeshProUGUI cancelButtonText;

	[SerializeField] private Button okButton;
	[SerializeField] private Button cancelButton;

	[SerializeField] private Button panelButton;

	public VResult result;
	private Action onOkCallback;
	private Action onCancelCallback;

	// 내부에서 ResultCode값이 null이어도 방어가 필요함
	public void Init(string title, string desc, string okButtonStr = "str_ui_ok", string cancelButtonStr = "str_ui_cancel", Action _onOkCallback = null, Action _onCancelCallback = null)
	{
		titleText.text = title;
		contentText.text = desc;

		onOkCallback = _onOkCallback;
		onCancelCallback = _onCancelCallback;

		okButtonText.text = PlatformManager.Language[okButtonStr];
		cancelButtonText.text = PlatformManager.Language[cancelButtonStr];
		SetInteractable();
	}

	public void Init(VResult _result, Action _onOkCallback = null, Action _onCancelCallback = null)
	{
		result = _result;

		onOkCallback = _onOkCallback;
		onCancelCallback = _onCancelCallback;

		SetInteractable();
		UpdateUI();
	}
	public void SetButtonText(string ok, string cancel)
	{
		okButtonText.text = ok;
		cancelButtonText.text = cancel;
	}

	private void OnCancelButtonClick()
	{
		onCancelCallback?.Invoke();

		onOkCallback = null;
		onCancelCallback = null;

		Close();
	}

	private void OnOkButtonClick()
	{
		onOkCallback?.Invoke();

		onOkCallback = null;
		onCancelCallback = null;

		Close();
	}

	public void Close()
	{
		if (onCancelCallback != null)
		{
			onCancelCallback.Invoke();
			onOkCallback = null;
		}
		else if (onOkCallback != null)
		{
			onOkCallback.Invoke();
			onOkCallback = null;
		}

		gameObject.SetActive(false);
	}

	private void SetInteractable()
	{
		// 버튼 이벤트 등록
		panelButton.onClick.RemoveAllListeners();
		panelButton.onClick.AddListener(() =>
		{

		});

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(OnCancelButtonClick);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(OnOkButtonClick);


		okButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(onCancelCallback != null);
	}

	private void UpdateUI()
	{

		// 제목
		if (string.IsNullOrEmpty(result.data.title))
		{
			if (result.data.alertType == PopAlertType.ERROR)
			{
				titleText.text = GameManager.Config.RESULT_CODE_ERROR_TITLE_TEXT;
			}
			else
			{
				titleText.text = GameManager.Config.RESULT_CODE_DEFAULT_TITLE_TEXT;
			}
		}
		else
		{
			titleText.text = result.data.title;
		}

		// 내용
		contentText.text = result.ContentsText;


		// 리절트 코드
		if (UnityEnv.HouseMode)
		{
			resultCodeText.text = $"{result.data.tid.ToString()} : {result.data.key}";
		}
		else
		{
			resultCodeText.text = result.data.tid.ToString();
		}

		// 버튼
		if (string.IsNullOrEmpty(result.data.okText))
		{
			okButtonText.text = GameManager.Config.RESULT_CODE_DEFAULT_OK_TEXT;
		}
		else
		{
			okButtonText.text = result.data.title;
		}

		if (string.IsNullOrEmpty(result.data.cancelText))
		{
			cancelButtonText.text = GameManager.Config.RESULT_CODE_DEFAULT_CANCEL_TEXT;
		}
		else
		{
			cancelButtonText.text = result.data.title;
		}
	}
}
