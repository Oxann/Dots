using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDot : MonoBehaviour
{
    //Cache
    private Rigidbody2D rigidBody;
    private static PlayerAttack playerAttack = null;

    private const float Damage = 1.0f;
    public Vector2 Direction;
    private float Speed = 10.0f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (!playerAttack) playerAttack = FindObjectOfType<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.MovePosition(rigidBody.position + Direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<EnemyDotManager>().HitMe(Damage);
        playerAttack.Score += (int)Damage;
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
