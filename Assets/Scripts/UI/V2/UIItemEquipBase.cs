using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemEquipBase : MonoBehaviour
{
	[SerializeField] private GameObject selectedObj;
	[SerializeField] private Image iconImage;
	[SerializeField] private Text levelText;
	[SerializeField] private Slider slider;
	[SerializeField] private TextMeshProUGUI sliderText;
	[SerializeField] private GameObject equipedObj;
	[SerializeField] private GameObject[] gradeList;
	[SerializeField] private GameObject[] starList;
	[SerializeField] private GameObject limitObj;
	[SerializeField] private TextMeshProUGUI limitText;
	[SerializeField] private GameObject sssLimitObj;
	[SerializeField] private TextMeshProUGUI sssLimitText;
	[SerializeField] private GameObject noPossessedObj;
}
