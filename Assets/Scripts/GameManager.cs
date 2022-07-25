using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State = GameState.BET;

    public static event Action<GameState> OnGameStateChanged;

    int NumChestsGotRight = 0;
    int NumChestsWithGold = 0;
    public bool GotAllChests = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.BET);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.BET:
                NumChestsGotRight = 0;
                NumChestsWithGold = 0;
                GotAllChests = false;
                break;
            case GameState.PLAY:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void SetNumChestsWithGold(int value)
    {
        NumChestsWithGold = value;
    }

    public void IncrementNumChestGotRight()
    {
        NumChestsGotRight++;
        GotAllChests = NumChestsGotRight >= NumChestsWithGold ? true : false;
    }
}

public enum GameState
{
    BET,
    PLAY
}
