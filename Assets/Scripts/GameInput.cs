using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    // Singleton
    public static GameInput Instance { get; private set; }

    // Events
    public event EventHandler OnBindingRebind;
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler<OnSprintActionEventHandler> OnSprintAction;
    public class OnSprintActionEventHandler : EventArgs
    {
        public bool started;
    }

    // Const Variables
    private const string PLAYER_PREFS_BINDINGS = "PlayerPrefsBindings";

    // Variables
    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        GamepadInteract,
        Pause,
        GamepadPause,
    }

    private InputSystem_Actions inputSystemActions;

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
    
    public float GetVerticalInput()
    {
        float verticalInput = 0f;

        // Check if the "MoveUp" action is pressed
        if (inputSystemActions.Player.ZeroGUp.IsPressed())
        {
            verticalInput += 1f;
        }

        // Check if the "MoveDown" action is pressed
        if (inputSystemActions.Player.ZeroGDown.IsPressed())
        {
            verticalInput -= 1f;
        }

        return verticalInput;
    }

    // Get Binding Text
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            // Move Up
            case Binding.MoveUp:
                return inputSystemActions.Player.Move.bindings[1].ToDisplayString();
            // Move Down
            case Binding.MoveDown:
                return inputSystemActions.Player.Move.bindings[2].ToDisplayString();
            // Move Left
            case Binding.MoveLeft:
                return inputSystemActions.Player.Move.bindings[3].ToDisplayString();
            // Move Right
            case Binding.MoveRight:
                return inputSystemActions.Player.Move.bindings[4].ToDisplayString();
            // Interact
            case Binding.Interact:
                return inputSystemActions.Player.Interact.bindings[0].ToDisplayString();
            // Pause
            case Binding.Pause:
                return inputSystemActions.Player.Pause.bindings[0].ToDisplayString();
            // Gamepad Interact
            case Binding.GamepadInteract:
                return inputSystemActions.Player.Interact.bindings[1].ToDisplayString();
            // Gamepad Pause
            case Binding.GamepadPause:
                return inputSystemActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    // Rebind Binding
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        inputSystemActions.Player.Disable();

        // Get binding to be rebound
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = inputSystemActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = inputSystemActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = inputSystemActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = inputSystemActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = inputSystemActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = inputSystemActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamepadInteract:
                inputAction = inputSystemActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamepadPause:
                inputAction = inputSystemActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        // Rebind key
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                // Manually dispose callback
                callback.Dispose();

                // Reenable input
                inputSystemActions.Player.Enable();

                onActionRebound();

                // Save the new keybinds
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputSystemActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
