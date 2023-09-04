//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;


//#if UNITY_EDITOR
//public class AnimationEditorPanel : BaseEditorPanel
//{

//	public Button animationPlayButton;

//	public TMP_Dropdown animationDropdown;
//	public Slider animationSpeedSlider;
//	public Slider attackTimeSlider;
//	public TextMeshProUGUI animationSpeedSliderText;
//	public TextMeshProUGUI animationSpeedMinText;
//	public TextMeshProUGUI animationSpeedMaxText;

//	public TextMeshProUGUI attackTimeSliderText;
//	public TextMeshProUGUI attackTimeMinText;
//	public TextMeshProUGUI attackTimeMaxText;


//	private List<UnitData> unitdata;


//	List<string> animationList = new List<string>();

//	public override void Init(EditorToolUI _editorToolUI)
//	{
//		base.Init(_editorToolUI);

//	}

//	private void Update()
//	{
//		animationSpeedSlider.gameObject.SetActive(viewTarget != null);
//		animationDropdown.gameObject.SetActive(viewTarget != null);
//		animationPlayButton.gameObject.SetActive(viewTarget != null);

//		attackTimeSlider.gameObject.SetActive(viewTarget != null);

//		if (viewTarget != null)
//		{
//			UnitEditor.it.SetAnimationSpeed(animationSpeedSlider.value);
//			UnitEditor.it.SetAttackTime(attackTimeSlider.value);
//		}
//	}


//	//public override void SetUnit(EditorUnit _unit, UnityEditor.Animations.AnimatorController _animatorController)
//	//{
//	//	viewTarget = _unit;

//	//	animationList.Clear();
//	//	animatorController = _animatorController;
//	//	foreach (var layer in animatorController.layers)
//	//	{
//	//		foreach (var state in layer.stateMachine.states)
//	//		{
//	//			AnimationClip clip = state.state.motion as AnimationClip;

//	//			//attackTimeSlider.minValue = 0;
//	//			//attackTimeSlider.maxValue = clip.length;
//	//			animationList.Add($"{layer.name}:{state.state.name}");
//	//		}
//	//	}

//	//	animationDropdown.ClearOptions();
//	//	animationDropdown.AddOptions(animationList);
//	//}
//	public void SetSliderValue(float value)
//	{
//		animationSpeedSliderText.text = $"Current Speed : {value}";
//		animationSpeedMinText.text = $"{animationSpeedSlider.minValue}";
//		animationSpeedMaxText.text = $"{animationSpeedSlider.maxValue}";
//	}

//	public void SetAttackTimeSliderValue(float value)
//	{
//		attackTimeSliderText.text = $"Attack Time: {value}";
//		attackTimeMinText.text = $"{attackTimeSlider.minValue}";
//		attackTimeMaxText.text = $"{attackTimeSlider.maxValue}"; ;
//	}


//	public void OnClickPlayAnimation()
//	{
//		string[] splitname = animationList[animationDropdown.value].Split(':');

//		foreach (var layer in animatorController.layers)
//		{
//			foreach (var state in layer.stateMachine.states)
//			{
//				if (state.state.name.Contains("attack"))
//				{
//					AnimationClip clip = state.state.motion as AnimationClip;

//					attackTimeSlider.minValue = 0;
//					attackTimeSlider.maxValue = clip.length;
//					SetAttackTimeSliderValue(0);
//					animationList.Add($"{layer.name}:{state.state.name}");
//				}
//			}
//		}

//		if (attackTimeSlider.value == 0)
//		{
//			attackTimeSlider.value = 0;
//		}
//		attackTimeSliderText.text = $"Attack Time: {attackTimeSlider.value}";
//		UnitEditor.it.OnClickPlayAnimation(animationList[animationDropdown.value]);
//	}


//	public void SetAnimationSpeed(string speed)
//	{
//		if (float.TryParse(speed, out float value))
//		{
//			animationSpeedSlider.value = value;
//		}
//	}

//	public void SetAttackTime(string time)
//	{
//		if (float.TryParse(time, out float value))
//		{
//			attackTimeSlider.value = value;
//		}
//	}

//	public void OnClickUnitDataModify()
//	{

//	}
//}
//#endif
