using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack: MonoBehaviour
{
    public enum RandomFeature
    {
        None,
        ScoreMultiplier_2x,
        ScoreMultiplier_5x,
        ScoreMultiplier_10x,
        UnlimitedFuel,
        Fire
    }

    //Cache
    private GameObject bulletDotPrefab;
    private Text ScoreText;
    private EnemySpawner enemySpawner;

    //Attack
    public float Damage = 1.0f;
    public int Fuel;
    public int MaxFuel = 50;

    //Score
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            ScoreText.text = score.ToString();
        }
    }
    private int score;
    private int scoreMultiplier = 1;

    //Random Feature
    private RandomFeature randomFeature = RandomFeature.None;
    private Text RandomFeatureText;
    private readonly float RandomFeatureDuration = 12.5f;
    private float currentRandomFeatureDuration = 0.0f;

    private void Start()
    {
        RandomFeatureText = GameObject.Find("Random Feature Text").GetComponent<Text>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
        bulletDotPrefab = Resources.Load<GameObject>("Prefabs/Bullet Dot");
        Score = 0;
        Fuel = MaxFuel;
    }

    private void Update()
    {
        PerformRandomFeature();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHealth>() && randomFeature != RandomFeature.UnlimitedFuel)
            Fuel = MaxFuel;

        EnemyDotManager enemy = collision.GetComponent<EnemyDotManager>();
        if(Fuel > 0 && enemy)
        {
            enemy.HitMe(Damage);
            Fuel--;
            Score += (int)(Damage * (float)scoreMultiplier);
        }
    }

    private void PerformRandomFeature()
    {
        if(randomFeature != RandomFeature.None)
        {
            if (currentRandomFeatureDuration <= Mathf.Epsilon)
                DeactivateRandomFeature();
            if (randomFeature == RandomFeature.Fire)
                Fire();

            currentRandomFeatureDuration -= Time.deltaTime;
            RandomFeatureText.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(0.0f, 1.0f, currentRandomFeatureDuration / RandomFeatureDuration));
        }
    }
    
    public void ActivateRandomFeature(RandomFeature rFeature)
    {
        randomFeature = rFeature;
        currentRandomFeatureDuration = RandomFeatureDuration;
        switch(randomFeature)
        {
            case RandomFeature.ScoreMultiplier_2x:
                RandomFeatureText.text = "Score 2X";
                scoreMultiplier = 2;
                break;
            case RandomFeature.ScoreMultiplier_5x:
                RandomFeatureText.text = "Score 5X";
                scoreMultiplier = 5;
                break;
            case RandomFeature.ScoreMultiplier_10x:
                RandomFeatureText.text = "Score 10X";
                scoreMultiplier = 10;
                break;
            case RandomFeature.UnlimitedFuel:
                RandomFeatureText.text = "UNLIMITED FUEL";
                Fuel = int.MaxValue;
                break;
            default:
                break;
        }
    }

    private void DeactivateRandomFeature()
    {
        if(randomFeature == RandomFeature.UnlimitedFuel)
            Fuel = MaxFuel;
        else if (randomFeature != RandomFeature.Fire)
            scoreMultiplier = 1;

        randomFeature = RandomFeature.None;
        RandomFeatureText.text = "";

        //It's ended.We need to activate it's spawning.
        enemySpawner.ActivateRandomFeatureSpawn(true);
    }

    private void Fire()
    {
        BulletDot bd = Instantiate(bulletDotPrefab, Vector2.zero, Quaternion.identity).GetComponent<BulletDot>();
        bd.Direction = Random.insideUnitCircle.normalized;
    }

    private void OnDisable()
    {
        RandomFeatureText.text = "";
        randomFeature = RandomFeature.None;
    }
}
