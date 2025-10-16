using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingManager : MonoBehaviour
{
    public GameObject Dialogue_Box;
    private static bool hasDialogueBeenShown = false;

    void Start()
    {
        if (!hasDialogueBeenShown)
        {
            StartCoroutine(WaitBeforeTextbox(2.0f));
        }
    }

    IEnumerator WaitBeforeTextbox(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Dialogue_Box.SetActive(true);
        hasDialogueBeenShown = true;
    }
}
