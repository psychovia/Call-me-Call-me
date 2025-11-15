using UnityEngine;

public class TEST_CycleInteractableObject : BaseInteractableObject
{
    // Interact
    public override void Interact()
    {
        CycleManager.Instance.UpdateTime(2f);
    }
}
