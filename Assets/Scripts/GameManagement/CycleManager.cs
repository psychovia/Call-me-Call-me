using System;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    // Singleton
    public static CycleManager Instance { get; private set; }

    // Events
    public event EventHandler OnTimeChanged;

    // References


    // Variables
    private float currentTime;
    private float currentCycle;

    private float timeInCycle;

    // Awake
    private void Awake()
    {
        currentTime = 0f;
        currentCycle = 0f;
    }

    // On Enable
    private void OnEnable()
    {
        OnTimeChanged += CM_OnTimeChanged;
    }

    // On Disable
    private void OnDisable()
    {
        OnTimeChanged -= CM_OnTimeChanged;
    }

    // On Time Changed
    private void CM_OnTimeChanged(object sender, EventArgs e)
    {
        // reset current time and increment cycle if time goes over limit
        if (currentTime >= timeInCycle)
        {
            currentTime = 0f;
            currentCycle += 1;
        }
    }

    // Add Time
    public void UpdateTime(float timeToAdd)
    {
        currentTime += timeToAdd;

        OnTimeChanged?.Invoke(this, EventArgs.Empty);
    }

    // Set Time
    public void SetTime(float newTime)
    {
        currentTime = newTime;

        OnTimeChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get Current Time
    public float GetCurrentTime()
    {
        return currentTime;
    }

    // Get Current Cycle
    public float GetCurrentCycle()
    {
        return currentCycle;
    }
}
