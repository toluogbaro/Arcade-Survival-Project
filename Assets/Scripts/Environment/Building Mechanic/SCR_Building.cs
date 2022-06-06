using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Building : MonoBehaviour
{
    //[SerializeField] private int gridZ = 3; //grid size of tiles in Z axis
    //[SerializeField] private int gridX = 3; //grid size of tiles in X axis
    //[SerializeField] private float gridPadding = 0.5f; //padding between tiles
    //[SerializeField] private float tileSizeRef = 2f; //reference for the size of the puzzle tile
    [SerializeField] private GameObject tilePrefab; //prefab of puzzle tile
    //[SerializeField] private Transform puzzleAnchorTrans; //transform of puzzle anchor *SUBJECT TO CHANGE
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();


    //Non-Serialized Variables
    private Vector3 spawnPos = Vector3.zero; //reference for the current spawn position
    private GameObject currentTile; //reference to current tile in generation
    [SerializeField] private int xPos;
    [SerializeField] private int zPos;
    [SerializeField] private float padding;
    [SerializeField] private float tileSize;
    private float xOffset = 0f; //spawn offset for X axis
    private float zOffset = 0f; //spawn offset for Z axis

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) GenerateTiles(xPos, zPos, padding, tileSize, transform);

        if (Input.GetMouseButton(1)) DeleteTiles();
    }

    private void GenerateTiles(int x, int z, float padding, float tileSize, Transform anchor)
    {
        for (int loopA = 0; loopA < x; loopA++)
        {
            xOffset = loopA * (tileSize + padding);

            for (int loopB = 0; loopB < z; loopB++)
            {
                zOffset = loopB * (tileSize + padding);
                spawnPos = new Vector3(anchor.position.x + xOffset, anchor.position.y, anchor.position.z + zOffset);
                currentTile = Instantiate(tilePrefab, spawnPos, tilePrefab.transform.rotation);
                currentTile.transform.parent = gameObject.transform;
                tiles.Add(currentTile);

            }
        }
    }

    private void DeleteTiles()
    {
        foreach (GameObject tile in tiles) Destroy(tile);
        tiles.Clear();
    }
}
