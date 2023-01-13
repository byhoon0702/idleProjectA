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
	[SerializeField] private TextMeshProUGUI okButtonText;
	[SerializeField] private TextMeshProUGUI cancelButtonText;


	[SerializeField] private Button okButton;
	[SerializeField] private Button cancelButton;
	[SerializeField] private Button xButton;
	[SerializeField] private Button panelButton;


	public ResultCodeData resultCodeData;
	private Action onOkCallback;
	private Action onCancelCallback;


	// 내부에서 ResultCode값이 null이어도 방어가 필요함
	public void Init(ResultCodeData _resultCodeData, Action _onOkCallback = null, Action _onCancelCallback = null)
	{
		resultCodeData = _resultCodeData;
		if(resultCodeData == null)
		{
			resultCodeData = MakeDefaultResultCodeData();
		}

		onOkCallback = _onOkCallback;
		onCancelCallback = _onCancelCallback;

		SetInteractable();
		UpdateUI();
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
		if(onCancelCallback != null)
		{
			onCancelCallback.Invoke();
			onOkCallback = null;
		}
		else if(onOkCallback != null)
		{
			onOkCallback.Invoke();
			onOkCallback = null;
		}

		Destroy(gameObject);
	}

	private void SetInteractable()
	{
		// 버튼 이벤트 등록
		xButton.onClick.RemoveAllListeners();
		xButton.onClick.AddListener(OnCancelButtonClick);

		panelButton.onClick.RemoveAllListeners();
		panelButton.onClick.AddListener(() => 
		{
			if (xButton.gameObject.activeInHierarchy)
			{
				OnCancelButtonClick();
			}
		});

		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(OnCancelButtonClick);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener(OnOkButtonClick);


		// 버튼 활성화 여부 처리
		xButton.gameObject.SetActive(resultCodeData.alertType != PopAlertType.ERROR);
		okButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(onCancelCallback != null);
	}

	private void UpdateUI()
	{
		if (string.IsNullOrEmpty(resultCodeData.title))
		{
			if (resultCodeData.alertType == PopAlertType.ERROR)
			{
				titleText.text = ConfigMeta.it.RESULT_CODE_ERROR_TITLE_TEXT;
			}
			else
			{
				titleText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_TITLE_TEXT;
			}
		}
		else
		{
			titleText.text = resultCodeData.title;
		}

		contentText.text = resultCodeData.content;

		if (string.IsNullOrEmpty(resultCodeData.okText))
		{
			okButtonText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_OK_TEXT;
		}
		else
		{
			okButtonText.text = resultCodeData.title;
		}

		if (string.IsNullOrEmpty(resultCodeData.cancelText))
		{
			cancelButtonText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_CANCEL_TEXT;
		}
		else
		{
			cancelButtonText.text = resultCodeData.title;
		}
	}

	private ResultCodeData MakeDefaultResultCodeData()
	{
		ResultCodeData resultCodeData = new ResultCodeData();

		resultCodeData.title = ConfigMeta.it.RESULT_CODE_ERROR_TITLE_TEXT;
		resultCodeData.content = $"정의되지 않은 오류입니다.";
		resultCodeData.okText = ConfigMeta.it.RESULT_CODE_DEFAULT_OK_TEXT;
		resultCodeData.alertType = PopAlertType.ERROR;

		return resultCodeData;
	}
}
