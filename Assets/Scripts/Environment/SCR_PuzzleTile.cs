using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PuzzleTile : MonoBehaviour
{
    private SCR_PuzzleGenerator generator; //reference to generator script
    private MeshRenderer meshRenderer; //reference to attached renderer
    public bool isActive; //bool for status
    public int tileNum; //tile number in puzzle
    void Start()
    {
        generator = GetComponentInParent<SCR_PuzzleGenerator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            meshRenderer.material = generator.debugActiveColor;
            isActive = true;

            generator.puzzleAttempt.Add(tileNum);
            generator.CheckPuzzleState();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            meshRenderer.material = generator.debugBaseColor;
            isActive = false;
        }
    }
}
