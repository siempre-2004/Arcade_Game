using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupMessage : MonoBehaviour
{
    public TextMeshPro messageText;
    public float displayDuration = 3f; 

    private float timer;
    private bool isShowingMessage;

    void Update()
    {
        if (isShowingMessage)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                HideMessage();
            }
        }
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);
        timer = displayDuration;
        isShowingMessage = true;
    }

    public void HideMessage()
    {
        gameObject.SetActive(false);
        isShowingMessage = false;
    }
}
