using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static event EventHandler OnSceneChanged;

    // Variables
    public enum Scene
    {
        MenuScene,
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

    // Invoke On Scene Changed
    public static void InvokeOnSceneChanged()
    {
        OnSceneChanged?.Invoke(null, EventArgs.Empty);
    }
}
