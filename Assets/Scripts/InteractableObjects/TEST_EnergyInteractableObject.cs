using UnityEngine;

public class TEST_EnergyInteractableObject : BaseInteractableObject
{
    // Interact
    public override void Interact()
    {
        EnergySystem.es.ReduceEnergy(3f);
    }
}
