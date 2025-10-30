using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    // Singleton
    public static MainMenuUI Instance { get; private set; }

    // References
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    // Awake
    private void Awake()
    {
        // Manage singleton
        if (Instance != this && Instance != null)
        {
            Debug.LogError("There is more than one MainMenuUI object!!!");
        }

        Instance = this;

        //play button
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.TestScene);
        });

        //settings button
        settingsButton.onClick.AddListener(() =>
        {
            Hide();
            SettingsUI.Instance.Show(Show); //when settings is closed, this will open back up
        });

        //quit button
        quitButton.onClick.AddListener(() =>
        {
            Hide();
            QuitConfirmationUI.Instance.Show(Show); //when confirmation is closed, this will open back up
        });
    }

    // Start
    private void Start()
    {
        Show();
    }

    // Show
    public void Show()
    {
        gameObject.SetActive(true);

        playButton.Select(); // start with play button selected
    }

    // Hide
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
