using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    // Singleton
    public static PauseUI Instance { get; private set; }

    // References
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    // Awake
    private void Awake()
    {
        // Manage singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //play button
        resumeButton.onClick.AddListener(() =>
        {
            Hide();
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

        //menu button
        menuButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scene.MenuScene); //load menu scene
        });
    }

    // Start
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        Hide();
    }

    // GameInput- On Pause Action
    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    // Show
    public void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select(); // start with play button selected
    }

    // Hide
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
