using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton
    public static SoundManager Instance { get; private set; }

    // References
    [SerializeField] private SFXRefsSO sfxRefsSO;

    // Variables
    private float volume = 1f;

    // Const Variables
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    // Awake
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Debug.LogError("There is more than one Sound Manager Instance!");
        }

        Instance = this;

        // Load saved volume
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    // Start
    private void Start()
    {
        // add events that will play audio sounds here
    }

    

    // Play Sound
    /// <summary>
    /// Plays a sound effect at the defined position
    /// </summary>
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    /// <summary>
    /// Play random version of a sound effect
    /// </summary>
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    // Play Footsteps Sound
    /// <summary>
    /// Play footstep sound effect
    /// </summary>
    /// <param name="position">Position that the footstep sound originates from</param>
    /// <param name="volumeMultiplier">Volume of the footstep sound</param>
    public void PlayFootstepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(sfxRefsSO.footstep, position, volume);
    }

    // Change Volume
    /// <summary>
    /// Changes the volume of general sounds
    /// </summary>
    public void ChangeVolume(float volume)
    {
        this.volume = volume;

        // Save volume when closing application
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
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
