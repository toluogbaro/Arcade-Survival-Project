using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleInterface
{
    public void PuzzleComplete();
    public void InvokeAllMethods()
    {
        PuzzleComplete();
    }
}
