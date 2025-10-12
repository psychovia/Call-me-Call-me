using UnityEngine;

public interface I_Interactable
{
    public void Interact();

    public Transform GetTransform();

    public void SetSelected(bool isSelected);
}
