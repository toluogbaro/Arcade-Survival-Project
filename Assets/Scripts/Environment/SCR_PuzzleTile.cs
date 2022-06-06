using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PuzzleTile : MonoBehaviour
{
    private SCR_PuzzleGenerator generator; //reference to generator script
    private MeshRenderer meshRenderer; //reference to attached renderer
    public bool isActive; //bool for status
    public int tileNum; //tile number in puzzle
    public bool isCorrect = false;
    void Start()
    {
        generator = GetComponentInParent<SCR_PuzzleGenerator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider collider)
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
    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if(!isCorrect && (generator.puzzleState != PuzzleState.SOLVED || generator.puzzleState != PuzzleState.FAILED)) meshRenderer.material = generator.debugBaseColor;
            isActive = false;
        }
    }
}
