using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    // Singleton
    public static SettingsUI Instance { get; private set; }

    // Const
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // References
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton, moveDownButton, moveLeftButton, moveRightButton, interactButton, pauseButton;
    [SerializeField] private Button gamepadInteractButton, gamepadPauseButton;
    [SerializeField] private Slider sfxSlider, musicSlider;
    [SerializeField] private TextMeshProUGUI sfxText, musicText;
    [SerializeField] private TextMeshProUGUI moveUpText, moveDownText, moveLeftText, moveRightText, interactText, pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText, gamepadPauseText;
    [SerializeField] private GameObject pressToRebindKeyScreen;

    // Variables
    private Action onCloseAction;


    // Awake
    private void Awake()
    {
        // Manage singleton
        if (Instance != this && Instance != null)
        {
            Debug.LogError("There is more than one SettingsUI object!!!");
        }

        Instance = this;
        
        // Sound Effects Slider
        sfxSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
        sfxSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.ChangeVolume(value);
            UpdateVisual();
        });

        // Music Slider
        musicSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        musicSlider.onValueChanged.AddListener(value =>
        {
            MusicManager.Instance.ChangeVolume(value);
            UpdateVisual();
        });

        // Close Button On Click
        closeButton.onClick.AddListener(() =>
        {
            Hide();

            onCloseAction();
        });

        // Key Rebinding
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });

        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamepadInteract); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamepadPause); });
    }

    // Start
    private void Start()
    {
        UpdateVisual();
        Hide();
        HidePressToRebindKeyScreen();
    }

    // Update Visual
    private void UpdateVisual()
    {
        // Sound Effects Visual
        sfxText.text = Mathf.Ceil(sfxSlider.value * 100).ToString();

        // Music Visual
        musicText.text = Mathf.Ceil(musicSlider.value * 100).ToString();

        // Binding Text
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadInteract);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamepadPause);
    }

    // Show
    public void Show(Action onCloseAction)
    {
        this.onCloseAction = onCloseAction;

        gameObject.SetActive(true);

        sfxSlider.Select(); // start with sound effects slider selected for gamepads
    }

    // Hide
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Show Press to Rebind Key Screen
    private void ShowPressToRebindKeyScreen()
    {
        pressToRebindKeyScreen.SetActive(true);
    }

    // Hide Press to Rebind Key Screen
    private void HidePressToRebindKeyScreen()
    {
        pressToRebindKeyScreen.SetActive(false);
    }

    // Rebind Binding
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKeyScreen();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKeyScreen();
            UpdateVisual();
        });
    }
}
