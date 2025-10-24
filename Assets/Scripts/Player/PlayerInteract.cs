using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Singleton
    public static PlayerInteract Instance { get; private set; }

    // Variables
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private bool showDebugSphere = true;

    private I_Interactable closestInteractable;
    private float closestDist;

    private Collider[] interactablesInRange;

    // Awake
    private void Awake()
    {
        Instance = this;

        if (Instance != this)
        {
            Debug.LogError("There are multiple player interact objects!");
        }
    }

    // Start
    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        ClearClosestInteractable();
    }

    // Update
    private void Update()
    {
        // dont check for new interactables if the player isn't moving
        if (PlayerMovement.Instance.IsMoving())
        {
            CheckForInteractables();
        }
    }

    // On Draw Gizmos
    private void OnDrawGizmos()
    {
        if (!showDebugSphere) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    // Check For Interactables
    private void CheckForInteractables()
    {
        interactablesInRange = Physics.OverlapSphere(transform.position, interactRange, interactableMask);

        // no interactables in range
        if (interactablesInRange.Length == 0)
        {
            ClearClosestInteractable();
            return;
        }

        // Loop through the potential interactable objects
        foreach (Collider hit in interactablesInRange)
        {
            // Make sure the object has interface
            if (hit.TryGetComponent<I_Interactable>(out var interactable))
            {
                // Get distance between interactable and player
                float dist = Vector3.Distance(transform.position, hit.ClosestPoint(transform.position));

                if (interactable == closestInteractable) // current closest
                {
                    closestDist = dist;
                    continue;
                }

                // Check for new smallest distance
                else if (dist <= closestDist)
                {
                    if (closestInteractable != null) //unselect old if old exists
                        closestInteractable.SetSelected(false);
                    
                    //set new
                    closestDist = dist;
                    closestInteractable = interactable;
                }
            }
            else //object doesn't have the interface
            {
                Debug.LogError(hit.name + " is marked as interactable but does not have interface.");
            }

            // set new closest to selected
            closestInteractable.SetSelected(true);
        }
    }

    // Clear Closest Interactable
    public void ClearClosestInteractable()
    {
        if (closestInteractable != null)
            closestInteractable.SetSelected(false);

        closestInteractable = null;
        closestDist = interactRange + 1;
    }

    // Get Closest Interactable
    public I_Interactable GetClosestInteractable()
    {
        return closestInteractable;
    }

    // GameInput- On Interact Action
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        // if there is an interactable in range, interact with it
        if (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }
}