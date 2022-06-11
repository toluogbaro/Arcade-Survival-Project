using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SuicideCube : MonoBehaviour, IPuzzleInterface
{
    public void PuzzleComplete()
    {
        Destroy(gameObject);
    }
}
