using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardPatrol2D : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsCurrentPoint();

        // Check if the guard has reached the current point
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            SwitchPoint();
        }
    }

    void MoveTowardsCurrentPoint()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    void SwitchPoint()
    {
        // Switch between PointA and PointB
        if (currentPoint == PointB.transform)
        {
            currentPoint = PointA.transform;
        }
        else
        {
            currentPoint = PointB.transform;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has been caught by the guard");
            SceneManager.LoadScene("EndGame_LoseState");
        }
    }
}
