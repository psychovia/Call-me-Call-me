using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class MusicManager : MonoBehaviour
{
    // Singleton
    public static MusicManager Instance { get; private set; }

    // References
    [SerializeField] private MusicRefsSO musicRefsSO;

    // Variables
    private AudioSource audioSourceA;
    private AudioSource audioSourceB;
    private bool isAActive; // A starts as active

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

        // Initialize both audio sources (needed for fading)
        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();

        // Load saved volume
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSourceA.volume = volume;
        audioSourceB.volume = volume;
        audioSourceA.loop = true;
        audioSourceB.loop = true;
    }

    // Start
    private void Start()
    {
        SwitchClip(musicRefsSO.menuThemeIntro);
        Debug.Log(audioSourceA.isPlaying);
        Debug.Log(audioSourceB.isPlaying);
    }

    // On Enable
    private void OnEnable() { SceneLoader.OnSceneChanged += SceneLoader_OnSceneChanged; }

    // On Disable
    private void OnDisable() { SceneLoader.OnSceneChanged -= SceneLoader_OnSceneChanged; }
    
    // Scene Loader- On Scene Changed
    private void SceneLoader_OnSceneChanged(object sender, System.EventArgs e)
    {
        SceneLoader.Scene scene = SceneLoader.GetTargetScene();

        switch (scene)
        {
            // Menu Scene
            case (SceneLoader.Scene.MenuScene):
                SwitchClip(musicRefsSO.menuThemeIntro);
                CrossfadeClip(musicRefsSO.menuThemeLoop, 80, 3);
                break;
            // Stop if not a specific scene
            default:
                
                break;
        }
    }

    // Change Volume
    /// <summary>
    /// Changes the general volume of the music
    /// </summary>
    public void ChangeVolume(float volume)
    {
        this.volume = volume;

        audioSourceA.volume = volume;
        audioSourceB.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // Get Volume
    /// <summary>
    /// Returns the normalized volume level
    /// </summary>
    public float GetVolume() { return volume; }

    // Switch Clip
    /// <summary>
    /// Switches the music to a new clip and plays it
    /// </summary>
    private void SwitchClip(AudioClip clip)
    {
        // stop old music
        audioSourceA.Stop();
        audioSourceB.Stop();

        // start new music
        audioSourceA.clip = clip;
        audioSourceA.Play();
    }

    // Crossfade Clips
    private IEnumerable CrossfadeClip(AudioClip newClip, float fadeStart, float fadeDuration)
    {
        // get direction of audio sources
        AudioSource from = isAActive ? audioSourceA : audioSourceB;
        AudioSource to = isAActive ? audioSourceB : audioSourceA;

        // invert active audio source
        isAActive = !isAActive;

        to.clip = newClip;
        to.volume = 0f;
        to.Play();

        // Start crossfade
        float t = 0f;
        float startVol = from.volume;

        // Wait to start crossfade
        while (t < fadeStart)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // Crossfade
        while(t < fadeDuration)
        {
            t += Time.deltaTime;

            float normalized = (t-fadeStart) / fadeDuration;

            from.volume = Mathf.Lerp(startVol, 0f, normalized);
            to.volume = Mathf.Lerp(0f, startVol, normalized);

            yield return null;
        }

        // Crossfade is over
        from.Stop();
    }
}