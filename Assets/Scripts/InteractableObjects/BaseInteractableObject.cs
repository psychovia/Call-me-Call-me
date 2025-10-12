using UnityEngine;

public class BaseInteractableObject : MonoBehaviour, I_Interactable
{
    // Variables
    [SerializeField] private GameObject selectedVisual;
    private bool isSelected;

    // Start
    private void Start()
    {
        selectedVisual.SetActive(false); //hide the selected visual
    }

    // Interact
    public virtual void Interact()
    {
        Debug.LogWarning("Base Interactable Object");
    }

    // Set Selected
    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;

        // enable/disable the visual based on if it's selected
        if (this.isSelected) { selectedVisual.SetActive(true); }
        else { selectedVisual.SetActive(false); }
    }

    // Is Selected
    public bool IsSelected()
    {
        return isSelected;
    }

    // Get Transform
    public Transform GetTransform()
    {
        return this.transform;
    }

    // On Destroy
    private void OnDestroy()
    {
        PlayerInteract.Instance.ClearClosestInteractable();
    }
}
