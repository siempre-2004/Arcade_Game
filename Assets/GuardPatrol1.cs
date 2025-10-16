using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrol2 : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (currentPoint.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);

        float distanceToCurrentPoint = Vector3.Distance(transform.position, currentPoint.position);
        if (distanceToCurrentPoint < 0.1f)
        {
            if (currentPoint == PointB.transform)
            {
                currentPoint = PointA.transform;
            }
            else
            {
                currentPoint = PointB.transform;
            }
        }
    }
}
