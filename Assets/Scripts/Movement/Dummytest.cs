using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummytest : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 moveVelocity = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveVelocity += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVelocity += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVelocity += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVelocity += Vector2.down;
        }
        rb.velocity = moveVelocity * speed;
    }
}
