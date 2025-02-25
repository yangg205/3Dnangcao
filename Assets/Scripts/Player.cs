using UnityEngine;
using UnityEngine.UI;

public class PlayerPre : MonoBehaviour
{
    public Slider slider;
    private float value;

    private void Start()
    {
        LoadPref();
        slider.value = value;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        value = newValue;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void LoadPref()
    {
        value = PlayerPrefs.GetFloat("Volume", 100f);
    }

}