using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    private float defaultAudioVolume = 0;

    private void Start() {
        if (PlayerPrefs.HasKey("GameVolume")) {
            slider.value = PlayerPrefs.GetFloat("GameVolume");
        } else {
            slider.value = defaultAudioVolume;
            PlayerPrefs.SetFloat("GameVolume", defaultAudioVolume);
        }
    }

    public void ChangeVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("GameVolume", volume);
    }
}
