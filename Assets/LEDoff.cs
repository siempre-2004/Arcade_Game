using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDoff : MonoBehaviour
{


    public SerialController serialController;
    void Start()
    {
        Debug.Log("Start called.");
        InitializeSerialController();
        if (serialController != null)
        {
            Debug.Log("resetting arduino");
            serialController.SendSerialMessage("F");
        }

    }

    void InitializeSerialController()
    {
        Debug.Log("InitializeSerialController called.");
        serialController = GameObject.Find("SerialController")?.GetComponent<SerialController>();
        if (serialController == null)
        {
            Debug.LogError("SerialController not found");
        }
    }
}
