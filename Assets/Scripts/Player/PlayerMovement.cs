using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Singleton
    public static PlayerMovement Instance { get; private set; }

    // Variables
    [Header("References")]
    [SerializeField] private GameObject playerVisual;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    private bool isWalking;
    private bool isSprinting;

    private bool isMoving = false;

    // Awake
    private void Awake()
    {
        Instance = this;

        if (Instance != this)
        {
            Debug.LogError("There are multiple PlayerMovement instances!");
        }
    }

    // Start
    private void Start()
    {
        GameInput.Instance.OnSprintAction += GameInput_OnSprintAction;
    }

    // Game Input- On Sprint Action
    private void GameInput_OnSprintAction(object sender, GameInput.OnSprintActionEventHandler e)
    {
        isSprinting = e.started;
    }

    // Update
    private void Update()
    {
        HandleMovement();
    }

    // Handle Movement
    private void HandleMovement()
    {
        // Get input vector
        Vector2 inputVector = GameInput.Instance.GetInputVectorNormalized();

        // Cast to 3D vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Move player
        if (isSprinting) // sprinting
        { 
            transform.position += moveDir * sprintSpeed * Time.deltaTime; //move player

            isWalking = false; //not walking
        }
        else if (moveDir != Vector3.zero) // normal speed and not standstill
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime; //move player

            isWalking = true; //walking
        }

        // check for is moving
        isMoving = moveDir != Vector3.zero ? true : false;

        // Rotation
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    // Is Moving
    public bool IsMoving()
    {
        return isMoving;
    }
}
