using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ComparisonTile : SCR_Tile
{
    private SCR_TilePuzzle generator; //reference to generator script
    private MeshRenderer meshRenderer; //reference to attached renderer
    void Start()
    {
        generator = GetComponentInParent<SCR_ComparisonPuzzle>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if(isActive)
            {
                isActive = false;
                meshRenderer.material = generator.debugBaseColor;
                generator.puzzleAttempt.Remove(tileNum);
                if(!generator.puzzleSolution.Contains(tileNum))
                {
                    isCorrect = true;
                }
                
                if(generator.IsComparisonComplete())
                    generator.PuzzleSolved();

            }
            else
            {
                if(generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)
                {
                    isActive = true;
                    if(!generator.puzzleAttempt.Contains(tileNum))
                        generator.puzzleAttempt.Add(tileNum);
                    generator.CheckPuzzleState(gameObject);
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            return;
        }
        
    }
}
