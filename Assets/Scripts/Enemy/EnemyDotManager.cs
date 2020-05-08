using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDotManager : MonoBehaviour
{
    //Cache
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;
    IEnemyFeature feature;
    static EnemySpawner enemySpawner = null;

    //Attributes
    private float Speed;
    public float SpawnSpeed;
    public float Health { get; private set; }
    public float SpawnHealth;

    //Path
    List<Vector2> Path = new List<Vector2>();
    private int currentDestination = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        if(!enemySpawner) enemySpawner = FindObjectOfType<EnemySpawner>();
        feature = GetComponent<IEnemyFeature>();
        Health = SpawnHealth;
        Speed = SpawnSpeed;
        GeneratePath();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, Path[currentDestination], Speed * Time.deltaTime));
        if (rigidBody.position == Path[currentDestination])
            currentDestination++;
    }

    public void HitMe(float damage)
    {
        if(enabled)
        {
            Health -= damage;
            if (Health <= Mathf.Epsilon)
                Die();
            else
                Speed = Mathf.Lerp(0.5f, SpawnSpeed, Health / SpawnHealth);

            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        float newBlackFactor = Health / SpawnHealth;
        newBlackFactor = Mathf.Clamp(newBlackFactor, 0.0f, 1.0f);
        spriteRenderer.color = new Vector4(newBlackFactor, newBlackFactor, newBlackFactor, 1.0f);
    }

    public void Die()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        enemySpawner.AliveEnemyCount--;
        gameObject.layer = 10; //Dead Enemy Layer
        enabled = false;
        if (feature != null)
            feature.Activate();
    }

    private void GeneratePath()
    {    
        for (int destinationCount = Random.Range(1, 4); destinationCount > 0; destinationCount--)
        {
             Path.Add(Random.insideUnitCircle * 2.4f);
        }
        Path.Add(Vector2.zero);
    }
}
