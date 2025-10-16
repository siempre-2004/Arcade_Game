/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
    public SerialController serialController;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        Debug.Log("Press A or Z to execute some actions");
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.
        // port 2
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Sending A");
            serialController.SendSerialMessage("A");
        }
        // port 3
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Sending B");
            serialController.SendSerialMessage("B");
        }
        // port 4
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Sending C");
            serialController.SendSerialMessage("C");
        }
        // port 5
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Sending D");
            serialController.SendSerialMessage("D");
        }
        // port 6
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Sending E");
            serialController.SendSerialMessage("E");
        }


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            ; // Debug.Log("Connection attempt failed or disconnection detected"); // turn on if you have the arduino and want to debug.
        else
            ;// Debug.Log("Message arrived: " + message);
    }
}
