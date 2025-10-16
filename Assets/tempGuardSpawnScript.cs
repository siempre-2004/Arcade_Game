using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class tempGuardSpawnScript : MonoBehaviour
{
    public GameObject guard;
    public Vector2 positionOffset = Vector2.up;
    public float delayInSeconds = 0.1f;


    private void Start()
    {
        GameObject newGuard = Instantiate(guard, (Vector2)transform.position + positionOffset, transform.rotation);
    }
    //IEnumerator Start()
    //{
    //    // Wait for the specified delay
    //    yield return new WaitForSeconds(delayInSeconds);

    //    // Spawn the guard after the delay
    //    GameObject newGuard = Instantiate(guard, (Vector2)transform.position + positionOffset, transform.rotation);
    //}

    //private void Update()
    //{
    //    if (GameObject.activeSelf) {

    //    }
    //}
}


