using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Cache
    Rigidbody2D rigidBody;
    CircleCollider2D circleCollider2D;

    //Config
    private bool IsMoving = false;
    private const float MinToCenterSpeed = 0.01f;
    private const float MaxToCenterSpeed = 12.5f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (Input.touchCount != 1)
            IsMoving = false;
        else
        {
            if (!IsMoving &&
                Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)) == circleCollider2D)
            {
                IsMoving = true;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                IsMoving = false;
            }
        }

    }

    private void FixedUpdate()
    {
#if DEBUG
        rigidBody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
#else
        if (IsMoving)
            rigidBody.MovePosition(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
        else if (rigidBody.position != Vector2.zero)
        {
            float currentToCenterSpeed = Mathf.Lerp( MinToCenterSpeed, 
                                                    MaxToCenterSpeed, 
                                                    Vector2.Distance(rigidBody.position, Vector2.zero) / 6.0f);
            rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position,
                                                       Vector2.zero,
                                                       currentToCenterSpeed * Time.deltaTime));
        }
#endif
    }
}