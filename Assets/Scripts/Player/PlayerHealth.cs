using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Cache
    GameOverManager gameOverManager;
    SpriteRenderer spriteRenderer;
    EnemySpawner enemySpawner;

    //Attributes
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            health = Mathf.Clamp(health, 0, MaxHealth);
            UpdateColor();
        }
    }
    public int MaxHealth { get; private set; } = 100;

    private void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        Health = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<PlayerAttack>())
        {
            if (Health > 0)
            {
                EnemyDotManager enemy = collision.GetComponent<EnemyDotManager>();
                if (enemy)
                {
                    Health -= (int)enemy.Health;
                    enemySpawner.AliveEnemyCount--;
                    if (Health <= 0)
                    {
                        Destroy(collision.gameObject);
                        StartCoroutine(gameOverManager.GameOver());
                    }
                }

                HealthBoost healthBoost = collision.GetComponent<HealthBoost>();
                if (healthBoost)
                    Health += healthBoost.Health;

                Destroy(collision.gameObject);
            }
            else
            {
                //If game is over, this destroys all game objects except player end node.
                Destroy(collision.gameObject);
            }
        }
    }

    private void UpdateColor()
    {
        float newBlackFactor = Mathf.Lerp(1.0f,0.0f,(float)Health / (float)MaxHealth);
        spriteRenderer.color = new Vector4(newBlackFactor, newBlackFactor, newBlackFactor, 1.0f);
    }
}


