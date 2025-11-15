using System;
using UnityEngine;
using UnityEngine.UI;

public class QuitConfirmationUI : MonoBehaviour
{
    // Singleton
    public static QuitConfirmationUI Instance { get; private set; }

    // References
    [SerializeField] private Button quitButton;
    [SerializeField] private Button cancelButton;

    private Action onCloseAction;

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

        // Quit Button
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
            Debug.LogWarning("Application Quit.");
        });

        // Cancel Button
        cancelButton.onClick.AddListener(() =>
        {
            onCloseAction();
            Hide();
        });
    }

    // Start
    private void Start()
    {
        Hide();
    }

    // Show
    public void Show(Action onCancelAction)
    {
        this.onCloseAction = onCancelAction;

        gameObject.SetActive(true);

        cancelButton.Select(); // start with cancel button selected
    }

    // Hide
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
