using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stealthMeter : MonoBehaviour
{


    public GameObject guard;
    private guardSight guardSightScript;

    private float lightThreshold = 0.0f;

    SpriteRenderer spriteRenderer;

    public float lightNumber = 1.0f;

    private bool detected = false;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //StartCoroutine(AssignGuardDelayed());
    }

    //private IEnumerator AssignGuardDelayed()
    //{
    //    yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
    //    guard = GameObject.FindGameObjectWithTag("guard");
    //    guardSightScript = guard.GetComponent<guardSight>();
    //    lightThreshold = guardSightScript.detectionLimit / 5 * lightNumber;
    //    Debug.Log("guardSet");
    //}

    void Update()
    {
        if (guard == null && GameObject.FindGameObjectWithTag("guard") != null)
        {
            guard = GameObject.FindGameObjectWithTag("guard");
            guardSightScript = guard.GetComponent<guardSight>();
            lightThreshold = guardSightScript.detectionLimit / 5 * lightNumber;
            Debug.Log(lightThreshold);
            Debug.Log("guardSet");
        }

        if (guard != null && guardSightScript.detectionTime >= lightThreshold && !detected)
        {
            detected = true;
            spriteRenderer.color = Color.yellow;
            Debug.Log("color changed");
        }

        if (detected && guardSightScript.detectionTime < lightThreshold) 
        {
            detected = false;
            spriteRenderer.color = Color.white;
        }
        
    }
}
