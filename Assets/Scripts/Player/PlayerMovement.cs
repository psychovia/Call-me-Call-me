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

    private bool gravityOn;

    private bool isMoving = false;

    private Rigidbody rb;

    private Vector2 inputVector;
    private Vector3 moveDir;
    private float verticalInput;

    // Awake
    private void Awake()
    {
        Instance = this;

        if (Instance != this)
        {
            Debug.LogError("There are multiple PlayerMovement instances!");
        }

        rb = GetComponent<Rigidbody>();
    }

    // Start
    private void Start()
    {
        GameInput.Instance.OnSprintAction += GameInput_OnSprintAction;

        gravityOn = rb.useGravity; 
    }

    // Game Input- On Sprint Action
    private void GameInput_OnSprintAction(object sender, GameInput.OnSprintActionEventHandler e)
    {
        isSprinting = e.started;
    }

    // Update
    private void Update()
    {
        // Zero Gravity Movement uses physics so it lives in FixedUpdate

        // But since input occurs every frame, we will be catching inputs here
        inputVector = GameInput.Instance.GetInputVectorNormalized();
        verticalInput = GameInput.Instance.GetVerticalInput();
        moveDir = new Vector3(inputVector.x, 0.0f, inputVector.y);

        if (gravityOn)
        {
            HandleMovement();
        }
    }

    private void FixedUpdate()
    {
        // Only run this if gravity is off
        if (!gravityOn)
        {
            HandleZeroGravityMovement();
        }
    }

    // Handle Movement
    private void HandleMovement()
    {
        isMoving = moveDir != Vector3.zero;
        
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
        // Fail safe check for moveDir is not zero to avoid "Look rotation viewing vector is zero" error
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
    }
    
    private void HandleZeroGravityMovement()
    {
        // Getting 3D move dir info
        Vector3 moveDir3D = new Vector3(inputVector.x, verticalInput, inputVector.y);
        isMoving = moveDir3D != Vector3.zero;

        Debug.Log(moveDir3D);

        if (isMoving)
        {
            // No sprint speed right now since we are overloading leftshift for
            // both sprint and 3D down
            float targetSpeed = moveSpeed;

            // Normalizing 3D move dir
            rb.AddForce(moveDir3D.normalized * targetSpeed, ForceMode.Acceleration);

            // Rotation (We are only doing 2D rotation currently)
            if (moveDir != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, moveDir.normalized, Time.fixedDeltaTime * rotationSpeed);
            }
        }
    }

    // Is Moving
    public bool IsMoving()
    {
        return isMoving;
    }
}