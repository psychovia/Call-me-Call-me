using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton
    public static MusicManager Instance { get; private set; }

    // Variables
    private AudioSource audioSource;

    private float volume = .3f;

    // Const Variables
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Awake
    private void Awake()
    {
        // Handle Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        // Load saved volume
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume;
    }

    // Start
    private void Start()
    {
        audioSource.Play();
    }

    // Change Volume
    /// <summary>
    /// Changes the general volume of the music
    /// </summary>
    public void ChangeVolume(float volume)
    {
        this.volume = volume;

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // Get Volume
    /// <summary>
    /// Returns the normalized volume level
    /// </summary>
    public float GetVolume()
    {
        return volume;
    }
}