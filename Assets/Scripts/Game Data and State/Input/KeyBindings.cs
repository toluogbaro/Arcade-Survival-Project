using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "KeyBindings")]
public class KeyBindings : ScriptableObject
{

    public KeyCode Jump, Sprint, Inventory, Pause;


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

            case "Pause":
                return Pause;

            default:
                return KeyCode.None;
        }

    }

}
