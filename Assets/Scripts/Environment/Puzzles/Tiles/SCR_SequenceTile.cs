using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SequenceTile : SCR_Tile
{
    private SCR_TilePuzzle generator; //reference to generator script
    private MeshRenderer meshRenderer; //reference to attached renderer
    void Start()
    {
        generator = GetComponentInParent<SCR_SequencePuzzle>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if(generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)
            {
                isActive = true;
                if(!isCorrect)
                {
                    generator.puzzleAttempt.Add(tileNum);
                    generator.CheckPuzzleState(gameObject); //print("CHECKING SOLUTION");
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if(!isCorrect && (generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)) 
                meshRenderer.material = generator.debugBaseColor;
            isActive = false;  
        }
    }
}
