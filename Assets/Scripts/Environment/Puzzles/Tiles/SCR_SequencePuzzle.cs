using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCR_SequencePuzzle : SCR_TilePuzzle
{
    public SequenceMode sequenceMode;
    [SerializeField] private int failCount = 0;
    [SerializeField] private int failThreshhold = 3;
    protected override void GeneratePuzzle()
    {
        int count = 0;
        puzzleSolution = new List<int>();
        foreach(GameObject tile in tiles)
        {
            puzzleSolution.Add(count);
            count++;
        }
        puzzleSolution = puzzleSolution.OrderBy(i => Guid.NewGuid()).ToList();
    }

    public override void CheckPuzzleState(GameObject tileCheck)
    {
        for (int i = 0; i < puzzleAttempt.Count; i++)
        {
            if (puzzleAttempt[i] == puzzleSolution[i])
            {
                tileCheck.GetComponent<MeshRenderer>().material = debugActiveColor;
                tileCheck.GetComponent<SCR_Tile>().isCorrect = true;
                if (puzzleAttempt.Last() == puzzleSolution.Last())
                {
                    failCount = 0;
                }
            }
            else
            {
                if(sequenceMode == SequenceMode.RESET_ON_FAIL)
                {
                    puzzleState = PuzzleState.FAILED;
                    puzzleAttempt = new List<int>();
                    foreach (GameObject tile in tiles)
                    {
                        tile.GetComponent<SCR_Tile>().isCorrect = false;
                    }
                    StartCoroutine(DebugPuzzleIndicator(debugFailedColor));
                    StartCoroutine(PuzzleSolvedCooldown());
                }
                else if (sequenceMode == SequenceMode.NO_RESET)
                {
                    puzzleAttempt.Remove(puzzleAttempt.Last());
                    tileCheck.GetComponent<MeshRenderer>().material = debugFailedColor;
                    tileCheck.GetComponent<SCR_Tile>().isCorrect = false;
                }
                else if (sequenceMode == SequenceMode.RESET_AFTER_X)
                {
                    puzzleState = PuzzleState.FAILED;
                    if(failCount <= failThreshhold)
                    {
                        puzzleAttempt.Remove(puzzleAttempt.Last());
                        tileCheck.GetComponent<MeshRenderer>().material = debugFailedColor;
                        tileCheck.GetComponent<SCR_Tile>().isCorrect = false;
                        if(puzzleAttempt.Count > 0) failCount++;
                    }
                    else
                    {
                        puzzleState = PuzzleState.FAILED;
                        puzzleAttempt = new List<int>();
                        StartCoroutine(DebugPuzzleIndicator(debugFailedColor));
                        foreach (GameObject tile in tiles)
                        {
                            tile.GetComponent<SCR_Tile>().isCorrect = false;
                        }
                        failCount = 0;
                    }
                }
            }
        }

        if(puzzleAttempt.Count == puzzleSolution.Count)
            {
                PuzzleSolved();
            }
        
    }
}
public enum SequenceMode
{
    RESET_ON_FAIL,
    RESET_AFTER_X,
    NO_RESET
}
