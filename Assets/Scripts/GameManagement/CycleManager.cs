using System;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    // Singleton
    public static CycleManager Instance { get; private set; }

    // Events
    public event EventHandler OnTimeChanged;
    public event EventHandler<OnCycleEndedEventHandler> OnCycleEnded;
    public class OnCycleEndedEventHandler : EventArgs
    {
        public bool crashed;
    }

    // Variables
    private float currentTime;
    private float maxTime = 12f;
    private int currentCycle;

    // Awake
    private void Awake()
    {
        // Handle Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Reset();
    }

    // Reset
    public void Reset()
    {
        currentTime = 0f;
        currentCycle = 0;
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
        if (currentTime >= maxTime)
        {
            currentTime = 0f;
            currentCycle += 1;
        }
    }

    // End Cycle
    /// <summary>
    /// End the cycle. If crashed = true, means it was ended from a lack of energy
    /// </summary>
    public void EndCycle(bool crashed = false)
    {
        currentTime = 0f;
        currentCycle += 1;

        OnCycleEnded?.Invoke(this, new OnCycleEndedEventHandler
        {
            crashed = crashed
        });

        OnTimeChanged?.Invoke(this, EventArgs.Empty);

        Debug.Log("Ended Current Cycle");
    }

    // Update Time
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
    public float GetCurrentTime() { return currentTime; }

    // Get Max Time
    public float GetMaxTime() { return maxTime; }

    // Get Current Cycle
    public int GetCurrentCycle() { return currentCycle; }
}
