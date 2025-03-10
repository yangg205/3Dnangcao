using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class Option : MonoBehaviour
{
    [Header("Volume Settings")]
    public Slider volumeSlider;  // The slider UI element to adjust volume
    public float minVolume = 0f; // Minimum volume level
    public float maxVolume = 1f; // Maximum volume level
    private const string VolumePrefKey = "GameVolume"; // Key for storing volume in PlayerPrefs
    public GameObject Panel;
    void Start()
    {
        Panel.SetActive(false);
        // Initialize the slider and volume
        if (volumeSlider != null)
        {
            // Load saved volume from PlayerPrefs or set to maxVolume if not found
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, maxVolume);
            volumeSlider.value = savedVolume; // Set slider value
            SetVolume(savedVolume);           // Set AudioListener volume

            // Add listener to handle volume change via the slider
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        else
        {
            Debug.LogError("Volume slider is not assigned in the Inspector.");
        }
    }

    public void SetVolume(float newVolume)
    {
        // Clamp the volume and set it
        newVolume = Mathf.Clamp(newVolume, minVolume, maxVolume);
        AudioListener.volume = newVolume;

        // Save the volume setting
        PlayerPrefs.SetFloat(VolumePrefKey, newVolume);
        PlayerPrefs.Save();

        Debug.Log($"Volume set to: {AudioListener.volume * 100}%");
    }
    public void Off()
    {
        Panel?.SetActive(false);
    }
    public void On()
    {
        Panel?.SetActive(true);
    }
}
