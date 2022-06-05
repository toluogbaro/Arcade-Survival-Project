using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SCR_PuzzleGenerator : MonoBehaviour
{
    //Serialized Variables
    [Header("Generation Settings")]
    [SerializeField] private int gridZ = 3; //grid size of tiles in Z axis
    [SerializeField] private int gridX = 3; //grid size of tiles in X axis
    [SerializeField] private float gridPadding = 0.5f; //padding between tiles
    [SerializeField] private float tileSizeRef = 2f; //reference for the size of the puzzle tile
    [SerializeField] private GameObject tilePrefab; //prefab of puzzle tile
    [SerializeField] private Transform puzzleAnchorTrans; //transform of puzzle anchor *SUBJECT TO CHANGE
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    [Header("Puzzle Settings")]
    public Material debugBaseColor;
    public Material debugActiveColor;
    public Material debugFailedColor;
    public Material debugSolvedColor;
    [SerializeField] private List<int> puzzleSolution= new List<int>();
    public List<int> puzzleAttempt = new List<int>();
    private PuzzleState puzzleState;


    //Non-Serialized Variables
    private Vector3 spawnPos = Vector3.zero; //reference for the current spawn position
    private GameObject currentTile; //reference to current tile in generation
    private float xOffset = 0f; //spawn offset for X axis
    private float zOffset = 0f; //spawn offset for Z axis
    private int tileCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles(gridX, gridZ, gridPadding, tilePrefab, tileSizeRef, puzzleAnchorTrans);
        GenerateSequencePuzzle();
    }

    private void GenerateTiles(int x, int z, float padding, GameObject tile, float tileSize, Transform anchor)
    {
        for (int loopA = 0; loopA < x; loopA++)
        {
            xOffset = loopA * (tileSize + padding);

            for (int loopB = 0; loopB < z; loopB++)
            {
                zOffset = loopB * (tileSize + padding);
                spawnPos = new Vector3(xOffset, anchor.position.y, anchor.position.z + zOffset);
                currentTile = Instantiate(tile, spawnPos, anchor.rotation);
                currentTile.transform.parent = gameObject.transform;
                currentTile.GetComponent<SCR_PuzzleTile>().tileNum = tileCounter;
                tiles.Add(currentTile);
                tileCounter++;
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

    public void CheckPuzzleState()
    {
        for (int i = 0; i < puzzleAttempt.Count; i++)
        {
            if (puzzleAttempt[i] == puzzleSolution[i])
            {
                puzzleState = PuzzleState.OK;
            }
            else
            {
                puzzleAttempt = new List<int>();
                puzzleState = PuzzleState.FAILED;
                StartCoroutine(DebugPuzzleIndicator(debugFailedColor));
            }
        }

        if(puzzleAttempt.Count == puzzleSolution.Count && puzzleState == PuzzleState.OK)
            {
                puzzleState = PuzzleState.SOLVED;
                puzzleAttempt = new List<int>();
                StartCoroutine(DebugPuzzleIndicator(debugSolvedColor));
            }
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
}

public enum PuzzleState
{
    OK,
    FAILED,
    SOLVED
}
