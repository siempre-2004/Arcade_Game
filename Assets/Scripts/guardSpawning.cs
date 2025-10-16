using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardSpawning : MonoBehaviour
{
    public GameObject guard;

    [SerializeField] private float maxSpawnDistance = 2;

    private Vector2 buildingPos = Vector2.zero;
    private Vector2 spawnPosition = Vector2.zero;

    private Quaternion guardRotation = Quaternion.identity;

    private int facingDirection = 0;

    public int guardsSpawned = 2;

    private void Start()
    {
        buildingPos = transform.position;

        // spawn Debugging
        for (int spawnedCount = 0; spawnedCount < guardsSpawned; spawnedCount++)
        {
            spawnGuard();
        }
    }

    private void Update()
    {

    }

    void spawnGuard()
    {
        float spawnPosX = Random.Range(-maxSpawnDistance, maxSpawnDistance);
        float spawnPosY = Random.Range(-maxSpawnDistance, -3);

        spawnPosition = new Vector2(spawnPosX + buildingPos.x, spawnPosY + buildingPos.y);

        if (Vector2.Distance(spawnPosition, buildingPos) < maxSpawnDistance && !Physics2D.OverlapCircle(spawnPosition, 0.3f))
        {
            if (spawnPosX > 0)
            {
                facingDirection = 3; // right
                Debug.Log("right");
            }
            else
            {
                facingDirection = 4; // left
                Debug.Log("left");
            }

            GameObject newGuard = Instantiate(guard, spawnPosition, guardRotation);
            guardSight guardSightScript = newGuard.GetComponent<guardSight>();
            if (guardSightScript != null)
            {
                guardSightScript.facingDirection = facingDirection;
                guardSightScript.InitializeDirection();
            }
        }
        else
        {
            spawnGuard();
        }
    }
}
