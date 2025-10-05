using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Singleton
    public static GameInput Instance { get; private set; }

    // Events
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler<OnSprintActionEventHandler> OnSprintAction;
    public class OnSprintActionEventHandler : EventArgs
    {
        public bool started;
    }

    // Variables
    private const string PLAYER_PREFS_BINDINGS = "PlayerPrefsBindings";

    public enum Bindings
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        Interact,
        Pause,
    }

    private InputSystem_Actions inputSystemActions;

    // Awake
    private void Awake()
    {
        // Handle Singleton
        Instance = this;

        if (Instance != this)
        {
            Debug.LogError("There is more than one Gmae Input");
        }

        // Initialize input
        inputSystemActions = new InputSystem_Actions();

        // Load Keybinds
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            inputSystemActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        // Enable player input system
        inputSystemActions.Player.Enable();

        // Subscribe to input events
        inputSystemActions.Player.Interact.performed += OnInteractPerformed;
        inputSystemActions.Player.Pause.performed += OnPausePerformed;
        inputSystemActions.Player.Sprint.performed += OnSprintPerformed;
        inputSystemActions.Player.Sprint.canceled += OnSprintCanceled;
    }

    // On Destroy
    private void OnDestroy()
    {
        // Unsubscribe frorm input events
        inputSystemActions.Player.Interact.performed -= OnInteractPerformed;
        inputSystemActions.Player.Pause.performed -= OnPausePerformed;
        inputSystemActions.Player.Sprint.performed -= OnSprintPerformed;
        inputSystemActions.Player.Sprint.canceled -= OnSprintCanceled;

        // Dispose of input system
        inputSystemActions.Dispose();
    }

    // On Sprint Performed
    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        OnSprintAction?.Invoke(this, new OnSprintActionEventHandler
        {
            started = true
        });
    }

    // On Sprint Canceled
    private void OnSprintCanceled(InputAction.CallbackContext obj)
    {
        OnSprintAction?.Invoke(this, new OnSprintActionEventHandler
        {
            started = false
        });
    }

    // On Interact Performed
    private void OnInteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // On Pause Performed
    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // Get Movement Vector Normalized
    public Vector2 GetInputVectorNormalized()
    {
        // Get the player input from input system
        Vector2 inputVector = inputSystemActions.Player.Move.ReadValue<Vector2>();

        // Normalize input vector 
        inputVector = inputVector.normalized;

        // Return input vector
        return inputVector;
    }
}
