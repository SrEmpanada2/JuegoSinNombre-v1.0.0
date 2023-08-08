using UnityEngine;

public class AudioControler : MonoBehaviour
{
    public static AudioControler Instance;

    private AudioSource audioSource;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audio) {
        if (audio != null) {
            audioSource.PlayOneShot(audio);
        } else {
            Debug.LogWarning("Trying to play a null AudioClip!");
        }
    }
}
