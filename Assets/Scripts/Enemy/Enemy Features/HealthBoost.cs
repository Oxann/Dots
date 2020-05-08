using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    //Cache
    Rigidbody2D rigidBody;
    
    public int Health;
    private const float Speed = 2.0f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, Vector2.zero, Speed * Time.deltaTime));
    }
}
