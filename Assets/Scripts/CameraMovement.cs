using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform mainCharacterTransform;
    public float offset;
    public float damping;
    public Vector3 vel = Vector3.zero;
    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetPosition.x = mainCharacterTransform.position.x + offset;
        targetPosition.y = mainCharacterTransform.position.y;


      

        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
    }
}
