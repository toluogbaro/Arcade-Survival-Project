using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Gameplay,
    PauseMenu,
    SettingsMenu,
    PopupUI,
    Inventory
}
public class SCR_GameManager : MonoBehaviour
{
    public static SCR_GameManager _instance;
    public static GameState _gameState;
    public static bool _menuOpen;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        if (FindObjectOfType<Movement.SCR_PlayerController>()) SetState(GameState.Gameplay);
        else SetState(GameState.PauseMenu);
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        _menuOpen = false;
    }

    public void SettingsMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        _menuOpen = true;
    }

    public void PopupUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 0f;
        _menuOpen = true;
    }

    public void Inventory()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
        _menuOpen = true;
    }

    public void PauseMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        _menuOpen = true;
    }

    public void SetState(GameState gameState)
    {
        _gameState = gameState;
        switch (_gameState)
        {
            case GameState.Gameplay:
                ResumeGame();
                break;
            case GameState.SettingsMenu:
                SettingsMenu();
                break;
            case GameState.PopupUI:
                PopupUI();
                break;
            case GameState.Inventory:
                Inventory();
                break;
            case GameState.PauseMenu:
                PauseMenu();
                break;
        }
    }
}
