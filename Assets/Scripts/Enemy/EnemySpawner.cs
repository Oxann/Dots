using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Cache
    GameObject EnemyDotPrefab;
    GameObject HealthBoostSpawnerEnemyDot;
    GameObject FuelBoostEnemyDot;
    GameObject RandomFeatureEnemyDot;
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;

    //Config
    public int TotalSpawnedEnemies;
    public int AliveEnemyCount;
    public int MaxEnemyCount;
    public int MinEnemyHealth;
    public int MaxEnemyHealth;
    public float MinEnemySpeed;
    public float MaxEnemySpeed;
    public float MinScaleFactor;
    public float MaxScaleFactor;

    //Levels
    private int HealthAndSpeedLevel = 0;
    private int CountLevel = 0;
    private readonly float SpeedIncFactor = 0.01f;
    private readonly int HealthIncFactor = 1;

    //Spawn Rates
    private float RandomFeatureSpawnRate = 0.05f;
    private float HealthBoostSpawnRate = 0.01f;
    private float FuelBoostSpawnRate = 0.00f;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        EnemyDotPrefab = (GameObject)Resources.Load("Prefabs/Enemy Dot");
        HealthBoostSpawnerEnemyDot = (GameObject)Resources.Load("Prefabs/Enemy Dot(Player Health Boost)");
        FuelBoostEnemyDot = Resources.Load<GameObject>("Prefabs/Enemy Dot(Player Fuel Boost)");
        RandomFeatureEnemyDot = Resources.Load<GameObject>("Prefabs/Enemy Dot(Random Feature)");
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        ConfigureSpawnLevels();
        ConfigureSpawnRates();
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (AliveEnemyCount < MaxEnemyCount)
        {
            Vector2 spawnPos = Random.insideUnitCircle.normalized * 8.0f;
            //Transforming spawPos into a rectangle
            spawnPos.x = Mathf.Clamp(spawnPos.x, -4.2f, 4.2f);
            spawnPos.y = Mathf.Clamp(spawnPos.y, -6.5f, 6.5f);

            EnemyDotManager newEnemy = Instantiate(ChooseNewEnemy(),
                                    spawnPos,
                                    Quaternion.identity,
                                    transform).GetComponent<EnemyDotManager>();
            ConfigureNewEnemy(newEnemy);
            AliveEnemyCount++;
            TotalSpawnedEnemies++;
        }
    }

    private void ConfigureSpawnLevels()
    {
        //Enemy Health And Speeed Configuration
        int newHealthAndSpeedLevel = playerAttack.Score / 1000;
        if (newHealthAndSpeedLevel > HealthAndSpeedLevel)
        {
            HealthAndSpeedLevel++;
            MinEnemySpeed += SpeedIncFactor;
            MaxEnemySpeed += SpeedIncFactor;
            MinEnemyHealth += HealthIncFactor;
            MaxEnemyHealth += HealthIncFactor;
            FuelBoostSpawnRate += 0.0015f;
        }

        //Max Enemy Count Configuration
        if (playerAttack.Score > 75 && CountLevel == 0)
        {
            CountLevel = 1;
            MaxEnemyCount = 2;
        }
        else if (playerAttack.Score > 500 && CountLevel == 1)
        {
            CountLevel = 2;
            MaxEnemyCount = 3;
        }
        else if (playerAttack.Score > 2500 && CountLevel == 2)
        {
            CountLevel = 3;
            MaxEnemyCount = 4;
        }
        else if (playerAttack.Score > 10000 && CountLevel == 3)
        {
            CountLevel = 4;
            MaxEnemyCount = 5;
        }
        else if (playerAttack.Score > 25000 && CountLevel == 4)
        {
            CountLevel = 5;
            MaxEnemyCount = 6;
        }
        else if (playerAttack.Score > 75000 && CountLevel == 4)
        {
            CountLevel = 6;
            MaxEnemyCount = 7;
        } 
    }

    private GameObject ChooseNewEnemy()
    {
        float SpawnRNG = Random.value;
        if (SpawnRNG <= FuelBoostSpawnRate)
            return FuelBoostEnemyDot;
        else if (FuelBoostSpawnRate < SpawnRNG && SpawnRNG <= FuelBoostSpawnRate + HealthBoostSpawnRate)
            return HealthBoostSpawnerEnemyDot;
        else if (FuelBoostSpawnRate + HealthBoostSpawnRate < SpawnRNG && FuelBoostSpawnRate + HealthBoostSpawnRate + RandomFeatureSpawnRate >= SpawnRNG)
        {
            //We don't want more than one random feature at the same time.
            //It's spawning will be activated after this one ends.
            ActivateRandomFeatureSpawn(false);
            return RandomFeatureEnemyDot;
        }
        else
            return EnemyDotPrefab;
    }

    private void ConfigureNewEnemy(EnemyDotManager newEnemy)
    {
        newEnemy.SpawnHealth = Random.Range(MinEnemyHealth, MaxEnemyHealth);
        newEnemy.SpawnSpeed = Random.Range(MinEnemySpeed, MaxEnemySpeed);
        float scaleFactor = Random.Range(MinScaleFactor, MaxScaleFactor);
        newEnemy.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }

    private void ConfigureSpawnRates()
    {
        //Health Boost Spawn Rate Config
        if(playerHealth.Health / playerHealth.MaxHealth <= 0.3f)
            HealthBoostSpawnRate = 0.05f;
        else
            HealthBoostSpawnRate = 0.01f;
    }

    public void ActivateRandomFeatureSpawn(bool value)
    {
        if (value)
            RandomFeatureSpawnRate = 0.05f;
        else
            RandomFeatureSpawnRate = 0.00f;
    }
}