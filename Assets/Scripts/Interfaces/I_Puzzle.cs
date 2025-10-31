using System;
using UnityEngine;

public interface I_Puzzle
{
    // On Puzzle Completed
    public event EventHandler OnPuzzleCompleted;

    // Start Puzzle
    public void StartPuzzle();

    // End Puzzle
    public void EndPuzzle();

    // Is Solved
    public void IsSolved();
}
