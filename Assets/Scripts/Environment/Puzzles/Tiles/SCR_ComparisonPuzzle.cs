using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCR_ComparisonPuzzle : SCR_TilePuzzle
{
    public ComparisonMode comparisonMode;
    [SerializeField]private Transform comparisonSolutionTrans;
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

        int tilesToRemove = (int)Mathf.Round(puzzleSolution.Count / 2);
        
        for(int i = 0; i < tilesToRemove; i++)
        {
            puzzleSolution.Remove(puzzleSolution.Last());
        }

        GenerateComparisonReference(gridX, gridZ, gridPadding, tilePrefab, tileSizeRef, comparisonSolutionTrans);
        comparisonSolutionTrans.rotation = Quaternion.Euler(0,0,90);
    }

    private void GenerateComparisonReference(int x, int z, float padding, GameObject tile, float tileSize, Transform anchor)
    {
        int tileRefCounter = 0;
        for (int loopA = 0; loopA < x; loopA++)
        {
            xOffset = loopA * (tileSize + padding);

            for (int loopB = 0; loopB < z; loopB++)
            {
                zOffset = loopB * (tileSize + padding);
                spawnPos = new Vector3(anchor.position.x + xOffset, anchor.position.y, anchor.position.z + zOffset);
                GameObject currentRefTile = Instantiate(tile, spawnPos, anchor.rotation);
                currentRefTile.GetComponent<SCR_Tile>().tileNum = tileRefCounter;
                currentRefTile.transform.parent = anchor;

                if(puzzleSolution.Contains(currentRefTile.GetComponent<SCR_Tile>().tileNum))
                {
                    currentRefTile.GetComponent<MeshRenderer>().material = debugActiveColor;
                }

                Destroy(currentRefTile.GetComponent<SCR_Tile>());
                tileRefCounter++;
            }
        }
    }
    public override void CheckPuzzleState(GameObject tileCheck)
    {
        if (puzzleSolution.Contains(tileCheck.GetComponent<SCR_Tile>().tileNum))
        {
            tileCheck.GetComponent<MeshRenderer>().material = debugActiveColor;
            tileCheck.GetComponent<SCR_Tile>().isCorrect = true;
        }
        else
        {
            tileCheck.GetComponent<MeshRenderer>().material = debugActiveColor;
            tileCheck.GetComponent<SCR_Tile>().isCorrect = false;
        }

        if (IsComparisonComplete())
        {
            PuzzleSolved();
        }
        
    }
public enum ComparisonMode
{
    NO_RESET
}
}
