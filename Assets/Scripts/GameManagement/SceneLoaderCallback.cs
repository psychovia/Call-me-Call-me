using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderCallback : MonoBehaviour
{
    [SerializeField] private Image fillBar;

    // Start
    private void Start() => StartCoroutine(LoadNextScene());

    // Load Next Scene
    private IEnumerator LoadNextScene()
    {
        yield return null; // wait a frame to ensure loading scene is ready

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneLoader.GetTargetScene().ToString());

        while (!asyncLoad.isDone) // still loading
        {
            fillBar.fillAmount = asyncLoad.progress;
            yield return null;
        }
    }
}
