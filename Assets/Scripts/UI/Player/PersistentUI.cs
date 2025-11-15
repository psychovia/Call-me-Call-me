using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    // Singleton
    private static PersistentUI Instance;

    // Awake
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //destroy dupes
        }
    }
}
