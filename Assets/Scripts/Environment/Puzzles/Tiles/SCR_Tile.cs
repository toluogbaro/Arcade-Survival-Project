using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SCR_Tile : MonoBehaviour
{
    public bool isActive; //bool for status
    public int tileNum; //tile number in puzzle
    public bool isCorrect = false;
    protected abstract void OnTriggerEnter(Collider collider);
    protected abstract void OnTriggerExit(Collider collider);
}
