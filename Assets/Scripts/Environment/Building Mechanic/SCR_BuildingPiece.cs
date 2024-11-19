using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Piece", menuName = "Building/Building Piece")]
public class SCR_BuildingPiece : ScriptableObject
{
    [Header("Piece Info")]
    public string buildingName;

    [TextArea(5, 10)] public string buildingDescription;
    public int width;
    public int height;
    public int cost;
    public int durability;

    [Header("Physical Piece")]
    public GameObject ghostPiece;
    public GameObject physicalPiece;
}
