using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "KeyBindings")]
public class KeyBindings : ScriptableObject
{

    public KeyCode Jump, Sprint, Inventory;


    public KeyCode CheckKey(string key)
    {
        switch (key)
        {

            case "Jump":
                return Jump;

            case "Sprint":
                return Sprint;

            case "Inventory":
                return Inventory;

            default:
                return KeyCode.None;
        }

    }

}
