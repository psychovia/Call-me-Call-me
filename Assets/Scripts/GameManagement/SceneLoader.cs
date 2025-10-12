using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Variables
    public enum Scene
    {
        EnergyCapsule,
        LivingCapsule,
        LoadingScene,
        TestScene,
    }

    private static Scene targetScene;

    // Load
    public static void Load(Scene targetScene)
    {
        SceneLoader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // Get Target Scene
    public static Scene GetTargetScene()
    {
        return SceneLoader.targetScene;
    }
}
