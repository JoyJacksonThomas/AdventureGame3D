using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance;

    public int BombMultiplierChance = 50;
    public int BronzeMultiplierChance = 30;
    public int SilverMultiplierChance = 15;
    public int GoldMultiplierChance = 5;

    public int[] BronzeMultiplierValues = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public int[] SilverMultiplierValues = {12, 16, 24, 32, 48, 64 };
    public int[] GoldMultiplierValues = {100, 200, 300, 400, 500 };

    public int multiplierTier;
    public int multiplier = 0;
    public float totalInChests = 0;

    public GameObject ChestPrefab;

    public Transform[] ChestSpawners;

    ChestScript[] _chests;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        
            _chests = new ChestScript[ChestSpawners.Length];
    }

    void GameManagerOnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.BET:
                if (_chests == null)
                    return;

                for(int i = 0; i < _chests.Length; i++)
                {
                    Destroy(_chests[i].gameObject);
                }
                break;
            case GameState.PLAY:
                
                multiplierTier = Random.Range(1, BombMultiplierChance + BronzeMultiplierChance + SilverMultiplierChance + GoldMultiplierChance);
                multiplier = 0;
                if(multiplierTier <= BombMultiplierChance) { }
                else if (multiplierTier <= BombMultiplierChance + BronzeMultiplierChance) 
                {
                    int multiplierIndex = Random.Range(0, BronzeMultiplierValues.Length);
                    multiplier = BronzeMultiplierValues[multiplierIndex];
                }
                else if (multiplierTier <= BombMultiplierChance + BronzeMultiplierChance + SilverMultiplierChance)
                {
                    int multiplierIndex = Random.Range(0, SilverMultiplierValues.Length);
                    multiplier = SilverMultiplierValues[multiplierIndex];
                }
                else if (multiplierTier <= BombMultiplierChance + BronzeMultiplierChance + SilverMultiplierChance + GoldMultiplierChance)
                {
                    int multiplierIndex = Random.Range(0, GoldMultiplierValues.Length);
                    multiplier = GoldMultiplierValues[multiplierIndex];
                }

                for(int i = 0; i < _chests.Length; i++)
                {
                    _chests[i] = Instantiate(ChestPrefab, ChestSpawners[i].position, ChestSpawners[i].rotation).GetComponent<ChestScript>();
                }

                UserInterfaceScript.Instance.SetMultiplier(multiplier);
                float denom = UserInterfaceScript.Instance.GetDenominationAmount();
                totalInChests = denom * multiplier;
                DistributeMultiplierAmongChests();
                break;
        }
    }

    void DistributeMultiplierAmongChests()
    {
        int totalNumTokens = (int)(totalInChests / .05f) + 1;
        int numTokensDistributed = 0;

        int numChestsWithGold = Random.Range(1, ChestSpawners.Length - 1);        
        numChestsWithGold = Mathf.Clamp(numChestsWithGold, 1, totalNumTokens);

        GameManager.Instance.SetNumChestsWithGold(numChestsWithGold);

        List<int> chestIndeces = new List<int>();

        while(chestIndeces.Count < numChestsWithGold)
        {
            int chestIndex = Random.Range(0, ChestSpawners.Length - 1);
            if(!chestIndeces.Contains(chestIndex))
            {
                chestIndeces.Add(chestIndex);
                int numTokensForThisChest = Random.Range(1, (totalNumTokens - numTokensDistributed) - (numChestsWithGold - chestIndeces.Count));
                numTokensDistributed += numTokensForThisChest;

               if (chestIndeces.Count == numChestsWithGold)    //put remainder in last chest
                {
                    numTokensForThisChest += totalNumTokens - numTokensDistributed;
                }
                   

                _chests[chestIndex].SetChestAmount(numTokensForThisChest * .05f);
            }
        }
    }

}
 