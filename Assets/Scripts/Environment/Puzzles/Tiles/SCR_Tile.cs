using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Tile : MonoBehaviour
{
    private SCR_TilePuzzle generator; //reference to generator script
    private MeshRenderer meshRenderer; //reference to attached renderer
    public bool isActive; //bool for status
    public int tileNum; //tile number in puzzle
    public bool isCorrect = false;
    void Start()
    {
        generator = GetComponentInParent<SCR_TilePuzzle>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            #region SEQUENCE
            if(generator.puzzleType == SCR_TilePuzzle.PuzzleType.SEQUENCE)
            {
                if(generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)
                {
                    isActive = true;
                    if(!isCorrect)
                    {
                        generator.puzzleAttempt.Add(tileNum);
                        generator.CheckPuzzleState(SCR_TilePuzzle.PuzzleType.SEQUENCE, gameObject); //print("CHECKING SOLUTION");
                    }
                }
            }
            #endregion
            #region COMPARISON
            else if (generator.puzzleType == SCR_TilePuzzle.PuzzleType.COMPARISON)
            {
                if(isActive)
                {
                    if(!isCorrect)
                    {
                        generator.puzzleState = PuzzleState.OK;
                        isActive = false;
                        meshRenderer.material = generator.debugBaseColor;

                        foreach (int value in generator.puzzleAttempt)
                        {
                            if (value == tileNum)
                                generator.puzzleAttempt.Remove(value);
                                print("Removed");
                                break;
                        }

                        if(generator.IsComparisonComplete())
                        {
                            generator.PuzzleSolved();
                        }
                    }
                }
                else
                {
                    if(generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)
                    {
                        isActive = true;
                        if(!isCorrect && !generator.puzzleAttempt.Contains(tileNum))
                        {
                            generator.puzzleAttempt.Add(tileNum);
                            generator.CheckPuzzleState(SCR_TilePuzzle.PuzzleType.COMPARISON, gameObject); //print("CHECKING SOLUTION");
                        }
                    }
                }
            }
            #endregion           
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if(generator.puzzleType == SCR_TilePuzzle.PuzzleType.SEQUENCE)
            {
                if(!isCorrect && (generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)) 
                    meshRenderer.material = generator.debugBaseColor;
                isActive = false;
            }
            else if(generator.puzzleType == SCR_TilePuzzle.PuzzleType.COMPARISON)
            {
                return;
            }      
        }
    }
}
