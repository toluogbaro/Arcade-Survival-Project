using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager _instance;

    public KeyBindings keyBindings;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public bool GetKeyDown(string key)
    {
        if (Input.GetKeyDown(keyBindings.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool KeyUp(string key)
    {
        if (Input.GetKeyUp(keyBindings.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetKey(string key)
    {
        if (Input.GetKey(keyBindings.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
