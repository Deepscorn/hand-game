using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderText : MonoBehaviour
{
	[SerializeField] 
	private Slider slider;

	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		text.text = GetFormattedSliderValue(slider.value);
		slider.onValueChanged.AddListener(OnSliderChanged);
	}

	private void OnDisable()
	{
		slider.onValueChanged.RemoveListener(OnSliderChanged);
	}

	private void OnSliderChanged(float value)
	{
		text.text = GetFormattedSliderValue(value);
	}

	private static string GetFormattedSliderValue(float value)
	{
		return string.Format("{0:0.00}", value);
	}
}
