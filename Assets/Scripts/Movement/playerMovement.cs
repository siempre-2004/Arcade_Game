using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public AudioSource footstepClip;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (footstepClip == null)
        {
            Debug.LogError("Footstep AudioSource not assigned!");
        }
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        MovePlayer(movement);

        if (movement.magnitude > 0)
        {
            if (!isMoving)
            {
                isMoving = true;
                StartWalkingSound();
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                StopWalkingSound();
            }
        }
    }

    private void MovePlayer(Vector2 movement)
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void StartWalkingSound()
    {
        if (footstepClip != null && !footstepClip.isPlaying)
        {
            footstepClip.loop = true;
            footstepClip.Play();
        }
    }

    private void StopWalkingSound()
    {
        if (footstepClip != null && footstepClip.isPlaying)
        {
            footstepClip.loop = false;
            footstepClip.Stop();
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
