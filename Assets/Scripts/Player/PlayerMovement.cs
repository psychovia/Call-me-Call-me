using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Singleton
    public static PlayerMovement Instance { get; private set; }

    // Variables
    [Header("References")]
    [SerializeField] private GameObject playerVisual;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float collisionDist = .7f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private LayerMask collisionLayerMask;

    private float playerHeight;
    private Vector3 playerCenter;
    private Vector3 playerTop;

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

        playerHeight = transform.localScale.y * 2; // set player height
        playerCenter = transform.position + new Vector3(0f, playerHeight / 2, 0f);
        playerTop = transform.position + new Vector3(0f, playerHeight, 0f);
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

        // handle player center and top
        playerCenter = transform.position + new Vector3(0f, playerHeight / 2, 0f);
        playerTop = transform.position + new Vector3(0f, playerHeight, 0f);
    }

    // Handle Movement
    /// <summary>
    /// Moves the player if it is able to move in the direction of the players input,
    /// as well as slerps the rotation of the player
    /// </summary>
    private void HandleMovement()
    {
        // Get input vector
        Vector2 inputVector = GameInput.Instance.GetInputVectorNormalized();

        // Cast to 3D vector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        float moveDistance = moveSpeed * Time.deltaTime;

        // Move player
        if (CanMove(ref moveDir))
        {
            MovePlayer(moveDir, moveDistance);
        }

        // check for is moving
        isMoving = moveDir != Vector3.zero ? true : false;

        // Rotation
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    // Can Move Distance
    /// <summary>
    /// Uses capsule casts, the player position, and the player radius to determine if the player 
    /// can move in a certain direction. If not, it projects the move direction along the obstruction
    /// and checks if it can now move in that direction.
    /// </summary>
    private bool CanMove(ref Vector3 moveDir)
    {
        Debug.DrawRay(playerCenter, moveDir, Color.green, 2f);
        // something in the way
        if (Physics.CapsuleCast(transform.position, playerTop, playerRadius, 
                                moveDir, out RaycastHit hit, collisionDist))
        {
            // project along the thing in the way
            Vector3 adjustedDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;

            // can move if nothing in that direction box cast
            if ( !Physics.CapsuleCast(transform.position, playerTop, playerRadius, 
                                      adjustedDir, collisionDist, collisionLayerMask))
            {
                moveDir = adjustedDir;
                return true;
            }

            // check only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            bool tryingToMoveX = Mathf.Abs(moveDir.x) > 0.5f;
            bool canMoveX = !Physics.CapsuleCast(transform.position, playerTop, playerRadius, 
                                                 moveDirX, collisionDist, collisionLayerMask);
            if (tryingToMoveX && canMoveX)
            {
                moveDir = moveDirX;
                return true;
            }

            // check Z movement
            Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
            bool tryingToMoveZ = Mathf.Abs(moveDir.z) > 0.5f;
            bool canMoveZ = !Physics.CapsuleCast(transform.position, playerTop, playerRadius, 
                                                 moveDirZ, collisionDist, collisionLayerMask);
            if (tryingToMoveZ && canMoveZ)
            {
                moveDir = moveDirZ;
                return true;
            }

            // cant move at all
            return false;

        }

        // nothing in the way
        return true;
    }

    // Move Player
    /// <summary>
    /// Modifies the transform of the player directly by adding the moveDir * moveDist
    /// </summary>
    private void MovePlayer(Vector3 moveDir, float moveDist)
    {
        transform.position += moveDir * moveDist;
    }

    // Is Moving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Get Player Top
    public Vector3 GetPlayerTop()
    {
        return playerTop;
    }

    // Get Player Center
    public Vector3 GetPlayerCenter()
    {
        return playerCenter;
    }
}
