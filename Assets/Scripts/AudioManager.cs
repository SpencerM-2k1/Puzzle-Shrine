using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer audioMixer;
    
    public float ambienceVolume = 75.0f;
    public float sfxVolume = 75.0f;

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("GamestateManager initialized!");
            instance = this;
            loadVolume();
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // OnDestroy, clear the singleton
    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;        
        }
    }

    public static void SetAmbienceVolume(float volume)
    {
        instance.ambienceVolume = volume;
        instance.audioMixer.SetFloat("AmbienceVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
    }

    public static void SetSFXVolume(float volume)
    {
        instance.sfxVolume = volume;
        instance.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public static void loadVolume()
    {
        if (PlayerPrefs.HasKey("AmbienceVolume"))
        {
            float newAmbienceVolume = PlayerPrefs.GetFloat("AmbienceVolume");
            SetAmbienceVolume(newAmbienceVolume);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float newSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            SetAmbienceVolume(newSFXVolume);
        }
    }
}
