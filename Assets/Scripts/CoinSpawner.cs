using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public float TimeBeforeSpawn = .4f;
    public float MinCoinSpawnRate;
    public float MaxCoinSpawnRate;
    public AnimationCurve CoinSpawnRateCurve;
    public float TimeToReachMaxSpawnRate;
    public GameObject CoinPrefab;
    public GameObject ExplosionPrefab;
    public AudioClip[] CoinSoundClips;
    int soundClipIndex = -1;

    public AudioSource KickChest;

    public float MoneyValue;

    public int NumBronzeCoins;
    public int NumSilverCoins;
    public int NumGoldCoins;
    public int NumDiamondCoins;

    private float timeElapsed;
    private float totalTimeElapsed;
    private float oneOverTimeToReachMaxSpawnRate;
    private int numCoinsSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        float moneyDistributionValue = MoneyValue;
        NumDiamondCoins = (int)(moneyDistributionValue / 100f);
        moneyDistributionValue -= NumDiamondCoins * 100f;
        NumGoldCoins = (int)(moneyDistributionValue / 10f);
        moneyDistributionValue -= NumGoldCoins * 10f;
        NumSilverCoins = (int)moneyDistributionValue;
        moneyDistributionValue -= NumSilverCoins;
        
        int helpWithBronze = Mathf.RoundToInt(moneyDistributionValue*100);
        NumBronzeCoins = helpWithBronze / 5;

        oneOverTimeToReachMaxSpawnRate = 1 / TimeToReachMaxSpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        totalTimeElapsed += Time.deltaTime;

        float t = CoinSpawnRateCurve.Evaluate(totalTimeElapsed * oneOverTimeToReachMaxSpawnRate);
        float spawnRate = Mathf.Lerp(MinCoinSpawnRate, MaxCoinSpawnRate, t);

        if (timeElapsed > spawnRate && totalTimeElapsed > TimeBeforeSpawn)
        {
            timeElapsed = 0;
            if (numCoinsSpawned >= NumBronzeCoins + NumSilverCoins + NumGoldCoins + NumDiamondCoins)
            {
                if (MoneyValue == 0 || GameManager.Instance.GotAllChests)
                {
                    if(MoneyValue == 0)
                    {
                        Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
                        GameObject explosion = Instantiate(ExplosionPrefab, toCamera + transform.position, Quaternion.identity);
                        explosion.transform.LookAt(Camera.main.transform.position);
                    }
                    GameManager.Instance.UpdateGameState(GameState.BET);
                }
                transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                transform.parent.gameObject.GetComponent<Rigidbody>().useGravity = true;
               
                transform.parent.gameObject.GetComponent<WaterFloating>().enabled = false;

                Vector3 expodePosition = new Vector3(Random.Range(-3f, 3f), -2f, 2) +transform.position;
                transform.parent.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50, expodePosition, 20, 0, ForceMode.Impulse);

                KickChest.time = 0;
                KickChest.Play();



                
                gameObject.SetActive(false);
            }
            else
            {
                GameObject coinObj = Instantiate(CoinPrefab, transform.position, transform.rotation);

                if (numCoinsSpawned % 3 == 0)
                    soundClipIndex++;

                soundClipIndex = Mathf.Clamp(soundClipIndex, 0, CoinSoundClips.Length -1);

                coinObj.GetComponent<AudioSource>().clip = CoinSoundClips[soundClipIndex];
                coinObj.GetComponent<AudioSource>().Play();

                if (numCoinsSpawned < NumBronzeCoins)
                {
                    coinObj.GetComponent<CoinScript>().TypeOfCoin = CoinType.BRONZE;
                }
                else if (numCoinsSpawned < NumBronzeCoins + NumSilverCoins)
                {
                    coinObj.GetComponent<CoinScript>().TypeOfCoin = CoinType.SILVER;
                }
                else if (numCoinsSpawned < NumBronzeCoins + NumSilverCoins + NumGoldCoins)
                {
                    coinObj.GetComponent<CoinScript>().TypeOfCoin = CoinType.GOLD;
                }
                else if (numCoinsSpawned < NumBronzeCoins + NumSilverCoins + NumGoldCoins + NumDiamondCoins)
                {
                    soundClipIndex = CoinSoundClips.Length - 1;
                    coinObj.GetComponent<CoinScript>().TypeOfCoin = CoinType.DIAMOND;
                }

            }
            numCoinsSpawned++;

        }
    }
}
