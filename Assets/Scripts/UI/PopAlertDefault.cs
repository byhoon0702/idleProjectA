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
	[SerializeField] GameObject descRoot;
	[SerializeField] private TextMeshProUGUI descText;


	[SerializeField] private Button okButton;
	[SerializeField] private Button cancelButton;
	[SerializeField] private Button xButton;
	[SerializeField] private Button panelButton;


	public VResult result;
	private Action onOkCallback;
	private Action onCancelCallback;


	// 내부에서 ResultCode값이 null이어도 방어가 필요함
	public void Init(VResult _result, Action _onOkCallback = null, Action _onCancelCallback = null)
	{
		result = _result;
		VLog.Log($"{result.data.key}({result.data.tid}) : {result.data.description}. \n{result.description}");

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
		xButton.gameObject.SetActive(result.data.alertType != PopAlertType.ERROR);
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
				titleText.text = ConfigMeta.it.RESULT_CODE_ERROR_TITLE_TEXT;
			}
			else
			{
				titleText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_TITLE_TEXT;
			}
		}
		else
		{
			titleText.text = result.data.title;
		}

		// 내용
		contentText.text = result.ContentsText;


		// 리절트 코드
		if(UnityEnv.HouseMode)
		{
			resultCodeText.text = $"result.data.tid.ToString() : {result.data.key}";
		}
		else
		{
			resultCodeText.text = result.data.tid.ToString();
		}


		// 디스크립션
		descRoot.SetActive(UnityEnv.HouseMode);
		descText.text = result.description;


		// 버튼
		if (string.IsNullOrEmpty(result.data.okText))
		{
			okButtonText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_OK_TEXT;
		}
		else
		{
			okButtonText.text = result.data.title;
		}

		if (string.IsNullOrEmpty(result.data.cancelText))
		{
			cancelButtonText.text = ConfigMeta.it.RESULT_CODE_DEFAULT_CANCEL_TEXT;
		}
		else
		{
			cancelButtonText.text = result.data.title;
		}
	}
}
