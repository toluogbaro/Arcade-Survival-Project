using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCR_TilePuzzle : MonoBehaviour
{
    //Serialized Variables
    //Generation
    [SerializeField] private int gridZ = 3; //grid size of tiles in Z axis
    [SerializeField] private int gridX = 3; //grid size of tiles in X axis
    [SerializeField] private float gridPadding = 0.5f; //padding between tiles
    [SerializeField] private float tileSizeRef = 2f; //reference for the size of the puzzle tile
    [SerializeField] private GameObject tilePrefab; //prefab of puzzle tile
    [SerializeField] private Transform puzzleAnchorTrans; //transform of puzzle anchor *SUBJECT TO CHANGE
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    //Puzzle
    public enum PuzzleType{SEQUENCE, COMPARISON};
    public PuzzleType puzzleType;
    public SequenceMode sequenceMode;
    public ComparisonMode comparisonMode;
    [SerializeField] private Transform comparisonSolutionTrans;
    public Material debugBaseColor;
    public Material debugActiveColor;
    public Material debugFailedColor;
    public Material debugSolvedColor;
    [SerializeField] private int failThreshhold = 3;
    public List<int> puzzleSolution= new List<int>();


    //Non-Serialized Variables
    private Vector3 spawnPos = Vector3.zero; //reference for the current spawn position
    private GameObject currentTile; //reference to current tile in generation
    private float xOffset = 0f; //spawn offset for X axis
    private float zOffset = 0f; //spawn offset for Z axis
    private int tileCounter = 0;
    public List<int> puzzleAttempt = new List<int>();
    [HideInInspector] public PuzzleState puzzleState;
    private int failCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles(gridX, gridZ, gridPadding, tilePrefab, tileSizeRef, puzzleAnchorTrans);
        
        if(puzzleType == PuzzleType.SEQUENCE)
            GenerateSequencePuzzle();
        else if (puzzleType == PuzzleType.COMPARISON)
            GenerateComparisonPuzzle();
    }

    private void GenerateTiles(int x, int z, float padding, GameObject tile, float tileSize, Transform anchor)
    {
        for (int loopA = 0; loopA < x; loopA++)
        {
            xOffset = loopA * (tileSize + padding);

            for (int loopB = 0; loopB < z; loopB++)
            {
                zOffset = loopB * (tileSize + padding);
                spawnPos = new Vector3(anchor.position.x + xOffset, anchor.position.y, anchor.position.z + zOffset);
                currentTile = Instantiate(tile, spawnPos, anchor.rotation);
                currentTile.transform.parent = gameObject.transform;
                currentTile.GetComponent<SCR_Tile>().tileNum = tileCounter;
                tiles.Add(currentTile);
                tileCounter++;
            }
        }
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
    private void GenerateSequencePuzzle()
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

    private void GenerateComparisonPuzzle()
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

        // foreach(Transform child in comparisonSolutionTrans)
        // {
        //     if(puzzleSolution.Contains(child.GetComponent<SCR_Tile>().tileNum))
        //     {
        //         child.GetComponent<MeshRenderer>().material = debugActiveColor;
        //         //Destroy(child.GetComponent<SCR_Tile>());
        //         print("found");
        //     }
        //     else
        //     {
        //         print("null");
        //         //Destroy(child.GetComponent<SCR_Tile>());
        //     }
        // }
}

    public void CheckPuzzleState(PuzzleType type, GameObject tileCheck)
    {
        switch(type)
        {
            #region SEQUENCE CASE
            case PuzzleType.SEQUENCE:

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
                break;
            #endregion

            #region COMPARISON CASE
            case PuzzleType.COMPARISON:
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
                break;
            #endregion
        }
        
    }

    public void PuzzleSolved()
    {
        puzzleState = PuzzleState.SOLVED;
        puzzleAttempt = new List<int>();
        foreach (GameObject tile in tiles)
            {
                tile.GetComponent<SCR_Tile>().isCorrect = false;
            }
        StartCoroutine(DebugPuzzleIndicator(debugSolvedColor));
        StartCoroutine(PuzzleSolvedCooldown());
    }

    public bool IsComparisonComplete()
    {
        int count = 0;

        for(int i = 0 ; i < puzzleSolution.Count; i++)
        {
            if(puzzleAttempt.Contains(puzzleSolution[i]))
                count++;
        }

        if (count == puzzleSolution.Count && puzzleAttempt.Count == puzzleSolution.Count)
            return true;
        else
            return false;
    }

    private IEnumerator DebugPuzzleIndicator(Material mat)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<MeshRenderer>().material = mat;
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<MeshRenderer>().material = debugBaseColor;
        }
    }

    private IEnumerator PuzzleSolvedCooldown()
    {
        yield return new WaitForSeconds(1f);
        puzzleState = PuzzleState.NULL;
    }
}

public enum PuzzleState
{
    NULL,
    FAILED,
    SOLVED
}
public enum SequenceMode
{
    RESET_ON_FAIL,
    RESET_AFTER_X,
    NO_RESET
}
public enum ComparisonMode
{
    NO_RESET
}