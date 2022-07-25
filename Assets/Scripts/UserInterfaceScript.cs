using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserInterfaceScript : MonoBehaviour
{
    public static UserInterfaceScript Instance;

    public TMP_Text CurrentBalanceText;
    public TMP_Text DenominationText;
    public TMP_Text LastWinReadOutText;
    public TMP_Text MultiplierText;
    public Button PlayButton;
    public Button PlusButton;
    public Button MinusButton;

    public float StartingBalance = 10.00f;
    public float[] DenominationAmounts;

    public AudioSource BetAudio;

    float currentBalance;
    int denomIndex;
    float lastGameWin;
    float currentGameWin;
    int multiplier;

    GameState gameState = GameState.BET;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.BET:
                PlayButton.interactable = false;
                denomIndex = 0;
                PlusButton.interactable = true;
                MinusButton.interactable = true;
                gameState = GameState.BET;
                lastGameWin = currentGameWin;
                break;
            case GameState.PLAY:
                PlayButton.interactable = false;
                PlusButton.interactable = false;
                MinusButton.interactable = false;
                gameState = GameState.PLAY;

                currentGameWin = 0;
                currentBalance -= DenominationAmounts[denomIndex];
                break;
        }
    }
    void Start()
    {
        currentBalance = StartingBalance;
    }


    void Update()
    {
        if(gameState == GameState.BET)
        {
            CurrentBalanceText.text = "$" + (currentBalance - DenominationAmounts[denomIndex]).ToString("0.00");
        }
        if (gameState == GameState.PLAY)
        {
            
            CurrentBalanceText.text = "$" + (currentBalance).ToString("0.00");
        }

        DenominationText.text = "$" + DenominationAmounts[denomIndex].ToString("0.00");
        LastWinReadOutText.text = "Last Game Win Amount: $" + lastGameWin.ToString("0.00");
        MultiplierText.text = "x" + multiplier.ToString();
    }

    public void ShiftDenominationIndex(int direction)
    {
        direction /= Mathf.Abs(direction); // make sure it is 1 or -1

        int prevDenomIndex = denomIndex;
        denomIndex = Mathf.Clamp(denomIndex + direction, 0, DenominationAmounts.Length - 1);

        if(denomIndex != prevDenomIndex)
        {
            BetAudio.time = 0;
            BetAudio.Play();
        }

        if(currentBalance - DenominationAmounts[denomIndex] < 0 || denomIndex == 0)
        {
            PlayButton.interactable = false;
            CurrentBalanceText.color = Color.red;
        }
        else
        {
            PlayButton.interactable = true;
            CurrentBalanceText.color = Color.white;
        }
    }

    public void StartPlay()
    {
        GameManager.Instance.UpdateGameState(GameState.PLAY);
    }

    public float GetDenominationAmount()
    {
        return DenominationAmounts[denomIndex];
    }

    public void AddToCurrentBalance(float value)
    {
        currentBalance += value;
        currentGameWin += value;
    }

    public void SetMultiplier(int value)
    {
        multiplier = value;
    }
}
