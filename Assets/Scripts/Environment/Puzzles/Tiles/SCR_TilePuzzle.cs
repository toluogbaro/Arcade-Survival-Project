using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class SCR_TilePuzzle : MonoBehaviour
{
    //Serialized Variables
    //Generation
    [SerializeField] protected int gridZ = 3; //grid size of tiles in Z axis
    [SerializeField] protected int gridX = 3; //grid size of tiles in X axis
    [SerializeField] protected float gridPadding = 0.5f; //padding between tiles
    [SerializeField] protected float tileSizeRef = 2f; //reference for the size of the puzzle tile
    [SerializeField] protected GameObject tilePrefab; //prefab of puzzle tile
    [SerializeField] protected Transform puzzleAnchorTrans; //transform of puzzle anchor *SUBJECT TO CHANGE
    [SerializeField] protected List<GameObject> tiles = new List<GameObject>();
    protected float xOffset = 0f; //spawn offset for X axis
    protected float zOffset = 0f; //spawn offset for Z axis
    protected Vector3 spawnPos = Vector3.zero; //reference for the current spawn position

    //Puzzle
    public Material debugBaseColor;
    public Material debugActiveColor;
    public Material debugFailedColor;
    public Material debugSolvedColor;
    public List<int> puzzleSolution = new List<int>();
    [SerializeField] protected GameObject currentTile; //reference to current tile in generation
    [SerializeField] protected int tileCounter = 0;
    public List<int> puzzleAttempt = new List<int>();
    public PuzzleState puzzleState;
    [SerializeField] private GameObject[] puzzleInterfaces;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles(gridX, gridZ, gridPadding, tilePrefab, tileSizeRef, puzzleAnchorTrans);
        
        GeneratePuzzle();
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
    protected abstract void GeneratePuzzle();
   
    public abstract void CheckPuzzleState(GameObject tileCheck);

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

        foreach (GameObject puzzleInterface in puzzleInterfaces)
        {
            if(puzzleInterface.GetComponent<IPuzzleInterface>() != null)
                puzzleInterface.GetComponent<IPuzzleInterface>().PuzzleComplete();
        }
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

    public IEnumerator DebugPuzzleIndicator(Material mat)
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

    public IEnumerator PuzzleSolvedCooldown()
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