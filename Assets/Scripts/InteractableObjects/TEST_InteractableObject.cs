using UnityEngine;

public class TEST_InteractableObject : BaseInteractableObject
{
    public override void Interact()
    {
        SceneLoader.Load(SceneLoader.Scene.TestScene);
    }
}
